using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkProjectSettingsProvider : SettingsProvider
    {
        public const string PROVIDER_PATH = "Project/LDtk To Unity";
        private const string SETTINGS_PATH = "ProjectSettings/LDtkProjectSettings.asset";
 
        //cached so that we don't call the deserializer as much
        private static LDtkProjectSettings _instance;
        private static SerializedObject _serializedObject;

        private LDtkProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }
        
        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new LDtkProjectSettingsProvider(PROVIDER_PATH, SettingsScope.Project);
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
        
        public static LDtkProjectSettings Instance
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
            
            InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { _instance }, SETTINGS_PATH, true);
        }
        private static void LoadFromJson()
        {
            Object[] objs = InternalEditorUtility.LoadSerializedFileAndForget(SETTINGS_PATH);
            if (objs.Length == 0)
            {
                _instance = ScriptableObject.CreateInstance<LDtkProjectSettings>();
                return;
            }
            _instance = (LDtkProjectSettings)objs[0];
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

            new LDtkProjectSettingsGUI(_serializedObject, SaveAsJson).OnGUI(searchContext);
        }
    }
}