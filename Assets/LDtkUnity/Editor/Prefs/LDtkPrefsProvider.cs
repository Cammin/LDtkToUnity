using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkPrefsProvider : SettingsProvider
    {
        public const string PROVIDER_PATH = "Preferences/LDtk To Unity"; 
 
        //cached so that we don't call the deserializer as much
        private static LDtkPrefs _instance;
        private static SerializedObject _serializedObject;

        private LDtkPrefsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }
        
        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new LDtkPrefsProvider(PROVIDER_PATH, SettingsScope.User);
        }
        
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            UpdateSerializedObject();
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
                LDtkDebug.LogError("Tried saving prefs but the instance was null");
                return;
            }
            
            string json = JsonUtility.ToJson(_instance, true);
            EditorPrefs.SetString(PROVIDER_PATH, json);
            
        }
        private static void LoadFromJson()
        {
            CreateFreshInstance();
            string json = EditorPrefs.GetString(PROVIDER_PATH);
            JsonUtility.FromJsonOverwrite(json, _instance);
        }

        private static void CreateFreshInstance()
        {
            _instance = ScriptableObject.CreateInstance<LDtkPrefs>();
        }

        private static void UpdateSerializedObject()
        {
            _serializedObject = new SerializedObject(Instance);
        }

        public override void OnGUI(string searchContext)
        {
            if (_serializedObject.targetObject == null)
            {
                UpdateSerializedObject();
            }

            new LDtkPrefsGUI(_serializedObject, ResetAction, SaveAsJson).OnGUI(searchContext);
        }

        private void ResetAction()
        {
            CreateFreshInstance();
            UpdateSerializedObject();
            SaveAsJson();
        }
    }
}