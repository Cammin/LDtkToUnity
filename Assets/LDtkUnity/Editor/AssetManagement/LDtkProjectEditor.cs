using System.IO;
using System.Linq;
using LDtkUnity.Data;
using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkProject))]
    public class LDtkProjectEditor : UnityEditor.Editor
    {
        private LDtkDataProject? _data;

        private Vector2 _currentScroll;
        private bool _dropdown;

        //private string ProjectPath => Path.GetDirectoryName(AssetDatabase.GetAssetPath(((LDtkProject)target).ProjectJson));
        
        public override void OnInspectorGUI()
        {
            SerializedObject serializedObj = new SerializedObject(target);
            ShowGUI(serializedObj);
            serializedObj.ApplyModifiedProperties();
            
            _dropdown = EditorGUILayout.Foldout(_dropdown, "Internal Data");
            if (_dropdown)
            {
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                base.OnInspectorGUI();
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
        }
        

        private void ShowGUI(SerializedObject serializedObj)
        {
            SerializedProperty textProp = serializedObj.FindProperty(LDtkProject.PROP_JSON);
            
            DrawWelcomeMessage(textProp);
            if (!AssignJsonField(textProp) || _data == null)
            {
                return;
            }
            EditorGUILayout.Space();

            LDtkDataProject projectData = _data.Value;
            
            SerializedProperty gridPrefabProp = serializedObj.FindProperty(LDtkProject.PROP_TILEMAP_PREFAB);
            EditorGUILayout.PropertyField(gridPrefabProp);
            
            DrawLevels(projectData.levels);
            
            EditorGUILayout.Space();
            
            SerializedProperty intGridVisibilityProp = serializedObj.FindProperty(LDtkProject.PROP_INTGRIDVISIBLE);
            EditorGUILayout.PropertyField(intGridVisibilityProp);
            
            SerializedProperty intGridProp = serializedObj.FindProperty(LDtkProject.PROP_INTGRID);
            intGridProp.arraySize = projectData.defs.layers.SelectMany(p => p.intGridValues).Distinct().Count() - 1;
            DrawLayers(projectData.defs.layers, intGridProp);
            
            EditorGUILayout.Space();
            
            SerializedProperty entitiesProp = serializedObj.FindProperty(LDtkProject.PROP_ENTITIES);
            entitiesProp.arraySize = projectData.defs.entities.Length;
            DrawEntities(projectData.defs.entities, entitiesProp);
            
            EditorGUILayout.Space();
            
            SerializedProperty tilesetsProp = serializedObj.FindProperty(LDtkProject.PROP_TILESETS);
            tilesetsProp.arraySize = projectData.defs.tilesets.Length;
            DrawTilesets(projectData.defs.tilesets, tilesetsProp);
            
            EditorGUILayout.Space();
            
            if (projectData.defs.enums.Length > 0)
            {
                GenerateEnumsButton(projectData, serializedObj);
            }
            DrawEnums(projectData.defs.enums);
        }

        private void GenerateEnumsButton(LDtkDataProject projectData, SerializedObject serializedObj)
        {
            string projectName = serializedObj.targetObject.name;
            
            string targetPath = AssetDatabase.GetAssetPath(target);
            targetPath = Path.GetDirectoryName(targetPath);
            
            bool fileExists = LDtkEnumFactory.AssetExists(targetPath, projectName);
            string buttonMessage = fileExists ? "Update Enums" : "Generate Enums";

            if (GUILayout.Button(buttonMessage))
            {
                LDtkEnumGenerator.GenerateEnumScripts(projectData.defs.enums, targetPath, serializedObj.targetObject.name);
            }
        }

        private bool AssignJsonField(SerializedProperty textProp)
        {
            Object prevObj = textProp.objectReferenceValue;
            EditorGUILayout.PropertyField(textProp);
            Object newObj = textProp.objectReferenceValue;
            
            if (newObj == null)
            {
                return false;
            }
            
            TextAsset textAsset = (TextAsset)textProp.objectReferenceValue;
            
            if (!ReferenceEquals(prevObj, newObj))
            {
                _data = null;

                if (!LDtkLoader.IsValidJson(textAsset.text))
                {
                    Debug.LogError("LDtk: Invalid LDtk format");
                    textProp.objectReferenceValue = null;
                    return false;
                }
            }
            
            if (_data == null)
            {
                _data = LDtkLoader.DeserializeJson(textAsset.text);
            }

            return true;
        }


        
        private void DrawWelcomeMessage(SerializedProperty textProp)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            string welcomeMessage = GetWelcomeMessage(textProp);
            EditorGUI.LabelField(rect, welcomeMessage);
        }
        private string GetWelcomeMessage(SerializedProperty textProp)
        {
            if (textProp.objectReferenceValue == null)
            {
                return "Assign a LDtk json text asset";
            }

            string details = "";

            if (_data == null) return $"LDtk Project{details}";
            LDtkDataProject data = _data.Value;
            details = $" v{data.jsonVersion}";

            return $"LDtk Project{details}";

        }

        #region drawing
        private void DrawLevels(LDtkDataLevel[] lvls)
        {
            foreach (LDtkDataLevel level in lvls)
            {
                new LDtkReferenceDrawerLevelIdentifier().Draw(level);
            }
        }

        private void DrawEnums(LDtkDefinitionEnum[] definitions)
        {

            
            foreach (LDtkDefinitionEnum enumDefinition in definitions)
            {
                new LDtkReferenceDrawerEnum().Draw(enumDefinition);
            }
        }

        private void DrawTilesets(LDtkDefinitionTileset[] definitions, SerializedProperty tilesetArrayProp)
        {
            for (int i = 0; i < definitions.Length; i++)
            {
                LDtkDefinitionTileset tilesetData = definitions[i];
                SerializedProperty tilesetProp = tilesetArrayProp.GetArrayElementAtIndex(i);

                //TODO revise the parameter when able to setup auto-referencing of tilesets
                new LDtkReferenceDrawerTileset(tilesetProp, "ProjectPath").Draw(tilesetData);
            }
        }
        
        private void DrawEntities(LDtkDefinitionEntity[] entities, SerializedProperty entityArrayProp)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                LDtkDefinitionEntity entityData = entities[i];
                SerializedProperty entityProp = entityArrayProp.GetArrayElementAtIndex(i);

                new LDtkReferenceDrawerEntity(entityProp).Draw(entityData);
            }
        }

        private void DrawLayers(LDtkDefinitionLayer[] layers, SerializedProperty intGridArrayProp)
        {
            int intGridValueIterator = 0;
            foreach (LDtkDefinitionLayer layer in layers)
            {
                if (!layer.IsIntGridLayer()) continue;

                new LDtkReferenceDrawerIntGridLayer().Draw(layer);
                for (int i = 0; i < layer.intGridValues.Length; i++)
                {
                    
                    LDtkDefinitionIntGridValue valueData = layer.intGridValues[i];
                    SerializedProperty valueProp = intGridArrayProp.GetArrayElementAtIndex(intGridValueIterator);
                    intGridValueIterator++;

                    new LDtkReferenceDrawerIntGridValue(valueProp, layer.displayOpacity).Draw(valueData);
                }
            }
        }
        #endregion

        
        
        /*private Rect DrawEntry(Texture2D icon, string entryName)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            
            int indent = 15;
            controlRect.xMin += indent;

            //controlRect = EditorGUI.IndentedRect(controlRect);

            Rect textureRect = new Rect(controlRect)
            {
                width = controlRect.height
            };
            GUI.DrawTexture(textureRect, icon);

            controlRect.xMin += textureRect.width;
            
            //EditorGUI.DrawRect(controlRect, Color.red);
            

            Rect labelRect = new Rect(controlRect)
            {
                width = Mathf.Max(controlRect.width/2, EditorGUIUtility.labelWidth) - EditorGUIUtility.fieldWidth
            };
            EditorGUI.LabelField(labelRect, entryName);
            
            Rect fieldRect = new Rect(controlRect)
            {
                x = labelRect.xMax,
                width = Mathf.Max(controlRect.width - labelRect.width, EditorGUIUtility.fieldWidth)
            };
            return fieldRect;
        }*/

        /*private void DrawAssignableAssetEntry<T>(Rect fieldRect) where T : Object, ILDtkAsset
        {
            //present green if okay //TODO
            //present yellow if referenced item is null
            //present red if asset does not exist
        }*/
    }
}