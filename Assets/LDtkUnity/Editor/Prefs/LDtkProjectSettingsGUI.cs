using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkProjectSettingsGUI
    {
        public const string GITHUB_LINK = "https://github.com/deepnight/ldtk/blob/master/app/assets/embedAtlas/finalbossblues-icons_full_16.png";
        public const string ITCH_LINK = "https://finalbossblues.itch.io/icons";
        
        private readonly SerializedObject _serializedObject;

        private readonly Action _saveAction;
        private readonly SerializedProperty _internalIconsTexture;
        private readonly SerializedProperty _revertOverridesInScene;
        
        private readonly GUILayoutOption _buttonWidth = GUILayout.Width(180);
        private readonly GUILayoutOption _buttonWidthSmall = GUILayout.Width(88.5f);
        private readonly EditorGUIUtility.IconSizeScope _iconSizeScope = new EditorGUIUtility.IconSizeScope(new Vector2(16, 16));

        private static readonly GUIContent InternalIconsTexture = new GUIContent
        {
            text = "Internal Icons Texture",
            tooltip = "LDtk has a tileset image embedded in LDtk, but cannot be redistributed in any way.\n" +
                      "Because it was customized to include some extra icons created by deepnight, it is only available to download from the LDtk repository.",
            
#if UNITY_2021_1_OR_NEWER
            image = LDtkIconUtility.GetUnityIcon("Settings")
#else
            image = EditorGUIUtility.IconContent("GameManager Icon").image
#endif
        };
        
        private static readonly GUIContent GithubButton = new GUIContent
        {
            text = "Get Icons",
            tooltip = "Opens a link to GitHub",
            image = LDtkIconUtility.GetUnityIcon("Texture")
        };        
        private static readonly GUIContent ItchButton = new GUIContent
        {
            text = "Support",
            tooltip = "Most icons made by FinalBossBlues. Support the artist by buying the icons on itch.io",
            image = LDtkIconUtility.GetUnityIcon("AssetStore")
        };
        
        private static readonly GUIContent ReimportAllButton = new GUIContent
        {
            text = "Reimport all LDtk assets",
            tooltip = "Reimports all LDtk projects and levels. Useful as a shortcut to reimport everything at once.",
            image = LDtkIconUtility.GetUnityIcon("Refresh", "")
        };
        private static readonly GUIContent ReimportAllProjectsButton = new GUIContent
        {
            text = "Reimport all .ldtk assets ",
            tooltip = "Reimports all projects",
            image = LDtkIconUtility.LoadProjectFileIcon()
        };
        private static readonly GUIContent ReimportAllLevelsButton = new GUIContent
        {
            text = "Reimport all .ldtkl assets",
            tooltip = "Reimports all levels",
            image = LDtkIconUtility.LoadLevelFileIcon()
        };
        private static readonly GUIContent ReimportAllTilesetFilesButton = new GUIContent
        {
            text = "Reimport all .ldtkt assets",
            tooltip = "Reimports all tileset files",
            image = LDtkIconUtility.LoadTilesetFileIcon()
        };
        private static readonly GUIContent RevertOverridesInScene = new GUIContent
        {
            text = "Revert Overrides On Import",
            tooltip = "If enabled, then after any LDtk asset import, will attempt a prefab revert of all LDtk prefab instances in all currently loaded scenes.\n" +
                      "This is used for when you always want to maintain an unchanged state of the imported hierarchy in the scene, because Unity's tilemaps and other various components can accidentally change without notice nor intention.",
            image = LDtkIconUtility.GetUnityIcon("PrefabModel On")
        };
        private static readonly GUIContent RevertOverridesInSceneButton = new GUIContent
        {
            text = "Revert Now",
            tooltip = "Revert all LDtk prefab instances in all loaded scenes immediately",
        };

        public LDtkProjectSettingsGUI(SerializedObject obj, Action saveAction)
        {
            _saveAction = saveAction;
            _serializedObject = obj;
            _internalIconsTexture = obj.FindProperty(LDtkProjectSettings.PROPERTY_INTERAL_ICONS_TEXTURE);
            _revertOverridesInScene = obj.FindProperty(LDtkProjectSettings.PROPERTY_REVERT_OVERRIDES_IN_SCENE);
        }
        
        public void OnGUI(string searchContext)
        {
            _serializedObject.Update();

            EditorGUIUtility.labelWidth = 200;

            LDtkSettingsSwitchGUI.DrawSwitchPrefsButton();
            LDtkEditorGUIUtility.DrawDivider();
            
            LDtkScriptingDefines.PreprocessorAddRemoveGui();
            
            using (_iconSizeScope)
            {
                DrawFieldInternalIcons();
                DrawRevertInstancesField();
                DrawItchButtons();
                LDtkEditorGUIUtility.DrawDivider();
                DrawReimportAllButtons();
            }
            
            if (_serializedObject.ApplyModifiedPropertiesWithoutUndo())
            {
                _saveAction?.Invoke();
            }
        }

        private void DrawFieldInternalIcons()
        {
            EditorGUILayout.PropertyField(_internalIconsTexture, InternalIconsTexture);
            if (_internalIconsTexture.objectReferenceValue is Texture2D tex)
            {
                if (tex.width != 512 || tex.height != 1024)
                {
                    LDtkDebug.LogWarning("Only assigning the internal icons texture is valid. (Needs a 512x1024 resolution)");
                    _internalIconsTexture.objectReferenceValue = null;
                }
            }
        }

        private void DrawItchButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(GithubButton, _buttonWidthSmall))
            {
                Application.OpenURL(GITHUB_LINK);
            }
            if (GUILayout.Button(ItchButton, _buttonWidthSmall))
            {
                Application.OpenURL(ITCH_LINK);
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawRevertInstancesField()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_revertOverridesInScene, RevertOverridesInScene);
            if (GUILayout.Button(RevertOverridesInSceneButton, _buttonWidth))
            {
                LDtkPostImportSceneAlterations.QueueRevertPrefabs(InteractionMode.UserAction);
            }
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawReimportAllButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            
            if (GUILayout.Button(ReimportAllButton, _buttonWidth))
            {
                WrapInAssetEditing(ReimportAll);
            }
            if (GUILayout.Button(ReimportAllProjectsButton, _buttonWidth))
            {
                WrapInAssetEditing(() =>
                {
                    string[] allPaths = AssetDatabase.GetAllAssetPaths();
                    ReimportAllFiles(allPaths, ".ldtk");
                });
            }
            if (GUILayout.Button(ReimportAllLevelsButton, _buttonWidth))
            {
                WrapInAssetEditing(() =>
                {
                    string[] allPaths = AssetDatabase.GetAllAssetPaths();
                    ReimportAllFiles(allPaths, ".ldtkl");
                });
            }
            if (GUILayout.Button(ReimportAllTilesetFilesButton, _buttonWidth))
            {
                WrapInAssetEditing(() =>
                {
                    string[] allPaths = AssetDatabase.GetAllAssetPaths();
                    ReimportAllFiles(allPaths, ".ldtkt");
                });
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private static void ReimportAll()
        {
            string[] allPaths = AssetDatabase.GetAllAssetPaths();
            
            //tilesets, then projects, then levels.
            ReimportAllFiles(allPaths, ".ldtkl");
            ReimportAllFiles(allPaths, ".ldtk");
            ReimportAllFiles(allPaths, ".ldtkt");
        }

        private static void WrapInAssetEditing(Action action)
        {
            try
            {
                AssetDatabase.StartAssetEditing();
                action.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }
        
        private static void ReimportAllFiles(string[] allPaths, string ext)
        {
            foreach (string assetPath in allPaths)
            {
                if (Path.GetExtension(assetPath) == ext)
                {
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.Default);
                }
            }
        }
    }
}