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
        private LDtkProjectImporter _projectImporter;
        
        protected override bool useAssetDrawPreview => true;
        
        private SerializedProperty _ppuProp;
        private SerializedProperty _overrideTextureProp;
        private LDtkTilesetDefinitionWrapper _tilesetDef;
        
        private static readonly GUIContent PpuContent = new GUIContent
        {
            text = "Pixels Per Unit",
            tooltip = "The pixels per unit for this tileset. It should match with the source project importer's pixels per unit.",
        };
        private static readonly GUIContent OverrideTextureContent = new GUIContent
        {
            text = "Override Texture",
            tooltip = "Choose a different texture, useful to make tile sprites of an increased resolution.\n" +
                      "Note: You can only choose textures that scale up equally to the original, like 1x, 2x, 3x, etc.\n" +
                      "You do NOT need to change the pixels per unit when using an override texture.",
        };
        
        public override void OnEnable()
        {
            base.OnEnable();
            _ppuProp = serializedObject.FindProperty(LDtkTilesetImporter.PIXELS_PER_UNIT);
            _overrideTextureProp = serializedObject.FindProperty(LDtkTilesetImporter.OVERRIDE_TEXTURE);
            CacheImporter();
            
            if (_importer == null || _importer.IsBackupFile())
            {
                return;
            }
            _projectImporter = _importer.GetProjectImporter();
            if (_projectImporter == null)
            {
                return;
            }
                
            _projectAsset = AssetDatabase.LoadMainAssetAtPath(_projectImporter.assetPath) as GameObject;
            _tilesetDef = LDtkTilesetImporter.FromJson<LDtkTilesetDefinitionWrapper>(_importer.assetPath);
        }

        private void CacheImporter()
        {
            _importer = (LDtkTilesetImporter)target;
        }

        public override void OnInspectorGUI()
        {
            //for testing ui
            //base.OnInspectorGUI();
            //return;
            
            LDtkProfiler.BeginSample("serializedObject.Update");
            serializedObject.Update();
            LDtkProfiler.EndSample();
            
            if (TryDrawBackupGui(_importer))
            {
                ApplyRevertGUI();
                return;
            }

            if (serializedObject.isEditingMultipleObjects)
            {
                DrawPpu();
                DrawRedirect();
                DrawDependenciesProperty();
                serializedObject.ApplyModifiedProperties();
                ApplyRevertGUI();
                return;
            }
            
            DrawProfilerButton();
            LDtkEditorGUIUtility.DrawDivider();
            DrawLogEntries();
            
            try
            {
                TryDrawProjectReferenceButton();
                DrawPpu();
                DrawRedirect();
                
                if (_projectImporter)
                {
                    if (_projectImporter.PixelsPerUnit != _importer._pixelsPerUnit)
                    {
                        EditorGUILayout.HelpBox($"This doesn't have the same pixels per unit as it's project \"{_projectImporter.AssetName}\" ({_projectImporter.PixelsPerUnit}). Ensure they match.", MessageType.Warning);
                    }
                }
                
                DrawDependenciesProperty();
                DoOpenSpriteEditorButton();
                SectionDependencies.Draw();
                
            }
            catch (Exception e)
            {   
                LDtkDebug.LogError(e.ToString());
                DrawTextBox();
            }
            
            serializedObject.ApplyModifiedProperties();
                
            LDtkProfiler.BeginSample("ApplyRevertGUI");
            ApplyRevertGUI();
            LDtkProfiler.EndSample();
        }

        protected override void Apply()
        {
            base.Apply();
            CacheImporter();
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

        private void DrawPpu()
        {
            EditorGUILayout.PropertyField(_ppuProp, PpuContent);
            
            //if manually reduced, never allow to go below 1
            if (_ppuProp.intValue <= 0)
            {
                _ppuProp.intValue = 1;
            }
        }

        private void DrawRedirect()
        {
            EditorGUILayout.PropertyField(_overrideTextureProp, OverrideTextureContent);

            if (!(_overrideTextureProp.objectReferenceValue is Texture2D overrideTexture))
            {
                return;
            }
            
            if (LDtkTilesetImporter.IsResolutionMultiple(overrideTexture.width, overrideTexture.height, _tilesetDef.Def.PxWid, _tilesetDef.Def.PxHei, out var multiplier))
            {
                EditorGUILayout.HelpBox($"Using multiple of {multiplier}.", MessageType.None, false);
                return;
            }
            
            EditorGUILayout.HelpBox("The resolution of the override texture must be a multiple of the original texture.", MessageType.Error, true);
        }
    }
}