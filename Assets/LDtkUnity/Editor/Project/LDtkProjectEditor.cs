using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkProject))]
    public class LDtkProjectEditor : UnityEditor.Editor
    {
        private LdtkJson _data;

        private ILDtkProjectSectionDrawer[] _sectionDrawers;
        
        private ILDtkProjectSectionDrawer _sectionLevels;
        private ILDtkProjectSectionDrawer _sectionLevelBackgrounds;
        private ILDtkProjectSectionDrawer _sectionIntGrids;
        private ILDtkProjectSectionDrawer _sectionEntities;
        private ILDtkProjectSectionDrawer _sectionEnums;
        private ILDtkProjectSectionDrawer _sectionTilesets;
        private ILDtkProjectSectionDrawer _sectionTileAssets;
        private ILDtkProjectSectionDrawer _sectionGridPrefabs;

        private bool _levelFieldsError;
        
        private void OnEnable()
        {
            _sectionLevels = new LDtkProjectSectionLevels(serializedObject);
            _sectionLevelBackgrounds = new LDtkProjectSectionLevelBackgrounds(serializedObject);
            _sectionIntGrids = new LDtkProjectSectionIntGrids(serializedObject);
            _sectionEntities = new LDtkProjectSectionEntities(serializedObject);
            _sectionEnums = new LDtkProjectSectionEnums(serializedObject);
            _sectionTilesets = new LDtkProjectSectionTilesets(serializedObject);
            _sectionTileAssets = new LDtkProjectSectionTileCollections(serializedObject);
            _sectionGridPrefabs = new LDtkProjectSectionGridPrefabs(serializedObject);
            
            _sectionDrawers = new[]
            {
                _sectionLevels,
                _sectionLevelBackgrounds,
                _sectionIntGrids,
                _sectionEntities,
                _sectionEnums,
                _sectionTilesets,
                _sectionTileAssets,
                _sectionGridPrefabs
            };

            foreach (ILDtkProjectSectionDrawer drawer in _sectionDrawers)
            {
                drawer.Init();
            }
        }

        private void OnDisable()
        {
            foreach (ILDtkProjectSectionDrawer drawer in _sectionDrawers)
            {
                drawer.Dispose();
            }
        }
        
        public override void OnInspectorGUI()
        {
            //at the start of all drawing, set icon size for some GuiContents
            EditorGUIUtility.SetIconSize(Vector2.one * 16);
            
            serializedObject.Update();
            ShowGUI();
            serializedObject.ApplyModifiedProperties();
            
            EditorGUIUtility.SetIconSize(Vector2.one * 32);
            DrawPotentialProblem();
        }

        private void ShowGUI()
        {
            SerializedProperty jsonProp = serializedObject.FindProperty(LDtkProject.JSON);

            if (!AssignJsonField(jsonProp) || _data == null)
            {
                return;
            }
            

            if (!DrawIsExternalLevels())
            {
                return;
            }
            
            Definitions defs = _data.Defs;
            
            PixelsPerUnitField();
            DeparentInRuntimeField();
            _levelFieldsError = LevelFieldsPrefabField(defs.LevelFields);

            
            _sectionLevels.Draw(_data.Levels);
            _sectionLevelBackgrounds.Draw(_data.Levels.Where(level => !string.IsNullOrEmpty(level.BgRelPath)));
            _sectionIntGrids.Draw(defs.IntGridLayers);
            _sectionEntities.Draw(defs.Entities);
            _sectionEnums.Draw(defs.Enums);
            _sectionTilesets.Draw(defs.Tilesets);
            _sectionTileAssets.Draw(defs.Tilesets);
            _sectionGridPrefabs.Draw(defs.UnityGridLayers);
            
            LDtkDrawerUtil.DrawDivider();
        }

        private bool AssignJsonField(SerializedProperty jsonProp)
        {
            Object prevObj = jsonProp.objectReferenceValue;
            EditorGUILayout.PropertyField(jsonProp);
            Object newObj = jsonProp.objectReferenceValue;
            
            if (newObj == null)
            {
                return false;
            }
            
            LDtkProjectFile jsonFile = (LDtkProjectFile)jsonProp.objectReferenceValue;
            
            if (!ReferenceEquals(prevObj, newObj))
            {
                _data = null;

                if (jsonFile.FromJson == null) //todo ensure this false loading is actually detected
                {
                    Debug.LogError("LDtk: Invalid LDtk format");
                    jsonProp.objectReferenceValue = null;
                    return false;
                }
            }
            
            if (_data == null)
            {
                _data = jsonFile.FromJson;
            }

            return true;
        }
        
        private bool DrawIsExternalLevels()
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
        }
        
        /// <summary>
        /// Returns if this method had a problem.
        /// </summary>
        private bool LevelFieldsPrefabField(FieldDefinition[] defsEntityLayers)
        {
            if (defsEntityLayers == null || defsEntityLayers.Length == 0)
            {
                return false;
            }
            
            SerializedProperty levelFieldsProp = serializedObject.FindProperty(LDtkProject.LEVEL_FIELDS_PREFAB);
            
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
            SerializedProperty pixelsPerUnitProp = serializedObject.FindProperty(LDtkProject.PIXELS_PER_UNIT);
            
            GUIContent content = new GUIContent()
            {
                text = pixelsPerUnitProp.displayName,
                tooltip = "Dictates what all of the instantiated Tileset scales will adjust to, in case several LDtk layer's GridSize's are different."
            };
            
            EditorGUILayout.PropertyField(pixelsPerUnitProp, content);
        }

        

        private void DeparentInRuntimeField()
        {
            SerializedProperty deparentProp = serializedObject.FindProperty(LDtkProject.DEPARENT_IN_RUNTIME);
            
            GUIContent content = new GUIContent()
            {
                text = deparentProp.displayName,
                tooltip = "When on, adds components to the project, levels, and entity-layer GameObjects that act to de-parent all of their children in runtime.\n" +
                          "This results in increased runtime performance.\n" +
                          "Keep this on if the exact level/layer hierarchy structure is not a concern in runtime."
                
            };
            
            EditorGUILayout.PropertyField(deparentProp, content);
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