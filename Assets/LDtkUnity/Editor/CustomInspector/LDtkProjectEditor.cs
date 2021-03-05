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
        
        private bool _levelDropdown;
        private bool _intGridDropdown;
        private bool _entityDropdown;
        private bool _enumDropdown;
        private bool _tilesetDropdown;
        private bool _internalDataDropdown;

        private bool _hasProblems = false;

        private LDtkProject Target => (LDtkProject) target;

        private void OnEnable()
        {
            _levelDropdown = EditorPrefs.GetBool(nameof(_levelDropdown), true);
            _intGridDropdown = EditorPrefs.GetBool(nameof(_intGridDropdown), true);
            _entityDropdown = EditorPrefs.GetBool(nameof(_entityDropdown), true);
            _enumDropdown = EditorPrefs.GetBool(nameof(_enumDropdown), true);
            _tilesetDropdown = EditorPrefs.GetBool(nameof(_tilesetDropdown), true);
            _internalDataDropdown = EditorPrefs.GetBool(nameof(_internalDataDropdown), true);
        }

        private void OnDisable()
        {
            EditorPrefs.SetBool(nameof(_levelDropdown), _levelDropdown);
            EditorPrefs.SetBool(nameof(_intGridDropdown), _intGridDropdown);
            EditorPrefs.SetBool(nameof(_entityDropdown), _entityDropdown);
            EditorPrefs.SetBool(nameof(_enumDropdown), _enumDropdown);
            EditorPrefs.SetBool(nameof(_tilesetDropdown), _tilesetDropdown);
            EditorPrefs.SetBool(nameof(_internalDataDropdown), _internalDataDropdown);
        }
        
        

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            ShowGUI();

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
            
            DrawInternalData();
            
        }

        private void DrawInternalData()
        {
            _internalDataDropdown = EditorGUILayout.Foldout(_internalDataDropdown, "Internal Data");
            if (!_internalDataDropdown)
            {
                return;
            }
            
            EditorGUI.indentLevel++;
            GUI.enabled = false;
            base.OnInspectorGUI();
            GUI.enabled = true;
            EditorGUI.indentLevel--;
        }


        private void ShowGUI()
        {
            SerializedProperty textProp = serializedObject.FindProperty(LDtkProject.JSON);

            if (!AssignJsonField(textProp) || _data == null)
            {
                return;
            }
            

            if (!DrawIsExternalLevels())
            {
                return;
            }
            
            GridField();
            PixelsPerUnitField();
            IntGridValuesVisibleField();
            
            _hasProblems = false;
            
            LevelsSection();
            IntGridValuesSection();
            EntitiesSection();
            EnumsSection();
            TilesetsSection();

            DrawDivider();

            if (_hasProblems)
            {
                EditorGUILayout.HelpBox("LDtk Project asset configuration has unresolved issues, mouse over them to see the problem", MessageType.Warning);
            }
        }

        private void TilesetsSection()
        {
            SerializedProperty tilesetsProp = serializedObject.FindProperty(LDtkProject.TILESETS);

            int arraySize = _data.Defs.Tilesets.Length;
            if (arraySize > 0)
            {
                DrawNewSection("Tilesets",
                    "Auto-assign the tilesets, and then hit the circle button to automatically generate spiced sprites, and also create tile assets out of them.",
                    LDtkIconLoader.LoadTilesetIcon(), ref _tilesetDropdown);
            }

            if (_tilesetDropdown)
            {
                tilesetsProp.arraySize = arraySize;

                bool success = DrawTilesets(_data.Defs.Tilesets, tilesetsProp);
                if (!success)
                {
                    _hasProblems = true;
                }

                LinkTilesetsButton(tilesetsProp);
            }
        }

        private void EnumsSection()
        {
            if (_data.Defs.Enums.Length > 0)
            {
                DrawNewSection("Enums", "The enums would be automatically generated as scripts.", LDtkIconLoader.LoadEnumIcon(),
                    ref _enumDropdown);
            }

            if (_enumDropdown)
            {
                DrawEnums(_data.Defs.Enums);
                

                if (_data.Defs.Enums.Length > 0)
                {
                    
                    GenerateEnumsButton();
                }
            }
        }

        private void EntitiesSection()
        {
            SerializedProperty entitiesProp = serializedObject.FindProperty(LDtkProject.ENTITIES);

            int arraySize = _data.Defs.Entities.Length;
            if (arraySize > 0)
            {
                DrawNewSection("Entities", "Assign GameObjects that would be spawned as entities",
                    LDtkIconLoader.LoadEntityIcon(), ref _entityDropdown);
            }

            if (_entityDropdown)
            {
                entitiesProp.arraySize = arraySize;

                bool success = DrawEntities(_data.Defs.Entities, entitiesProp);
                if (!success)
                {
                    _hasProblems = true;
                }
            }
        }

        private void IntGridValuesSection()
        {
            SerializedProperty intGridValuesProp = serializedObject.FindProperty(LDtkProject.INTGRID);

            int arraySize = _data.Defs.Layers.SelectMany(p => p.IntGridValues).Distinct().Count();
            if (arraySize > 0)
            {
                string tooltip = "The sprites assigned to IntGrid Values determine the collision shape of them in the tilemap.";
                DrawNewSection("IntGrids", tooltip, LDtkIconLoader.LoadIntGridIcon(), ref _intGridDropdown);
            }

            if (_intGridDropdown)
            {
                intGridValuesProp.arraySize = arraySize;

                if (arraySize > 0)
                {
                    LayerDefinition[] intGridLayerDefs = _data.Defs.Layers.Where(p => p.IsIntGridLayer).ToArray();
                    bool success = DrawIntGridLayers(intGridLayerDefs, intGridValuesProp);
                    if (!success)
                    {
                        _hasProblems = true;
                    }
                }
            }
        }

        private void LevelsSection()
        {
            SerializedProperty levelsProp = serializedObject.FindProperty(LDtkProject.LEVEL);

            int arraySize = _data.Levels.Length;
            if (arraySize > 0)
            {
                DrawNewSection("Levels", "The levels. Hit the button at the bottom to automatically assign them.",
                    LDtkIconLoader.LoadWorldIcon(), ref _levelDropdown);
            }

            if (_levelDropdown)
            {
                levelsProp.arraySize = arraySize;

                bool success = DrawLevels(_data.Levels, levelsProp);
                if (!success)
                {
                    _hasProblems = true;
                }

                LinkLevelsButton(levelsProp);
            }
        }

        private void DrawNewSection(string label, string tooltip, Texture tex, ref bool dropDown)
        {
            DrawDivider();
            GUIContent content = new GUIContent()
            {
                text = label,
                tooltip = tooltip,
                image = tex
            };
            
            dropDown = EditorGUILayout.Foldout(dropDown, content);
        }

        private static void DrawDivider()
        {
            int space = 2;
            
            EditorGUILayout.Space(space);
            
            float height = 2f;
            Rect area = GUILayoutUtility.GetRect(0, height);
            area.xMin -= 15;

            const float colorIntensity = 0.1f;
            Color areaColor = new Color(colorIntensity, colorIntensity, colorIntensity, 1);
            EditorGUI.DrawRect(area, areaColor);
            
            EditorGUILayout.Space(space);
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

        private string EnumsNamespaceField()
        {
            SerializedProperty namespaceProp = serializedObject.FindProperty(LDtkProject.ENUM_NAMESPACE);
            EditorGUILayout.PropertyField(namespaceProp);
            
            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }

            return namespaceProp.stringValue;
        }
        
        private void GenerateEnumsButton()
        {
            string projectName = serializedObject.targetObject.name;
            
            string targetPath = AssetDatabase.GetAssetPath(target);
            targetPath = Path.GetDirectoryName(targetPath);
            
            string nameSpace = EnumsNamespaceField();
                
            bool fileExists = LDtkEnumFactory.AssetExists(targetPath, projectName);
            string buttonMessage = fileExists ? "Update Enums" : "Generate Enums";

            if (GUILayout.Button(buttonMessage))
            {
                LDtkEnumGenerator.GenerateEnumScripts(_data.Defs.Enums, targetPath, serializedObject.targetObject.name, nameSpace);
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
                
                if (drawer.HasError(level))
                {
                    passed = false;
                }
                
                drawer.Draw(level);
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
                
                if (drawer.HasError(definition))
                {
                    passed = false;
                }
                
                drawer.Draw(definition);
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
                
                if (drawer.HasError(entityData))
                {
                    passed = false;
                }
                
                drawer.Draw(entityData);

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
                    
                    if (drawer.HasError(intGridValueDef))
                    {
                        passed = false;
                    }
                    
                    drawer.Draw(intGridValueDef);
                    

                }
            }

            return passed;
        }
        #endregion
    }
}