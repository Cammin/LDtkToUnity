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

        public LDtkProjectSettingsGUI(SerializedObject obj, Action saveAction)
        {
            _saveAction = saveAction;
            _serializedObject = obj;
            _internalIconsTexture = obj.FindProperty(LDtkProjectSettings.PROPERTY_INTERAL_ICONS_TEXTURE);
        }
        
        public void OnGUI(string searchContext)
        {
            _serializedObject.Update();

            EditorGUIUtility.labelWidth = 200;

            LDtkSettingsSwitchGUI.DrawSwitchPrefsButton();
            LDtkEditorGUIUtility.DrawDivider();
            
            using (new EditorGUIUtility.IconSizeScope(new Vector2(16, 16)))
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
            DrawItchButton();
            LDtkEditorGUIUtility.DrawDivider();
            DrawReimportAllButton();
            

            if (_serializedObject.ApplyModifiedPropertiesWithoutUndo())
            {
                _saveAction?.Invoke();
            }
        }

        private static void DrawItchButton()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            using (new EditorGUIUtility.IconSizeScope(new Vector2(16, 16)))
            {
                if (GUILayout.Button(GithubButton, GUILayout.Width(89)))
                {
                    Application.OpenURL(GITHUB_LINK);
                }
                if (GUILayout.Button(ItchButton, GUILayout.Width(88)))
                {
                    Application.OpenURL(ITCH_LINK);
                }
            }
            
            EditorGUILayout.EndHorizontal();
        }
        private static void DrawReimportAllButton()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            
            using (new EditorGUIUtility.IconSizeScope(new Vector2(16, 16)))
            {
                if (GUILayout.Button(ReimportAllButton, GUILayout.Width(180)))
                {
                    WrapInAssetEditing(ReimportAll);
                }
                if (GUILayout.Button(ReimportAllProjectsButton, GUILayout.Width(180)))
                {
                    WrapInAssetEditing(() =>
                    {
                        string[] allPaths = AssetDatabase.GetAllAssetPaths();
                        ReimportAllFiles(allPaths, ".ldtk");
                    });
                }
                if (GUILayout.Button(ReimportAllLevelsButton, GUILayout.Width(180)))
                {
                    WrapInAssetEditing(() =>
                    {
                        string[] allPaths = AssetDatabase.GetAllAssetPaths();
                        ReimportAllFiles(allPaths, ".ldtkl");
                    });
                }
                if (GUILayout.Button(ReimportAllTilesetFilesButton, GUILayout.Width(180)))
                {
                    WrapInAssetEditing(() =>
                    {
                        string[] allPaths = AssetDatabase.GetAllAssetPaths();
                        ReimportAllFiles(allPaths, ".ldtkt");
                    });
                }
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