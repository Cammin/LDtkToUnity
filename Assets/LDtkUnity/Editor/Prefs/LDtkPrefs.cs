using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkPrefs : ScriptableObject
    {
        //Misc
        public const string PROP_LOG_BUILD_TIMES = nameof(_logBuildTimes);
        [SerializeField] private bool _logBuildTimes = false;
        public static bool LogBuildTimes => Instance._logBuildTimes;
        
        
        //Level
        public const string PROP_SHOW_LEVEL_IDENTIFIER = nameof(_showLevelIdentifier); 
        public const string PROP_SHOW_LEVEL_BORDER = nameof(_showLevelBorder); 
        public const string PROP_SHOW_LEVEL_BORDER_THICKNESS = nameof(_showLevelBorderThickness);
        
        [SerializeField] private bool _showLevelIdentifier = true;
        [SerializeField] private bool _showLevelBorder = true;
        [SerializeField, Range(1, 20)] private float _showLevelBorderThickness = 1.5f;
        
        public static bool ShowLevelIdentifier => Instance._showLevelIdentifier;
        public static bool ShowLevelBorder => Instance._showLevelBorder;
        public static float ShowLevelBorderThickness => Instance._showLevelBorderThickness;
        
        
        
        //Entity
        public const string PROP_SHOW_ENTITY_IDENTIFIER = nameof(_showEntityIdentifier);
        public const string PROP_SHOW_ENTITY_SHAPE = nameof(_showEntityShape);
        public const string PROP_SHOW_ENTITY_ICON = nameof(_showEntityIcon);
        
        [SerializeField] private bool _showEntityIdentifier = true;
        [SerializeField] private bool _showEntityShape = true;
        [SerializeField] private bool _showEntityIcon = true;
        
        public static bool ShowEntityIdentifier => Instance._showEntityIdentifier;
        public static bool ShowEntityShape => Instance._showEntityShape;
        public static bool ShowEntityIcon => Instance._showEntityIcon;
        
        
        //Field
        public const string PROP_SHOW_FIELD_IDENTIFIER = nameof(_showFieldIdentifier); 
        public const string PROP_SHOW_FIELD_SHAPE = nameof(_showFieldShape);
        
        [SerializeField] private bool _showFieldIdentifier = true;
        [SerializeField] private bool _showFieldShape = true;
        
        public static bool ShowFieldIdentifier => Instance._showFieldIdentifier;
        public static bool ShowFieldShape => Instance._showFieldShape;
        
        
        
        //cached so that we don't call the loader as much
        private static LDtkPrefs _instance;
        private static LDtkPrefs Instance
        {
            get
            {
                if (_instance)
                {
                    return _instance;
                }
                
                _instance = LDtkInternalUtility.Load<LDtkPrefs>(LDtkPrefsProvider.PATH);
                if (_instance)
                {
                    return _instance;
                }

                Debug.LogError("LDtk: Could not load preferences asset");
                return null;
            }
        }
        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(Instance);
        }
    }
}