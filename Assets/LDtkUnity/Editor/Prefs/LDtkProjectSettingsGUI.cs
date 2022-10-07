using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    internal class LDtkProjectSettingsGUI
    {
        public const string ICON_LINK = "https://finalbossblues.itch.io/icons";
        
        private readonly SerializedObject _serializedObject;

        private readonly Action _saveAction;
        private readonly SerializedProperty _internalIconsTexture;

        private static readonly GUIContent InternalIconsTexture = new GUIContent
        {
            text = "Internal Icons Texture",
            tooltip = "LDtk has a tileset image embedded in LDtk, but cannot be redistributed in any way.\n" +
                      "To import any relevant icons, obtain this asset pack from FinalBossBlues and assign the image found in:\n\"icons_8.13.20/fullcolor/icons_full_16.png\".",
            image = EditorGUIUtility.IconContent("GameManager Icon").image
        };
        
        private static readonly GUIContent GetIconsButton = new GUIContent
        {
            text = "Get Icons",
            tooltip = "Opens a link to itch.io",
            image = LDtkIconUtility.GetUnityIcon("AssetStore")
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
                    if (tex.width != 256 || tex.height != 1024)
                    {
                        LDtkDebug.LogWarning("Only assigning the internal icons texture is valid. (Needs a 256x1024 resolution)");
                        _internalIconsTexture.objectReferenceValue = null;
                    }
                }
            }
            DrawItchButton();
            
            LDtkEditorGUIUtility.DrawDivider();

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
                if (GUILayout.Button(GetIconsButton, GUILayout.Width(100)))
                {
                    Application.OpenURL(ICON_LINK);
                }
            }
            

            EditorGUILayout.EndHorizontal();
        }
    }
}