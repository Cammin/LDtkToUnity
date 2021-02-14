using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private LDtkProject Target => (LDtkProject) target;
        
        public override void OnInspectorGUI()
        {
            ShowGUI();

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
            
        }

        private void DrawInternalData()
        {
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


        private void ShowGUI()
        {
            SerializedProperty textProp = serializedObject.FindProperty(LDtkProject.JSON);

            if (!AssignJsonField(textProp) || _data == null)
            {
                return;
            }
            
            bool hasProblems = false;

            if (!_data.ExternalLevels)
            {
                GUIContent content = new GUIContent(
                    "Not external levels",
                    LDtkIconLoader.LoadLevelIcon(), 
                    "The option \"Save Levels To Separate Files\" is a requirement");
                EditorGUILayout.HelpBox(content);
            }

            //Grid Field
            {
                GridField();
            }
            
            //Pixels Per Unit
            int targetPixelsPerUnit = PixelsPerUnitField();
            
            
            //IntGridValuesVisibleBool
            {
                IntGridValuesVisibleField();
            }
            
            EditorGUILayout.Space();


            
            //Levels
            {
                SerializedProperty levelProp = serializedObject.FindProperty(LDtkProject.LEVEL);
                levelProp.arraySize = _data.Levels.Length;
                bool success = DrawLevels(_data.Levels, levelProp);
                if (!success)
                {
                    hasProblems = true;
                }
                
            }
            
            EditorGUILayout.Space();
            
            //IntGridValues
            {
                SerializedProperty intGridProp = serializedObject.FindProperty(LDtkProject.INTGRID);
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
                SerializedProperty entitiesProp = serializedObject.FindProperty(LDtkProject.ENTITIES);
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
                SerializedProperty tilesetArrayProp = serializedObject.FindProperty(LDtkProject.TILESETS);
                tilesetArrayProp.arraySize = _data.Defs.Tilesets.Length;
                
                if (_data.Defs.Tilesets.Length > 0)
                {
                    LinkTilesetsButton(tilesetArrayProp);
                }
                
                bool success = DrawTilesets(_data.Defs.Tilesets, tilesetArrayProp, targetPixelsPerUnit);
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
                    GenerateEnumsButton();
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
            SerializedProperty gridPrefabProp = serializedObject.FindProperty(LDtkProject.TILEMAP_PREFAB);
            Rect rect = EditorGUILayout.GetControlRect();
            float labelWidth = LDtkDrawerUtil.LabelWidth(rect.width);
            Vector2 pos = new Vector2(rect.xMin + labelWidth, rect.yMin + rect.height / 2);

            const string tooltip = "Optional. Assign a prefab here if you wish to override the default Tilemap prefab.";
            LDtkDrawerUtil.DrawInfo(pos, tooltip, TextAnchor.MiddleRight);
            
            EditorGUI.PropertyField(rect, gridPrefabProp);
            serializedObject.ApplyModifiedProperties();
        }
        private int PixelsPerUnitField() 
        {
            SerializedProperty pixelsPerUnitProp = serializedObject.FindProperty(LDtkProject.PIXELS_PER_UNIT);
            Rect rect = EditorGUILayout.GetControlRect();
            
            float labelWidth = LDtkDrawerUtil.LabelWidth(rect.width);
            Vector2 pos = new Vector2(rect.xMin + labelWidth, rect.yMin + rect.height / 2);
            
            //todo a lot of the code is reused. minimise down. mainly the drawing of the message bubble since its used more than once
            string tooltip = $"Dictates what all of the instantiated Tileset scales will adjust to, in case several LDtk layer's GridSize's are different.";
            LDtkDrawerUtil.DrawInfo(pos, tooltip, TextAnchor.MiddleRight);
            
            EditorGUI.PropertyField(rect, pixelsPerUnitProp);
            serializedObject.ApplyModifiedProperties();
            
            return pixelsPerUnitProp.intValue;
        }



        private void IntGridValuesVisibleField()
        {
            SerializedProperty intGridVisibilityProp = serializedObject.FindProperty(LDtkProject.INTGRID_VISIBLE);
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
            
            LDtkProjectFile jsonFile = (LDtkProjectFile)textProp.objectReferenceValue;
            
            if (!ReferenceEquals(prevObj, newObj))
            {
                _data = null;

                if (jsonFile.FromJson == null) //todo ensure this false loading is actually detected
                {
                    Debug.LogError("LDtk: Invalid LDtk format");
                    textProp.objectReferenceValue = null;
                    return false;
                }
            }
            
            if (_data == null)
            {
                _data = jsonFile.FromJson;
            }

            return true;
        }

        private void LinkTilesetsButton(SerializedProperty tilesetArrayProp)
        {
            if (!GUILayout.Button("Update Tileset Link"))
            {
                return;
            }

            for (int i = 0; i < _data.Defs.Tilesets.Length; i++)
            {
                TilesetDefinition tilesetDefinition = _data.Defs.Tilesets[i];
                Texture2D texture = LDtkRelPath.GetAssetRelativeToAsset<Texture2D>(Target.ProjectJson, tilesetDefinition.RelPath);

                SerializedObject assetObj = tilesetArrayProp.GetArrayElementAtIndex(i).serializedObject;

                SerializedProperty prop = assetObj.FindProperty(LDtkAsset<Object>.PROP_ASSET);

                prop.objectReferenceValue = texture;

                assetObj.ApplyModifiedProperties();
            }
            
            Debug.Log("linked tileset");

            serializedObject.ApplyModifiedProperties();
        }
        
        private void GenerateEnumsButton()
        {
            string projectName = serializedObject.targetObject.name;
            
            string targetPath = AssetDatabase.GetAssetPath(target);
            targetPath = Path.GetDirectoryName(targetPath);
            
            bool fileExists = LDtkEnumFactory.AssetExists(targetPath, projectName);
            string buttonMessage = fileExists ? "Update Enums" : "Generate Enums";

            if (GUILayout.Button(buttonMessage))
            {
                LDtkEnumGenerator.GenerateEnumScripts(_data.Defs.Enums, targetPath, serializedObject.targetObject.name);
            }
        }

        #region drawing


        private void DrawEnums(EnumDefinition[] definitions)
        {

            
            foreach (EnumDefinition enumDefinition in definitions)
            {
                new LDtkReferenceDrawerEnum().Draw(enumDefinition);
            }
        }
        
        private bool DrawLevels(Level[] lvls, SerializedProperty levelArrayProp)
        {
            bool passed = true;
            for (int i = 0; i < lvls.Length; i++)
            {
                Level level = lvls[i];
                SerializedObject levelObj = levelArrayProp.GetArrayElementAtIndex(i).serializedObject;

                LDtkReferenceDrawerLevel drawer = new LDtkReferenceDrawerLevel(levelObj, level.Identifier);
                drawer.Draw(level);
                if (drawer.HasProblem)
                {
                    passed = false;
                }
            }

            return passed;
        }

        private bool DrawTilesets(TilesetDefinition[] definitions, SerializedProperty tilesetArrayProp, int targetPixelsPerUnit)
        {
            bool passed = true;
            for (int i = 0; i < definitions.Length; i++)
            {
                TilesetDefinition definition = definitions[i];
                
                SerializedObject tilesetObj = tilesetArrayProp.GetArrayElementAtIndex(i).serializedObject;

                LDtkReferenceDrawerTileset drawer = new LDtkReferenceDrawerTileset(tilesetObj, definition.Identifier, targetPixelsPerUnit);
                drawer.Draw(definition);
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
                SerializedObject entityObj = entityArrayProp.GetArrayElementAtIndex(i).serializedObject;

                LDtkReferenceDrawerEntity drawer = new LDtkReferenceDrawerEntity(entityObj, entityData.Identifier);
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
                if (!layer.IsIntGridLayer) continue;

                new LDtkReferenceDrawerIntGridLayer().Draw(layer);
                
                foreach (IntGridValueDefinition definition in layer.IntGridValues)
                {
                    SerializedObject valueObj = intGridArrayProp.GetArrayElementAtIndex(intGridValueIterator).serializedObject;
                    intGridValueIterator++;

                    LDtkReferenceDrawerIntGridValue drawer = new LDtkReferenceDrawerIntGridValue(valueObj, definition.Identifier, (float)layer.DisplayOpacity);
                    drawer.Draw(definition);
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