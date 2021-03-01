using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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
            
            DrawInternalData();
            
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

            if (!DrawIsExternalLevels())
            {
                return;
            }

            //Grid Field
            {
                GridField();
            }
            
            //Pixels Per Unit
            PixelsPerUnitField();
            
            //IntGridValuesVisibleBool
            {
                IntGridValuesVisibleField();
            }
            
            //Levels
            {
                SerializedProperty levelsProp = serializedObject.FindProperty(LDtkProject.LEVEL);

                int arraySize = _data.Levels.Length;
                if (arraySize > 0)
                {
                    EditorGUILayout.Space();
                }
                levelsProp.arraySize = arraySize;
                
                bool success = DrawLevels(_data.Levels, levelsProp);
                if (!success)
                {
                    hasProblems = true;
                }
                
                LinkLevelsButton(levelsProp);
            }
            
            //IntGridValues
            {
                SerializedProperty intGridValuesProp = serializedObject.FindProperty(LDtkProject.INTGRID);
                
                int arraySize = _data.Defs.Layers.SelectMany(p => p.IntGridValues).Distinct().Count();
                if (arraySize > 0)
                {
                    EditorGUILayout.Space();
                }
                intGridValuesProp.arraySize = arraySize;

                if (arraySize > 0)
                {
                    LayerDefinition[] intGridLayerDefs = _data.Defs.Layers.Where(p => p.IsIntGridLayer).ToArray();
                    bool success = DrawIntGridLayers(intGridLayerDefs, intGridValuesProp);
                    if (!success)
                    {
                        hasProblems = true;
                    }
                }
            }
            
            //Entities
            {
                SerializedProperty entitiesProp = serializedObject.FindProperty(LDtkProject.ENTITIES);

                int arraySize = _data.Defs.Entities.Length;
                if (arraySize > 0)
                {
                    EditorGUILayout.Space();
                }
                entitiesProp.arraySize = arraySize;
                
                bool success = DrawEntities(_data.Defs.Entities, entitiesProp);
                if (!success)
                {
                    hasProblems = true;
                }
            }
            
            //Enums
            {
                if (_data.Defs.Enums.Length > 0)
                {
                    EditorGUILayout.Space();
                }
                
                DrawEnums(_data.Defs.Enums);
                
                if (_data.Defs.Enums.Length > 0)
                {
                    GenerateEnumsButton();
                }
            }
            
            
            //Tilesets
            {
                SerializedProperty tilesetsProp = serializedObject.FindProperty(LDtkProject.TILESETS);
                
                int arraySize = _data.Defs.Tilesets.Length;
                if (arraySize > 0)
                {
                    EditorGUILayout.Space();
                }
                tilesetsProp.arraySize = arraySize;
                
                bool success = DrawTilesets(_data.Defs.Tilesets, tilesetsProp);
                if (!success)
                {
                    hasProblems = true;
                }
                LinkTilesetsButton(tilesetsProp);
            }



            if (hasProblems)
            {
                EditorGUILayout.HelpBox("LDtk Project asset configuration has unresolved issues, mouse over them to see the problem", MessageType.Warning);
            }
        }

        private bool DrawIsExternalLevels()
        {
            if (_data.ExternalLevels)
            {
                return true;
            }
            
            GUIContent content = new GUIContent(
                "Not external levels",
                LDtkIconLoader.LoadLevelIcon(),
                "The option \"Save Levels To Separate Files\" is a requirement");
            EditorGUILayout.HelpBox(content);

            return false;
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
            if (tilesetArrayProp.arraySize <= 0)
            {
                return;
            }
            
            if (!GUILayout.Button("Auto-Assign Tilesets"))
            {
                return;
            }
            
            for (int i = 0; i < _data.Defs.Tilesets.Length; i++)
            {
                TilesetDefinition tilesetDefinition = _data.Defs.Tilesets[i];
                Texture2D texture = LDtkRelPath.GetAssetRelativeToAsset<Texture2D>(Target.ProjectJson, tilesetDefinition.RelPath);

                SerializedProperty assetProp = tilesetArrayProp.GetArrayElementAtIndex(i);

                SerializedProperty prop = assetProp.FindPropertyRelative(LDtkAsset.PROP_ASSET);

                prop.objectReferenceValue = texture;

                assetProp.serializedObject.ApplyModifiedProperties();
            }
            
            Debug.Log("Linked tilesets");

            serializedObject.ApplyModifiedProperties();
        }
        
        //is a copypaste of LinkTilesetsButton. reduce mess when acceptable
        private void LinkLevelsButton(SerializedProperty levelArrayProp)
        {
            if (levelArrayProp.arraySize <= 0)
            {
                return;
            }
            
            if (!GUILayout.Button("Auto-Assign Levels"))
            {
                return;
            }
            
            for (int i = 0; i < _data.Levels.Length; i++)
            {
                Level levelDefinition = _data.Levels[i];
                LDtkLevelFile texture = LDtkRelPath.GetAssetRelativeToAsset<LDtkLevelFile>(Target.ProjectJson, levelDefinition.ExternalRelPath);

                SerializedProperty assetProp = levelArrayProp.GetArrayElementAtIndex(i);

                SerializedProperty prop = assetProp.FindPropertyRelative(LDtkAsset.PROP_ASSET);

                prop.objectReferenceValue = texture;

                assetProp.serializedObject.ApplyModifiedProperties();
            }
            
            Debug.Log("Linked levels");

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
                SerializedProperty levelObj = levelArrayProp.GetArrayElementAtIndex(i);
                
                LDtkReferenceDrawerLevel drawer = new LDtkReferenceDrawerLevel(levelObj, level.Identifier);
                drawer.Draw(level);
                if (drawer.HasProblem)
                {
                    passed = false;
                }
            }

            return passed;
        }

        private bool DrawTilesets(TilesetDefinition[] definitions, SerializedProperty tilesetArrayProp)
        {
            bool passed = true;
            for (int i = 0; i < definitions.Length; i++)
            {
                TilesetDefinition definition = definitions[i];
                
                SerializedProperty tilesetObj = tilesetArrayProp.GetArrayElementAtIndex(i);

                LDtkReferenceDrawerTileset drawer = new LDtkReferenceDrawerTileset(tilesetObj, definition.Identifier);
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
                SerializedProperty entityObj = entityArrayProp.GetArrayElementAtIndex(i);

                LDtkReferenceDrawerEntity drawer = new LDtkReferenceDrawerEntity(entityObj, entityData.Identifier);
                drawer.Draw(entityData);
                if (drawer.HasProblem)
                {
                    passed = false;
                }
            }

            return passed;
        }

        private bool DrawIntGridLayers(LayerDefinition[] intGridLayerDefs, SerializedProperty intGridArrayProp)
        {
            int intGridValueIterator = 0;
            bool passed = true;
            foreach (LayerDefinition intGridLayerDef in intGridLayerDefs)
            {
                new LDtkReferenceDrawerIntGridLayer().Draw(intGridLayerDef);

                foreach (IntGridValueDefinition intGridValueDef in intGridLayerDef.IntGridValues)
                {
                    SerializedProperty valueObj = intGridArrayProp.GetArrayElementAtIndex(intGridValueIterator);
                    intGridValueIterator++;


                    string key = LDtkIntGridKeyFormat.GetKeyFormat(intGridLayerDef, intGridValueDef);
                    
                    LDtkReferenceDrawerIntGridValue drawer =
                        new LDtkReferenceDrawerIntGridValue(valueObj, key,
                            (float) intGridLayerDef.DisplayOpacity);
                    drawer.Draw(intGridValueDef);
                    
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