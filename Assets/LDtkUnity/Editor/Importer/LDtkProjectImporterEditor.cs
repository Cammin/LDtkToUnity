using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkProjectImporter))]
    public class LDtkProjectImporterEditor : LDtkJsonImporterEditor
    {
        private LdtkJson _data;

        
        
        private ILDtkProjectSectionDrawer[] _sectionDrawers;
        
        private ILDtkProjectSectionDrawer _sectionLevels;
        //private ILDtkProjectSectionDrawer _sectionLevelBackgrounds;
        private ILDtkProjectSectionDrawer _sectionIntGrids;
        private ILDtkProjectSectionDrawer _sectionEntities;
        private ILDtkProjectSectionDrawer _sectionEnums;
        //private ILDtkProjectSectionDrawer _sectionTilesets;
        //private ILDtkProjectSectionDrawer _sectionTileAssets;
        private ILDtkProjectSectionDrawer _sectionGridPrefabs;

        private bool _levelFieldsError;
        private bool _isFirstUpdate = true;


        public override void OnEnable()
        {
            base.OnEnable();
            
            
            
            
            _sectionLevels = new LDtkProjectSectionLevels(serializedObject);
            //_sectionLevelBackgrounds = new LDtkProjectSectionLevelBackgrounds(serializedObject);
            _sectionIntGrids = new LDtkProjectSectionIntGrids(serializedObject);
            _sectionEntities = new LDtkProjectSectionEntities(serializedObject);
            _sectionEnums = new LDtkProjectSectionEnums(serializedObject);
            //_sectionTilesets = new LDtkProjectSectionTilesets(serializedObject);
            //_sectionTileAssets = new LDtkProjectSectionTileCollections(serializedObject);
            _sectionGridPrefabs = new LDtkProjectSectionGridPrefabs(serializedObject);
            
            _sectionDrawers = new[]
            {
                _sectionLevels,
                //_sectionLevelBackgrounds,
                _sectionIntGrids,
                _sectionEntities,
                _sectionEnums,
                //_sectionTilesets,
                //_sectionTileAssets,
                _sectionGridPrefabs
            };

            foreach (ILDtkProjectSectionDrawer drawer in _sectionDrawers)
            {
                drawer.Init();
            }
            
            
            
        }

        public override void OnDisable()
        {
            foreach (ILDtkProjectSectionDrawer drawer in _sectionDrawers)
            {
                drawer.Dispose();
            }
            base.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            try
            {
                //at the start of all drawing, set icon size for some GuiContents
                EditorGUIUtility.SetIconSize(Vector2.one * 16);
            
                serializedObject.Update();
                ShowGUI();
                serializedObject.ApplyModifiedProperties();
            
                EditorGUIUtility.SetIconSize(Vector2.one * 32);

                if (_isFirstUpdate)
                {
                    ApplyIfArraySizesChanged();
                    _isFirstUpdate = false;
                }
                DrawPotentialProblem();
            }
            finally
            {
                ApplyRevertGUI();
            }
        }

        private void ShowGUI()
        {
            

            if (!AssignJsonField() || _data == null)
            {
                return;
            }
            

            /*if (!DrawIsExternalLevels())
            {
                return;
            }*/
            
            Definitions defs = _data.Defs;
            
            PixelsPerUnitField();
            DeparentInRuntimeField();
            LogBuildTimesField();
            _levelFieldsError = LevelFieldsPrefabField(defs.LevelFields);

            
            _sectionLevels.Draw(_data.Levels);
            //_sectionLevelBackgrounds.Draw(_data.Levels.Where(level => !string.IsNullOrEmpty(level.BgRelPath)));
            _sectionIntGrids.Draw(defs.IntGridLayers);
            _sectionEntities.Draw(defs.Entities);
            _sectionEnums.Draw(defs.Enums);
            //_sectionTilesets.Draw(defs.Tilesets);
            //_sectionTileAssets.Draw(defs.Tilesets);
            _sectionGridPrefabs.Draw(defs.UnityGridLayers);
            
            LDtkDrawerUtil.DrawDivider();
        }

        private bool AssignJsonField()
        {
            SerializedProperty jsonProp = serializedObject.FindProperty(LDtkProjectImporter.JSON);
            
            if (_data != null)
            {
                return true;
            }
            
            Object jsonAsset = jsonProp.objectReferenceValue;
            if (jsonAsset == null)
            {
                Debug.LogError("Json asset is null, it's never expected to happen");
                return false;
            }
            
            LDtkProjectFile jsonFile = (LDtkProjectFile)jsonAsset;
            LdtkJson json = jsonFile.FromJson;
            if (json != null)
            {
                _data = jsonFile.FromJson;
                return true;
            }
            
            _data = null;
            Debug.LogError("LDtk: Invalid LDtk format");
            jsonProp.objectReferenceValue = null;
            return false;
        }

        
        
        /*private bool DrawIsExternalLevels()
        {
            if (_data.ExternalLevels)
            {
                return true;
            }
            
            GUIContent content = new GUIContent(
                "Not external levels",
                LDtkIconLoader.LoadLevelFileIcon(),
                "The option \"Save Levels To Separate Files\" is a requirement");
            EditorGUILayout.HelpBox(content);

            return false;
        }*/
        
        /// <summary>
        /// Returns if this method had a problem.
        /// </summary>
        private bool LevelFieldsPrefabField(FieldDefinition[] defsEntityLayers)
        {
            if (defsEntityLayers == null || defsEntityLayers.Length == 0)
            {
                return false;
            }
            
            SerializedProperty levelFieldsProp = serializedObject.FindProperty(LDtkProjectImporter.LEVEL_FIELDS_PREFAB);
            
            GUIContent content = new GUIContent()
            {
                text = levelFieldsProp.displayName,
                tooltip = "Optional.\n" +
                          "Similar to the Entity prefab components, Optionally assign a Prefab which has [LDtkField] attributes on a component's fields to inject the LDtk level values."
                
            };
            
            Rect controlRect = EditorGUILayout.GetControlRect();
            
            if (levelFieldsProp.objectReferenceValue == null)
            {
                LDtkDrawerUtil.DrawFieldWarning(controlRect, "The LDtk project has level fields defined, but there is no scripted level prefab assigned here.");
            }
            
            EditorGUI.PropertyField(controlRect, levelFieldsProp, content);
            return levelFieldsProp.objectReferenceValue == null;
        }

        
        private void PixelsPerUnitField() 
        {
            SerializedProperty pixelsPerUnitProp = serializedObject.FindProperty(LDtkProjectImporter.PIXELS_PER_UNIT);
            
            GUIContent content = new GUIContent()
            {
                text = pixelsPerUnitProp.displayName,
                tooltip = "Dictates what all of the instantiated Tileset scales will adjust to, in case several LDtk layer's GridSize's are different."
            };
            
            EditorGUILayout.PropertyField(pixelsPerUnitProp, content);
        }

        

        private void DeparentInRuntimeField()
        {
            SerializedProperty deparentProp = serializedObject.FindProperty(LDtkProjectImporter.DEPARENT_IN_RUNTIME);
            
            GUIContent content = new GUIContent()
            {
                text = deparentProp.displayName,
                tooltip = "When on, adds components to the project, levels, and entity-layer GameObjects that act to de-parent all of their children in runtime.\n" +
                          "This results in increased runtime performance.\n" +
                          "Keep this on if the exact level/layer hierarchy structure is not a concern in runtime."
                
            };
            
            EditorGUILayout.PropertyField(deparentProp, content);
        }
        
        private void LogBuildTimesField()
        {
            SerializedProperty pixelsPerUnitProp = serializedObject.FindProperty(LDtkProjectImporter.LOG_BUILD_TIMES);
            
            GUIContent content = new GUIContent()
            {
                text = pixelsPerUnitProp.displayName,
                tooltip = "Dictates what all of the instantiated Tileset scales will adjust to, in case several LDtk layer's GridSize's are different."
            };
            
            EditorGUILayout.PropertyField(pixelsPerUnitProp, content);
        }

        private void ApplyIfArraySizesChanged()
        {
            //IMPORTANT: if there are any new/removed array elements via this setup of automatically setting array size as LDtk definitions change,
            //then Unity's going to notice and make the apply/revert buttons appear active which gives us trouble when we try clicking out.
            //So, try applying right now when this specific case happens. Typically during the first OnInspectorGUI.
            
            if (_sectionDrawers.Any(drawer => drawer.HasResizedArrayPropThisUpdate))
            {
                Apply();
                Debug.Log("APPLIED");
            }
        }
        
        private void DrawPotentialProblem()
        {
            bool problem = _levelFieldsError || _sectionDrawers.Any(drawer => drawer.HasProblem);

            if (problem)
            {
                EditorGUILayout.HelpBox(
                    "LDtk Project asset configuration has unresolved issues, mouse over them to see the problem",
                    MessageType.Warning);
            }
        }
        

    }
}