using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LDtkLevelImporter))]
    internal sealed class LDtkLevelImporterEditor : LDtkSubImporterEditor
    {
        private LDtkLevelImporter _importer;

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
                DrawLogEntries();
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

        
    }
}