using System;
using System.Linq;
using LDtkUnity.InternalBridge;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LDtkTilesetImporter))]
    internal sealed class LDtkTilesetImporterEditor : LDtkImporterEditor
    {
        private LDtkTilesetImporter _importer;

        public override bool showImportedObject => true;
        protected override bool useAssetDrawPreview => true;
        

        public override void OnEnable()
        {
            base.OnEnable();

            _importer = (LDtkTilesetImporter)target;
            
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (!serializedObject.isEditingMultipleObjects)
            {
                DoOpenSpriteEditorButton();
            }
            
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
                DrawDependenciesProperty();
                LDtkEditorGUIUtility.DrawDivider();
                base.OnInspectorGUI();
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

        private void DoOpenSpriteEditorButton()
        {
            using (new EditorGUI.DisabledScope(targets.Length != 1))
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Sprite Editor"))
                {
                    if (HasModified())
                    {
                        // To ensure Sprite Editor Window to have the latest texture import setting,
                        // We must applied those modified values first.
                        var dialogText = $"Unapplied import settings for \'{_importer.assetPath}\'.\nApply and continue to sprite editor or cancel.";
                        if (EditorUtility.DisplayDialog("Unapplied import settings", dialogText, "Apply", "Cancel"))
                        {
#if UNITY_2022_2_OR_NEWER
                            SaveChanges();
#else
                            ApplyAndImport();
#endif
                            InternalEditorBridge.ShowSpriteEditorWindow(this.assetTarget);

                            // We re-imported the asset which destroyed the editor, so we can't keep running the UI here.
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        InternalEditorBridge.ShowSpriteEditorWindow(this.assetTarget);
                    }
                }
                GUILayout.EndHorizontal();
            }    
        }
    }
}