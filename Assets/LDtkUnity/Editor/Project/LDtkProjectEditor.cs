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
        private bool _internalDataDropdown;

        private ILDtkProjectSectionDrawer[] _sectionDrawers;
        
        private ILDtkProjectSectionDrawer _sectionLevels;
        private ILDtkProjectSectionDrawer _sectionIntGrids;
        private ILDtkProjectSectionDrawer _sectionEntities;
        private ILDtkProjectSectionDrawer _sectionEnums;
        private ILDtkProjectSectionDrawer _sectionTilesets;
        private ILDtkProjectSectionDrawer _sectionTileAssets;
        private ILDtkProjectSectionDrawer _sectionGridPrefabs;
        
        private void OnEnable()
        {
            _sectionLevels = new LDtkProjectSectionLevels(serializedObject);
            _sectionIntGrids = new LDtkProjectSectionIntGrids(serializedObject);
            _sectionEntities = new LDtkProjectSectionEntities(serializedObject);
            _sectionEnums = new LDtkProjectSectionEnums(serializedObject);
            _sectionTilesets = new LDtkProjectSectionTilesets(serializedObject);
            _sectionTileAssets = new LDtkProjectSectionTileCollections(serializedObject);
            _sectionGridPrefabs = new LDtkProjectSectionGridPrefabs(serializedObject);
            
            _sectionDrawers = new[]
            {
                _sectionLevels,
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

            _internalDataDropdown = EditorPrefs.GetBool(nameof(_internalDataDropdown), true);
        }

        private void OnDisable()
        {
            foreach (ILDtkProjectSectionDrawer drawer in _sectionDrawers)
            {
                drawer.Dispose();
            }

            EditorPrefs.SetBool(nameof(_internalDataDropdown), _internalDataDropdown);
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

            //DrawInternalData();
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
            SerializedProperty jsonProp = serializedObject.FindProperty(LDtkProject.JSON);

            if (!AssignJsonField(jsonProp) || _data == null)
            {
                return;
            }
            

            if (!DrawIsExternalLevels())
            {
                return;
            }
            
            PixelsPerUnitField();
            

            Definitions defs = _data.Defs;
            
            _sectionLevels.Draw(_data.Levels);
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
                LDtkIconLoader.LoadLevelIcon(),
                "The option \"Save Levels To Separate Files\" is a requirement");
            EditorGUILayout.HelpBox(content);

            return false;
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
        
        
        
        private void DrawPotentialProblem()
        {
            bool problem = _sectionDrawers.Any(drawer => drawer.HasProblem);

            if (problem)
            {
                EditorGUILayout.HelpBox(
                    "LDtk Project asset configuration has unresolved issues, mouse over them to see the problem",
                    MessageType.Warning);
            }
        }
    }
}