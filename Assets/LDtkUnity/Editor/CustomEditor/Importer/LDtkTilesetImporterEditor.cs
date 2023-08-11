using System;
using LDtkUnity.InternalBridge;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LDtkTilesetImporter))]
    internal sealed class LDtkTilesetImporterEditor : LDtkSubImporterEditor
    {
        private LDtkTilesetImporter _importer;
        
        protected override bool useAssetDrawPreview => true;
        
        public override void OnEnable()
        {
            base.OnEnable();
            _importer = (LDtkTilesetImporter)target;
            
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
                
                if (!serializedObject.isEditingMultipleObjects)
                {
                    DoOpenSpriteEditorButton();
                }
                
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

        private void DoOpenSpriteEditorButton()
        {
            using (new EditorGUI.DisabledScope(targets.Length != 1))
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                
                bool button = false;
                using (new EditorGUIUtility.IconSizeScope(Vector2.one * 16))
                {
                    GUIContent spriteEditorContent = new GUIContent
                    {
                        text = "Sprite Editor",
                        tooltip = "Open this tileset file in the Sprite Editor window",
                        image = LDtkIconUtility.GetUnityIcon("Sprite")
                    };
                    button = GUILayout.Button(spriteEditorContent, GUILayout.Width(105));
                }
                
                if (button)
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