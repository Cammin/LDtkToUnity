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

        private LDtkLevelImporter _importer;
        
        
        protected override bool needsApplyRevert => false;

        private GameObject _projectAsset;

        public override void OnEnable()
        {
            base.OnEnable();
            
            _importer = (LDtkLevelImporter)target;
            if (_importer == null || _importer.IsBackupFile())
            {
                return;
            }

            LDtkProjectImporter projectImporter = _importer.GetProjectImporter();
            if (projectImporter == null)
            {
                return;
            }
                
            _projectAsset = (GameObject)AssetDatabase.LoadMainAssetAtPath(projectImporter.assetPath);
            
            SectionDependencies.Init();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            SectionDependencies.Dispose();
        }

        public override void OnInspectorGUI()
        {
            if (TryDrawBackupGui(_importer))
            {
                return;
            }
            
            try
            {
                TryDrawProjectReferenceButton();
                SectionDependencies.Draw();
            }
            catch (Exception e)
            {   
                Debug.LogError(e);
                DrawBox();
            }
        }

        private void TryDrawProjectReferenceButton()
        {
            if (!_projectAsset)
            {
                DrawBox("Could not locate the source project asset. Make sure LDtk can also load this level from it's project, and try again.");
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
                    using (new EditorGUIUtility.IconSizeScope(Vector2.one * 16))
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