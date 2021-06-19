using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LDtkUnity.Editor
{
    public class LDtkPrefsProvider : SettingsProvider
    {
        private const string PREFS_PATH = "Preferences/LDtk To Unity"; 
 
        //cached so that we don't call the deserializer as much
        private static LDtkPrefs _instance;
        private static SerializedObject _serializedObject;

        private LDtkPrefsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }
        
        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new LDtkPrefsProvider(PREFS_PATH, SettingsScope.User);
        }
        
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            CreateSerializedObject();
            keywords = GetSearchKeywordsFromSerializedObject(_serializedObject);
        }

        public override void OnDeactivate()
        {
            SaveAsJson();
        }
        
        public static LDtkPrefs Instance
        {
            get
            {
                if (!_instance)
                {
                    LoadFromJson();
                }
                
                return _instance;
            }
        }

        private static void SaveAsJson()
        {
            if (!Instance)
            {
                Debug.LogError("LDtk: Tried saving prefs but the instance was null");
                return;
            }
            
            string json = JsonUtility.ToJson(_instance, true);
            EditorPrefs.SetString(PREFS_PATH, json);
            
        }
        private static void LoadFromJson()
        {
            CreateFreshInstance();
            string json = EditorPrefs.GetString(PREFS_PATH);
            JsonUtility.FromJsonOverwrite(json, _instance);
        }

        private static void CreateFreshInstance()
        {
            _instance = ScriptableObject.CreateInstance<LDtkPrefs>();
        }

        private static void CreateSerializedObject()
        {
            _serializedObject = new SerializedObject(Instance);
        }

        public override void OnGUI(string searchContext)
        {
            DrawResetButton();
            _serializedObject.Update();
            
            EditorGUIUtility.labelWidth = 200;

            using (new LDtkIndentScope())
            {

                //GUIStyle style = EditorStyles.miniBoldLabel;                
                
                _serializedObject.DrawField(LDtkPrefs.PROP_LOG_BUILD_TIMES);
                
                LDtkEditorGUIUtility.DrawDivider();
                
                //EditorGUILayout.LabelField("Level Handles", style);
                _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_LEVEL_IDENTIFIER);
                SerializedProperty levelBorderProp = _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_LEVEL_BORDER);
                if (levelBorderProp.boolValue)
                {
                    _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_LEVEL_BORDER_THICKNESS);
                }
                
                LDtkEditorGUIUtility.DrawDivider();

                //EditorGUILayout.LabelField("Entity Handles", style);
                _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_IDENTIFIER);
                _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_SHAPE);
                _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_ICON);
                
                LDtkEditorGUIUtility.DrawDivider();
                
                //EditorGUILayout.LabelField("Field Handles", style);
                _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_FIELD_IDENTIFIER);
                _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_FIELD_SHAPE);
                LDtkEditorGUIUtility.DrawDivider();
            }
            
            _serializedObject.ApplyModifiedProperties();
        }

        private static void DrawResetButton()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();


            Texture unityIcon = LDtkIconUtility.GetUnityIcon("Refresh", "");

            GUIContent content = new GUIContent
            {
                //text = "Reset",
                tooltip = "Reset to defaults",
                image = unityIcon
            };
                
            if (GUILayout.Button(content, GUILayout.Width(30)))
            {
                CreateFreshInstance();
                CreateSerializedObject();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}