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

            //Grid Field
            {
                SerializedProperty gridPrefabProp = serializedObj.FindProperty(LDtkProject.PROP_TILEMAP_PREFAB);
                Rect rect = EditorGUILayout.GetControlRect();
                float labelWidth = LDtkDrawerUtil.LabelWidth(rect.width);
                Vector2 pos = new Vector2(rect.xMin + labelWidth, rect.yMin + rect.height/2);
                
                const string tooltip = "Optional. Assign a prefab here if you wish to override the default Tilemap prefab.";
                LDtkDrawerUtil.DrawInfo(pos, tooltip, TextAnchor.MiddleRight);
                EditorGUI.PropertyField(rect, gridPrefabProp);
            }
            
            EditorGUILayout.Space();
            
            
            LDtkDataProject projectData = _data.Value;
            //Levels
            {
                DrawLevels(projectData.levels);
            }
            
            EditorGUILayout.Space();

            //IntGridValuesVisibleBool
            {
                SerializedProperty intGridVisibilityProp = serializedObj.FindProperty(LDtkProject.PROP_INTGRIDVISIBLE);
                EditorGUILayout.PropertyField(intGridVisibilityProp);
            }

            bool hasProblems = false;
            
            //IntGridValues
            {
                SerializedProperty intGridProp = serializedObj.FindProperty(LDtkProject.PROP_INTGRID);
                intGridProp.arraySize = projectData.defs.layers.SelectMany(p => p.intGridValues).Distinct().Count() - 1;
                bool success = DrawLayers(projectData.defs.layers, intGridProp);
                if (!success)
                {
                    hasProblems = true;
                }
            }
            

            EditorGUILayout.Space();

            //Entites
            {
                SerializedProperty entitiesProp = serializedObj.FindProperty(LDtkProject.PROP_ENTITIES);
                entitiesProp.arraySize = projectData.defs.entities.Length;
                bool success = DrawEntities(projectData.defs.entities, entitiesProp);
                if (!success)
                {
                    hasProblems = true;
                }
            }

            EditorGUILayout.Space();

            //Tilesets
            {
                SerializedProperty tilesetsProp = serializedObj.FindProperty(LDtkProject.PROP_TILESETS);
                tilesetsProp.arraySize = projectData.defs.tilesets.Length;
                bool success = DrawTilesets(projectData.defs.tilesets, tilesetsProp);
                if (!success)
                {
                    hasProblems = true;
                }
            }
            
            EditorGUILayout.Space();
            
            //Enums
            {
                if (projectData.defs.enums.Length > 0)
                {
                    GenerateEnumsButton(projectData, serializedObj);
                }

                DrawEnums(projectData.defs.enums);
            }
            
            if (hasProblems)
            {
                EditorGUILayout.HelpBox("LDtk Project asset configuration has unresolved issues, mouse over them to see the problem", MessageType.Warning);
            }
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

        private bool DrawTilesets(LDtkDefinitionTileset[] definitions, SerializedProperty tilesetArrayProp)
        {
            bool passed = true;
            for (int i = 0; i < definitions.Length; i++)
            {
                LDtkDefinitionTileset tilesetData = definitions[i];
                SerializedProperty tilesetProp = tilesetArrayProp.GetArrayElementAtIndex(i);

                //TODO revise the parameter when able to setup auto-referencing of tilesets
                LDtkReferenceDrawerTileset drawer = new LDtkReferenceDrawerTileset(tilesetProp, "ProjectPath");
                drawer.Draw(tilesetData);
                if (drawer.HasProblem)
                {
                    passed = false;
                }
            }
            return passed;
        }
        
        private bool DrawEntities(LDtkDefinitionEntity[] entities, SerializedProperty entityArrayProp)
        {
            bool passed = true;
            for (int i = 0; i < entities.Length; i++)
            {
                LDtkDefinitionEntity entityData = entities[i];
                SerializedProperty entityProp = entityArrayProp.GetArrayElementAtIndex(i);

                LDtkReferenceDrawerEntity drawer = new LDtkReferenceDrawerEntity(entityProp);
                drawer.Draw(entityData);
                if (drawer.HasProblem)
                {
                    passed = false;
                }
            }

            return passed;
        }

        private bool DrawLayers(LDtkDefinitionLayer[] layers, SerializedProperty intGridArrayProp)
        {
            int intGridValueIterator = 0;
            bool passed = true;
            foreach (LDtkDefinitionLayer layer in layers)
            {
                if (!layer.IsIntGridLayer()) continue;

                new LDtkReferenceDrawerIntGridLayer().Draw(layer);
                
                for (int i = 0; i < layer.intGridValues.Length; i++)
                {
                    
                    LDtkDefinitionIntGridValue valueData = layer.intGridValues[i];
                    SerializedProperty valueProp = intGridArrayProp.GetArrayElementAtIndex(intGridValueIterator);
                    intGridValueIterator++;

                    LDtkReferenceDrawerIntGridValue drawer = new LDtkReferenceDrawerIntGridValue(valueProp, layer.displayOpacity);
                    drawer.Draw(valueData);
                    if (drawer.HasProblem)
                    {
                        passed = false;
                    }
                    
                }
            }

            return passed;
        }
        #endregion
    }
}