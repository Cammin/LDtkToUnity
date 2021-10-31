using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LDtkProjectImporter))]
    public class LDtkProjectImporterEditor : ScriptedImporterEditor
    {
        private LdtkJson _data;
        
        private ILDtkSectionDrawer[] _sectionDrawers;
        private LDtkSectionMain _sectionMain;
        private LDtkSectionIntGrids _sectionIntGrids;
        private LDtkSectionEntities _sectionEntities;
        private LDtkSectionEnums _sectionEnums;
        private bool _isFirstUpdate = true;
        
        public override bool showImportedObject => false;
        protected override bool useAssetDrawPreview => false;
        protected override bool ShouldHideOpenButton() => false;

        public override void OnEnable()
        {
            base.OnEnable();

            _sectionMain = new LDtkSectionMain(serializedObject);
            _sectionIntGrids = new LDtkSectionIntGrids(serializedObject);
            _sectionEntities = new LDtkSectionEntities(serializedObject);
            _sectionEnums = new LDtkSectionEnums(serializedObject);
            
            _sectionDrawers = new[]
            {
                (ILDtkSectionDrawer)_sectionMain,
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
            if (_sectionDrawers == null)
            {
                return;
            }
            foreach (ILDtkSectionDrawer drawer in _sectionDrawers)
            {
                drawer?.Dispose();
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

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }

        private void ShowGUI()
        {

            EditorGUIUtility.SetIconSize(Vector2.one * 16);
            
            
            if (!CacheJson() || _data == null)
            {
                const string errorContent = "There was a breaking import error; Try reimporting this project, which might fix it.\n" +
                                            "Check if there are any import errors in the console window, and report to the developer so that it can be addressed.";

                using (new LDtkIconSizeScope(Vector2.one * 32))
                {
                    EditorGUILayout.HelpBox(errorContent, MessageType.Error);
                }
                return;
            }
            
            DrawExportButton();
            _sectionMain.SetJson(_data);

            Definitions defs = _data.Defs;
            
            
            _sectionMain.Draw();
            _sectionIntGrids.Draw(defs.IntGridLayers);
            _sectionEntities.Draw(defs.Entities);
            _sectionEnums.Draw(defs.Enums);

            LDtkEditorGUIUtility.DrawDivider();
        }

        private void DrawExportButton()
        {
            GUIContent content = new GUIContent()
            {
                text = "Export",
                tooltip = "Export Native Prefab"
            };
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool button = GUILayout.Button(content, GUILayout.Width(45));
            GUILayout.EndHorizontal();

            if (button)
            {
                GameObject gameObject = (GameObject)assetTarget;
                LDtkNativeExportWindow.CreateWindowWithContext(gameObject);
            }
            
            LDtkEditorGUIUtility.DrawDivider();
        }

        private bool CacheJson()
        {
            SerializedProperty jsonProp = serializedObject.FindProperty(LDtkProjectImporter.JSON);
            
            if (_data != null)
            {
                return true;
            }
            
            Object jsonAsset = jsonProp.objectReferenceValue;
            if (jsonAsset == null)
            {
                //Debug.LogError("LDtk: Json asset is null, resulting in the importer inspector not drawing. This is not expected to happen. Try reimporting to fix. Also, check if there are any import errors and report to the developer so that it can be addressed.");
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
        
        private void ApplyIfArraySizesChanged()
        {
            //IMPORTANT: if there are any new/removed array elements via this setup of automatically resizing arrays as LDtk definitions change,
            //then Unity's going to notice and make the apply/revert buttons appear active which normally gives us trouble when we try clicking out.
            //So, try applying right now when this specific case happens; whenever there is an array resize.
            
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