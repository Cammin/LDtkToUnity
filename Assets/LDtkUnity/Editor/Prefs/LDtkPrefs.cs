using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkPrefs : ScriptableObject
    {
        private const float THICKNESS_MIN = 1;
        private const float THICKNESS_MAX = 10;
        public const float THICKNESS_DEFAULT = 1.5f;

        private const float DISTANCE_MIN = 0;
        public const float DISTANCE_MAX = 1000;
        private const float DISTANCE_DEFAULT = 150;
        
        public const string WRITE_PROFILED_IMPORTS = nameof(_writeProfiledImports); 
        public const string VERBOSE_LOGGING = nameof(_verboseLogging); 
        public const string DRAW_DISTANCE = nameof(_drawDistance);

        public const string PROPERTY_SHOW_LEVEL_IDENTIFIER = nameof(_showLevelIdentifier); 
        public const string PROPERTY_SHOW_LEVEL_BORDER = nameof(_showLevelBorder); 
        public const string PROPERTY_LEVEL_BORDER_THICKNESS = nameof(_levelBorderThickness);
        
        public const string PROPERTY_SHOW_ENTITY_IDENTIFIER = nameof(_showEntityIdentifier);
        public const string PROPERTY_SHOW_ENTITY_ICON = nameof(_showEntityIcon);
        
        public const string PROPERTY_SHOW_ENTITY_SHAPE = nameof(_showEntityShape);
        public const string PROPERTY_ENTITY_SHAPE_ONLY_HOLLOW = nameof(_entityOnlyHollow);
        public const string PROPERTY_ENTITY_SHAPE_ONLY_BORDERS = nameof(_entityOnlyBorders);
        public const string PROPERTY_ENTITY_SHAPE_THICKNESS = nameof(_entityShapeThickness);
        
        public const string PROPERTY_SHOW_FIELD_RADIUS = nameof(_showFieldRadius);
        public const string PROPERTY_FIELD_RADIUS_THICKNESS = nameof(_fieldRadiusThickness);
        
        public const string PROPERTY_SHOW_FIELD_POINTS = nameof(_showFieldPoints);
        public const string PROPERTY_FIELD_POINTS_THICKNESS = nameof(_fieldPointsThickness);
        
        public const string PROPERTY_SHOW_ENTITY_REF = nameof(_showFieldEntityRef);
        public const string PROPERTY_ENTITY_REF_THICKNESS = nameof(_fieldEntityRefThickness);

        //misc
        [SerializeField] private bool _writeProfiledImports = false;
        [SerializeField] private bool _verboseLogging = false;
        [Range(DISTANCE_MIN, DISTANCE_MAX)]
        [SerializeField] private float _drawDistance = DISTANCE_DEFAULT;
        
        //level
        [SerializeField] private bool _showLevelIdentifier = true;
        [SerializeField] private bool _showLevelBorder = true;
        [Range(THICKNESS_MIN, THICKNESS_MAX)]
        [SerializeField] private float _levelBorderThickness = THICKNESS_DEFAULT;
        
        //entity
        [SerializeField] private bool _showEntityIdentifier = true;
        [SerializeField] private bool _showEntityIcon = true;
        [SerializeField] private bool _showEntityShape = true;
        [Range(THICKNESS_MIN, THICKNESS_MAX)]
        [SerializeField] private float _entityShapeThickness = THICKNESS_DEFAULT;
        [SerializeField] private bool _entityOnlyBorders = false;
        [SerializeField] private bool _entityOnlyHollow = false;
        
        //floats/ints
        [SerializeField] private bool _showFieldRadius = true;
        [Range(THICKNESS_MIN, THICKNESS_MAX)]
        [SerializeField] private float _fieldRadiusThickness = THICKNESS_DEFAULT;
        
        //points
        [SerializeField] private bool _showFieldPoints = true;
        [Range(THICKNESS_MIN, THICKNESS_MAX)]
        [SerializeField] private float _fieldPointsThickness = THICKNESS_DEFAULT;

        //entityRef
        [SerializeField] private bool _showFieldEntityRef = true;
        [Range(THICKNESS_MIN, THICKNESS_MAX)]
        [SerializeField] private float _fieldEntityRefThickness = THICKNESS_DEFAULT;


        private static LDtkPrefs Instance => LDtkPrefsProvider.Instance; 
        
        public static bool WriteProfiledImports => Instance._writeProfiledImports;
        public static float DrawDistance => Instance._drawDistance;
        public static bool ShowLevelIdentifier => Instance._showLevelIdentifier;
        public static bool ShowLevelBorder => Instance._showLevelBorder;
        public static float LevelBorderThickness => Instance._levelBorderThickness;
        public static bool ShowEntityIdentifier => Instance._showEntityIdentifier;
        public static bool ShowEntityShape => Instance._showEntityShape;
        public static float EntityShapeThickness => Instance._entityShapeThickness;
        public static bool EntityOnlyBorders => Instance._entityOnlyBorders;
        public static bool EntityOnlyHollow => Instance._entityOnlyHollow;
        public static bool ShowEntityIcon => Instance._showEntityIcon;
        public static bool ShowFieldRadius => Instance._showFieldRadius;
        public static bool ShowFieldPoints => Instance._showFieldPoints;
        public static float FieldRadiusThickness => Instance._fieldRadiusThickness;
        public static float FieldPointsThickness => Instance._fieldPointsThickness;
        public static bool ShowFieldEntityRef => Instance._showFieldEntityRef;
        public static float FieldEntityRefThickness => Instance._fieldEntityRefThickness;
        public static bool VerboseLogging => Instance._verboseLogging;
    }
}