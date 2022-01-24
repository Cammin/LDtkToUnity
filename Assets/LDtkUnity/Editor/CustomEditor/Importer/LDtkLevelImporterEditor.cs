using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelImporter))]
    internal class LDtkLevelImporterEditor : LDtkImporterEditor
    {
        private static readonly GUIContent ReimportProjectButton = new GUIContent()
        {
            text = "Reimport Project",
            tooltip = "Reimport this level's project."
        };
        
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
            catch (Exception e)
            {   
                Debug.LogError(e);
                DrawBreakingError();
            }
        }

        private void TryDrawProjectReferenceButton()
        {
            if (!_projectAsset)
            {
                DrawBreakingError();
                return;
            }

            GUIContent buttonContent = new GUIContent()
            {
                text = "LDtk Project",
                image = LDtkIconUtility.LoadProjectFileIcon()
            };
            
            
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    using (new LDtkIconSizeScope(16))
                    {
                        EditorGUILayout.ObjectField(buttonContent, _projectAsset, typeof(GameObject), false);
                    }
                }

                if (GUILayout.Button(ReimportProjectButton, GUILayout.Width(105)))
                {
                    string assetPath = AssetDatabase.GetAssetPath(_projectAsset);
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
            }
        }
    }
}