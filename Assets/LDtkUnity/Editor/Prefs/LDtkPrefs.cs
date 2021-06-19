using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkPrefs : ScriptableObject
    {
        public const string PROP_LOG_BUILD_TIMES = nameof(_logBuildTimes);
        
        public const string PROP_SHOW_LEVEL_IDENTIFIER = nameof(_showLevelIdentifier); 
        public const string PROP_SHOW_LEVEL_BORDER = nameof(_showLevelBorder); 
        public const string PROP_SHOW_LEVEL_BORDER_THICKNESS = nameof(_showLevelBorderThickness);
        
        public const string PROP_SHOW_ENTITY_IDENTIFIER = nameof(_showEntityIdentifier);
        public const string PROP_SHOW_ENTITY_SHAPE = nameof(_showEntityShape);
        public const string PROP_SHOW_ENTITY_ICON = nameof(_showEntityIcon);
        
        public const string PROP_SHOW_FIELD_IDENTIFIER = nameof(_showFieldIdentifier); 
        public const string PROP_SHOW_FIELD_SHAPE = nameof(_showFieldShape);
        
        [SerializeField] private bool _logBuildTimes = false;
        
        [SerializeField] private bool _showLevelIdentifier = true;
        [SerializeField] private bool _showLevelBorder = true;
        [SerializeField, Range(1, 20)] private float _showLevelBorderThickness = 1.5f;
        
        [SerializeField] private bool _showEntityIdentifier = true;
        [SerializeField] private bool _showEntityShape = true;
        [SerializeField] private bool _showEntityIcon = true;
        
        [SerializeField] private bool _showFieldIdentifier = true;
        [SerializeField] private bool _showFieldShape = true;

        private static LDtkPrefs Instance => LDtkPrefsProvider.Instance; 
            
        public static bool LogBuildTimes => Instance._logBuildTimes;
        public static bool ShowLevelIdentifier => Instance._showLevelIdentifier;
        public static bool ShowLevelBorder => Instance._showLevelBorder;
        public static float ShowLevelBorderThickness => Instance._showLevelBorderThickness;
        public static bool ShowEntityIdentifier => Instance._showEntityIdentifier;
        public static bool ShowEntityShape => Instance._showEntityShape;
        public static bool ShowEntityIcon => Instance._showEntityIcon;
        public static bool ShowFieldIdentifier => Instance._showFieldIdentifier;
        public static bool ShowFieldShape => Instance._showFieldShape;
    }
}