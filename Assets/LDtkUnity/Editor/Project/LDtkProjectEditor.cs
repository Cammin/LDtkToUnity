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

        private ILDtkProjectSectionDrawer _sectionLevels;
        private ILDtkProjectSectionDrawer _sectionIntGrids;
        private ILDtkProjectSectionDrawer _sectionEntities;
        private ILDtkProjectSectionDrawer _sectionEnums;
        private ILDtkProjectSectionDrawer _sectionTilesets;
        private ILDtkProjectSectionDrawer _sectionTiles;
        
        private void OnEnable()
        {
            _sectionLevels = new LDtkProjectSectionLevels(serializedObject);
            _sectionIntGrids = new LDtkProjectSectionIntGrids(serializedObject);
            _sectionEntities = new LDtkProjectSectionEntities(serializedObject);
            _sectionEnums = new LDtkProjectSectionEnums(serializedObject);
            _sectionTilesets = new LDtkProjectSectionTilesets(serializedObject);
            _sectionTiles = new LDtkProjectSectionTiles(serializedObject);
            
            _sectionLevels.Init();
            _sectionIntGrids.Init();
            _sectionEntities.Init();
            _sectionEnums.Init();
            _sectionTilesets.Init();
            _sectionTiles.Init();
            
            _internalDataDropdown = EditorPrefs.GetBool(nameof(_internalDataDropdown), true);
        }

        private void OnDisable()
        {
            _sectionLevels.Dispose();
            _sectionIntGrids.Dispose();
            _sectionEntities.Dispose();
            _sectionEnums.Dispose();
            _sectionTilesets.Dispose();
            _sectionTiles.Dispose();
            
            EditorPrefs.SetBool(nameof(_internalDataDropdown), _internalDataDropdown);
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ShowGUI();
            serializedObject.ApplyModifiedProperties();

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
            
            GridField();
            PixelsPerUnitField();
            IntGridValuesVisibleField();
            
            _sectionLevels.Draw(_data.Levels);
            _sectionIntGrids.Draw(_data.Defs.IntGridLayers);
            _sectionEntities.Draw(_data.Defs.Entities);
            _sectionEnums.Draw(_data.Defs.Enums);
            _sectionTilesets.Draw(_data.Defs.Tilesets);
            _sectionTiles.Draw(_data.Defs.Tilesets);
            
            LDtkDrawerUtil.DrawDivider();
            
            DrawPotentialProblem();
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
            
            //todo a lot boilerplate. minimise down. mainly the drawing of the message bubble since its used more than once
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
        
        private void DrawPotentialProblem()
        {
            bool problem =
                _sectionLevels.HasProblem ||
                _sectionIntGrids.HasProblem ||
                _sectionEntities.HasProblem ||
                _sectionEnums.HasProblem ||
                _sectionTilesets.HasProblem;

            if (problem)
            {
                EditorGUILayout.HelpBox(
                    "LDtk Project asset configuration has unresolved issues, mouse over them to see the problem",
                    MessageType.Warning);
            }
        }
    }
}