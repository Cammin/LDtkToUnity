using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelImporter))]
    internal class LDtkLevelImporterEditor : LDtkImporterEditor
    {
        public override bool showImportedObject => true;
        protected override bool needsApplyRevert => false;

        private GameObject _projectAsset;

        public override void OnEnable()
        {
            base.OnEnable();
            LDtkLevelImporter importer = (LDtkLevelImporter)target;

            if (importer != null)
            {
                _projectAsset = importer.GetProjectAsset();
            }
        }

        public override void OnInspectorGUI()
        {
            try
            {
                TryDrawProjectReferenceButton();
            }
            catch
            {   
                DrawBreakingError();
            }
        }

        private void TryDrawProjectReferenceButton()
        {
            if (!_projectAsset)
            {
                return;
            }

            GUIContent buttonContent = new GUIContent()
            {
                text = "LDtk Project",
                image = LDtkIconUtility.LoadProjectFileIcon()
            };
            
            using (new LDtkGUIScope(false))
            {

                using (new LDtkIconSizeScope(16))
                {
                    EditorGUILayout.ObjectField(buttonContent, _projectAsset, typeof(GameObject), false);
                }
                
            }
        }
    }
}