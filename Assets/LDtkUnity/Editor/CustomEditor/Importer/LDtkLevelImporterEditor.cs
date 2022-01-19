using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelImporter))]
    internal class LDtkLevelImporterEditor : LDtkImporterEditor
    {
        protected override bool needsApplyRevert => false;

        private GameObject _projectAsset;

        public override void OnEnable()
        {
            base.OnEnable();
            LDtkLevelImporter importer = (LDtkLevelImporter)target;
            _projectAsset = importer.GetProjectAsset();
        }

        public override void OnInspectorGUI()
        {
            try
            {
                TryDrawProjectReferenceButton();
            }
            finally
            {
                ApplyRevertGUI();
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
                EditorGUILayout.ObjectField(buttonContent, _projectAsset, typeof(GameObject), false);
            }
        }
    }
}