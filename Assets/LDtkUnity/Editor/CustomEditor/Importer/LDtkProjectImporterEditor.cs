using System.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LDtkProjectImporter))]
    public class LDtkProjectImporterEditor : ScriptedImporterEditor
    {
        private static readonly GUIContent PixelsPerUnit = new GUIContent
        {
            text = "Main Pixels Per Unit",
            tooltip = "Dictates what all of the instantiated Tileset scales will adjust to, in case several LDtk layer's GridSize's are different."
        };
        private static readonly GUIContent DeparentInRuntime = new GUIContent
        {
            text = "De-parent in Runtime",
            tooltip = "When on, adds components to the project, levels, and entity-layer GameObjects that act to de-parent all of their children in runtime.\n" +
                      "This results in increased runtime performance.\n" +
                      "Keep this on if the exact level/layer hierarchy structure is not a concern in runtime."
                
        };
        private static readonly GUIContent LogBuildTimes = new GUIContent
        {
            text = "Log Build Times",
            tooltip = "Use this to display the count of levels built, and how long it took to generate them."
        };
        private static readonly GUIContent Atlas = new GUIContent
        {
            text = "Sprite Atlas",
            tooltip = "Create your own Sprite Atlas and assign it here if desired.\n" +
                      "This solves the \"tearing\" in the sprites of the tilemaps.\n" +
                      "The sprite atlas is reserved for auto-generated sprites only. Any foreign sprites assigned to the atlas will be removed."
        };
        private static readonly GUIContent IntGridVisible = new GUIContent()
        {
            text = "Render IntGrid Values",
            tooltip = "Use this if rendering the IntGrid value colors is preferred"
        };
        
        private LdtkJson _data;
        private ILDtkSectionDrawer[] _sectionDrawers;
        private ILDtkSectionDrawer _sectionIntGrids;
        private ILDtkSectionDrawer _sectionEntities;
        private ILDtkSectionDrawer _sectionEnums;
        private bool _isFirstUpdate = true;
        
        public override bool showImportedObject => false;
        protected override bool useAssetDrawPreview => false;
        protected override bool ShouldHideOpenButton() => false;

        public override void OnEnable()
        {
            base.OnEnable();
            
            _sectionIntGrids = new LDtkSectionIntGrids(serializedObject);
            _sectionEntities = new LDtkSectionEntities(serializedObject);
            _sectionEnums = new LDtkSectionEnums(serializedObject);
            
            _sectionDrawers = new[]
            {
                _sectionIntGrids,
                _sectionEntities,
                _sectionEnums,
            };

            foreach (ILDtkSectionDrawer drawer in _sectionDrawers)
            {
                drawer.Init();
            }
            
        }

        public override void OnDisable()
        {
            foreach (ILDtkSectionDrawer drawer in _sectionDrawers)
            {
                drawer.Dispose();
            }
            base.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            try
            {
                serializedObject.Update();
                ShowGUI();
                serializedObject.ApplyModifiedProperties();
                
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
            EditorGUIUtility.SetIconSize(Vector2.one * 16);
            
            if (!AssignJsonField() || _data == null)
            {
                return;
            }

            Definitions defs = _data.Defs;
            
            DrawField(PixelsPerUnit, LDtkProjectImporter.PIXELS_PER_UNIT);

            //draw the sprite atlas only if we have tiles to pack essentially
            if (!_data.Defs.Tilesets.IsNullOrEmpty())
            {
                DrawField(Atlas, LDtkProjectImporter.ATLAS);
            }

            DrawField(DeparentInRuntime, LDtkProjectImporter.DEPARENT_IN_RUNTIME);
            DrawField(LogBuildTimes, LDtkProjectImporter.LOG_BUILD_TIMES);

            if (!_data.Defs.IntGridLayers.IsNullOrEmpty())
            {
                DrawField(IntGridVisible, LDtkProjectImporter.INTGRID_VISIBLE);
            }
            
            _sectionIntGrids.Draw(defs.IntGridLayers);
            _sectionEntities.Draw(defs.Entities);
            _sectionEnums.Draw(defs.Enums);

            LDtkEditorGUIUtility.DrawDivider();
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

        /// <summary>
        /// Returns if this method had a problem.
        /// </summary>
        /*private bool LevelFieldsPrefabField(FieldDefinition[] defsEntityLayers)
        {
            bool selectingSingleObject = Selection.count == 1;
            
            //dont draw it if we don't define level fields. but also always draw if we have more than one selection
            if (selectingSingleObject && (defsEntityLayers == null || defsEntityLayers.Length == 0))
            {
                return false;
            }
            
            SerializedProperty levelFieldsProp = serializedObject.FindProperty(LDtkProjectImporter.LEVEL_FIELDS_PREFAB);
            
            
            Rect controlRect = EditorGUILayout.GetControlRect();
            
            if (levelFieldsProp.objectReferenceValue == null && selectingSingleObject)
            {
                LDtkEditorGUI.DrawFieldWarning(controlRect, "The LDtk project has level fields defined, but there is no scripted level prefab assigned here.");
            }
            
            EditorGUI.PropertyField(controlRect, levelFieldsProp, LevelFields);
            return selectingSingleObject && levelFieldsProp.objectReferenceValue == null;
        }*/

        private void DrawField(GUIContent content, string propName)
        {
            SerializedProperty pixelsPerUnitProp = serializedObject.FindProperty(propName);
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
                //Debug.Log("Applied an array resize and reimported as a result");
            }
        }
        
        private void DrawPotentialProblem()
        {
            bool problem = _sectionDrawers.Any(drawer => drawer.HasProblem);

            if (problem)
            {
                EditorGUIUtility.SetIconSize(Vector2.one * 32);
                EditorGUILayout.HelpBox(
                    "LDtk Project asset configuration has unresolved issues, mouse over them to see the problem",
                    MessageType.Warning);
            }
        }
    }
}