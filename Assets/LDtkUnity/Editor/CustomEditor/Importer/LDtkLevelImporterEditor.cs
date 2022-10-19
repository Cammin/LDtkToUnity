using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LDtkLevelImporter))]
    internal sealed class LDtkLevelImporterEditor : LDtkImporterEditor
    {
        private static readonly GUIContent ReimportProjectButton = new GUIContent()
        {
            text = "Reimport Project",
            tooltip = "Reimport this level's project."
        };

        private LDtkLevelImporter _importer;
        private GUIContent _buttonContent;
        private GameObject _projectAsset;

        public override void OnEnable()
        {
            base.OnEnable();
            
            _buttonContent = new GUIContent
            {
                text = "Source Project",
                image = LDtkIconUtility.LoadProjectFileIcon()
            };

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
            SectionDependencies.Dispose();
            base.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            if (TryDrawBackupGui(_importer))
            {
                ApplyRevertGUI();
                return;
            }
            
            if (serializedObject.isEditingMultipleObjects)
            {
                DrawDependenciesProperty();
                serializedObject.ApplyModifiedProperties();
                ApplyRevertGUI();
                return;
            }
            
            try
            {
                TryDrawProjectReferenceButton();
                DrawDependenciesProperty();
                SectionDependencies.Draw();
            }
            catch (Exception e)
            {   
                LDtkDebug.LogError(e.ToString());
                DrawTextBox();
            }

            serializedObject.ApplyModifiedProperties();
            
            ApplyRevertGUI();
        }

        private void TryDrawProjectReferenceButton()
        {
            if (!_projectAsset)
            {
                DrawTextBox("Could not locate the source project asset. Make sure LDtk can also load this level from it's project, and try again.");
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    using (new EditorGUIUtility.IconSizeScope(Vector2.one * 16))
                    {
                        EditorGUILayout.ObjectField(_buttonContent, _projectAsset, typeof(GameObject), false);
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