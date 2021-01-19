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
        private LdtkJson _data;
        
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
                GridField();
            }
            
            //Pixels Per Unit
            {
                PixelsPerUnitField();
            }
            
            //IntGridValuesVisibleBool
            {
                IntGridValuesVisibleField();
            }
            
            EditorGUILayout.Space();

            //Levels
            {
                DrawLevels(_data.Levels);
            }
            
            EditorGUILayout.Space();



            bool hasProblems = false;
            
            //IntGridValues
            {
                SerializedProperty intGridProp = serializedObj.FindProperty(LDtkProject.PROP_INTGRID);
                intGridProp.arraySize = _data.Defs.Layers.SelectMany(p => p.IntGridValues).Distinct().Count() - 1;
                bool success = DrawLayers(_data.Defs.Layers, intGridProp);
                if (!success)
                {
                    hasProblems = true;
                }
            }
            

            EditorGUILayout.Space();

            //Entites
            {
                SerializedProperty entitiesProp = serializedObj.FindProperty(LDtkProject.PROP_ENTITIES);
                entitiesProp.arraySize = _data.Defs.Entities.Length;
                bool success = DrawEntities(_data.Defs.Entities, entitiesProp);
                if (!success)
                {
                    hasProblems = true;
                }
            }

            EditorGUILayout.Space();

            //Tilesets
            {
                SerializedProperty tilesetsProp = serializedObj.FindProperty(LDtkProject.PROP_TILESETS);
                tilesetsProp.arraySize = _data.Defs.Tilesets.Length;
                bool success = DrawTilesets(_data.Defs.Tilesets, tilesetsProp);
                if (!success)
                {
                    hasProblems = true;
                }
            }
            
            EditorGUILayout.Space();
            
            //Enums
            {
                if (_data.Defs.Enums.Length > 0)
                {
                    GenerateEnumsButton(_data, serializedObj);
                }

                DrawEnums(_data.Defs.Enums);
            }
            
            if (hasProblems)
            {
                EditorGUILayout.HelpBox("LDtk Project asset configuration has unresolved issues, mouse over them to see the problem", MessageType.Warning);
            }
            
            
        }

        private void GridField()
        {
            SerializedProperty gridPrefabProp = serializedObject.FindProperty(LDtkProject.PROP_TILEMAP_PREFAB);
            Rect rect = EditorGUILayout.GetControlRect();
            float labelWidth = LDtkDrawerUtil.LabelWidth(rect.width);
            Vector2 pos = new Vector2(rect.xMin + labelWidth, rect.yMin + rect.height / 2);

            const string tooltip = "Optional. Assign a prefab here if you wish to override the default Tilemap prefab.";
            LDtkDrawerUtil.DrawInfo(pos, tooltip, TextAnchor.MiddleRight);
            
            EditorGUI.PropertyField(rect, gridPrefabProp);
            serializedObject.ApplyModifiedProperties();
        }
        private void PixelsPerUnitField() 
        {
            SerializedProperty pixelsPerUnitProp = serializedObject.FindProperty(LDtkProject.PROP_PIXELS_PER_UNIT);
            Rect rect = EditorGUILayout.GetControlRect();
            
            float labelWidth = LDtkDrawerUtil.LabelWidth(rect.width);
            Vector2 pos = new Vector2(rect.xMin + labelWidth, rect.yMin + rect.height / 2);
            
            //todo a lot of the code is reused. minimise down. mainly the drawing of the message bubble since its used more than once
            string tooltip = $"Dictates what all of the instantiated Tileset scales will adjust to, in case several LDtk layer's GridSize's are different.";
            LDtkDrawerUtil.DrawInfo(pos, tooltip, TextAnchor.MiddleRight);
            
            EditorGUI.PropertyField(rect, pixelsPerUnitProp);
            serializedObject.ApplyModifiedProperties();
        }



        private void IntGridValuesVisibleField()
        {
            SerializedProperty intGridVisibilityProp = serializedObject.FindProperty(LDtkProject.PROP_INTGRIDVISIBLE);
            Rect rect = EditorGUILayout.GetControlRect();
            
            EditorGUI.PropertyField(rect, intGridVisibilityProp);
            serializedObject.ApplyModifiedProperties();
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

                if (LdtkJson.FromJson(textAsset.text) == null) //todo ensure this false loading is actually detected
                {
                    Debug.LogError("LDtk: Invalid LDtk format");
                    textProp.objectReferenceValue = null;
                    return false;
                }
            }
            
            if (_data == null)
            {
                _data = LdtkJson.FromJson(textAsset.text);
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
            LdtkJson data = _data;
            details = $" v{data.JsonVersion}";

            return $"LDtk Project{details}";

        }
        
        private void GenerateEnumsButton(LdtkJson projectData, SerializedObject serializedObj)
        {
            string projectName = serializedObj.targetObject.name;
            
            string targetPath = AssetDatabase.GetAssetPath(target);
            targetPath = Path.GetDirectoryName(targetPath);
            
            bool fileExists = LDtkEnumFactory.AssetExists(targetPath, projectName);
            string buttonMessage = fileExists ? "Update Enums" : "Generate Enums";

            if (GUILayout.Button(buttonMessage))
            {
                LDtkEnumGenerator.GenerateEnumScripts(projectData.Defs.Enums, targetPath, serializedObj.targetObject.name);
            }
        }

        #region drawing
        private void DrawLevels(Level[] lvls)
        {
            foreach (Level level in lvls)
            {
                new LDtkReferenceDrawerLevelIdentifier().Draw(level);
            }
        }

        private void DrawEnums(EnumDefinition[] definitions)
        {

            
            foreach (EnumDefinition enumDefinition in definitions)
            {
                new LDtkReferenceDrawerEnum().Draw(enumDefinition);
            }
        }

        private bool DrawTilesets(TilesetDefinition[] definitions, SerializedProperty tilesetArrayProp)
        {
            bool passed = true;
            for (int i = 0; i < definitions.Length; i++)
            {
                TilesetDefinition tilesetData = definitions[i];
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
        
        private bool DrawEntities(EntityDefinition[] entities, SerializedProperty entityArrayProp)
        {
            bool passed = true;
            for (int i = 0; i < entities.Length; i++)
            {
                EntityDefinition entityData = entities[i];
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

        private bool DrawLayers(LayerDefinition[] layers, SerializedProperty intGridArrayProp)
        {
            int intGridValueIterator = 0;
            bool passed = true;
            foreach (LayerDefinition layer in layers)
            {
                if (!layer.IsIntGridLayer()) continue;

                new LDtkReferenceDrawerIntGridLayer().Draw(layer);
                
                for (int i = 0; i < layer.IntGridValues.Length; i++)
                {
                    
                    dynamic valueData = layer.IntGridValues[i];
                    SerializedProperty valueProp = intGridArrayProp.GetArrayElementAtIndex(intGridValueIterator);
                    intGridValueIterator++;

                    LDtkReferenceDrawerIntGridValue drawer = new LDtkReferenceDrawerIntGridValue(valueProp, (float)layer.DisplayOpacity);
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