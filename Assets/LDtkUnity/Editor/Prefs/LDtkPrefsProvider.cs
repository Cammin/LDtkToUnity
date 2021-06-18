using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LDtkUnity.Editor
{
    public class LDtkPrefsProvider : SettingsProvider
    {
        public const string PATH = "Editor/Prefs/LDtkUnityPrefs.asset"; 
        public const string PREFS_PATH = "Preferences/LDtk To Unity"; 
 
        private SerializedObject _settings;

        private LDtkPrefsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
            
        }
        
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = LDtkPrefs.GetSerializedSettings();
        }

        public override void OnGUI(string searchContext)
        {
            _settings.Update();
            
            EditorGUIUtility.labelWidth = 200;
            EditorGUILayout.Space();
            
            using (new LDtkIndentScope())
            {
                EditorGUILayout.Space();

                GUIStyle style = EditorStyles.miniBoldLabel;                
                
                _settings.DrawField(LDtkPrefs.PROP_LOG_BUILD_TIMES);
                LDtkEditorGUIUtility.DrawDivider();
                
                //EditorGUILayout.LabelField("Level Handles", style);
                _settings.DrawField(LDtkPrefs.PROP_SHOW_LEVEL_IDENTIFIER);
                SerializedProperty levelBorderProp = _settings.DrawField(LDtkPrefs.PROP_SHOW_LEVEL_BORDER);
                if (levelBorderProp.boolValue)
                {
                    _settings.DrawField(LDtkPrefs.PROP_SHOW_LEVEL_BORDER_THICKNESS);
                }
                LDtkEditorGUIUtility.DrawDivider();

                //EditorGUILayout.LabelField("Entity Handles", style);
                _settings.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_IDENTIFIER);
                _settings.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_SHAPE);
                _settings.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_ICON);
                LDtkEditorGUIUtility.DrawDivider();
                
                //EditorGUILayout.LabelField("Field Handles", style);
                _settings.DrawField(LDtkPrefs.PROP_SHOW_FIELD_IDENTIFIER);
                _settings.DrawField(LDtkPrefs.PROP_SHOW_FIELD_SHAPE);
                LDtkEditorGUIUtility.DrawDivider();
            }
            
            _settings.ApplyModifiedProperties();
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            if (!LDtkInternalUtility.Exists(PATH))
            {
                return null;
            }
            
            return new LDtkPrefsProvider(PREFS_PATH, SettingsScope.User)
            {
                // Automatically extract all keywords from the Styles.
                //keywords = GetSearchKeywordsFromGUIContentProperties<LDtkStyles>()
            };
        }
    }
}