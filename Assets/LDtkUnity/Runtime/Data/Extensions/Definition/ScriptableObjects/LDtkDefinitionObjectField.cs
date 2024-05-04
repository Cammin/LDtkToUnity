using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_FIELD_DEF_JSON)]
    public sealed class LDtkDefinitionObjectField : LDtkDefinitionObject<FieldDefinition>, ILDtkUid
    {
        [field: Header("Internal")]
        [field: Tooltip("Human readable value type. Possible values: `Int, Float, String, Bool, Color, ExternEnum.XXX, LocalEnum.XXX, Point, FilePath`.")]
        [field: SerializeField] public string Type { get; private set; }
        
        [field: Tooltip("Optional list of accepted file extensions for FilePath value type. Includes the dot: `.ext`")]
        [field: SerializeField] public string[] AcceptFileTypes { get; private set; }
        
        [field: Tooltip("Possible values: `Any`, `OnlySame`, `OnlyTags`, `OnlySpecificEntity`")]
        [field: SerializeField] public AllowedRefs AllowedRefs { get; private set; }

        [field: SerializeField] public LDtkDefinitionObjectEntity AllowedRefsEntity { get; private set; }

        [field: SerializeField] public string[] AllowedRefTags { get; private set; }

        [field: SerializeField] public bool AllowOutOfLevelRef { get; private set; }

        [field: Tooltip("Array max length")]
        [field: SerializeField] public int ArrayMaxLength { get; private set; }
        
        [field: Tooltip("Array min length")]
        [field: SerializeField] public int ArrayMinLength { get; private set; }

        [field: SerializeField] public bool AutoChainRef { get; private set; }
        
        [field: Tooltip("TRUE if the value can be null. For arrays, TRUE means it can contain null values (exception: array of Points can't have null values).")]
        [field: SerializeField] public bool CanBeNull { get; private set; }
        
        [field: Tooltip("Default value if selected value is null or invalid.")]
        [field: SerializeField] public object DefaultOverride { get; private set; }
        
        [field: Tooltip("User defined documentation for this field to provide help/tips to level designers about accepted values.")]
        [field: SerializeField] public string Doc { get; private set; }

        [field: SerializeField] public bool EditorAlwaysShow { get; private set; }

        [field: SerializeField] public bool EditorCutLongValues { get; private set; }

        [field: SerializeField] public string EditorDisplayColor { get; private set; }
        
        [field: Tooltip("Possible values: `Hidden`, `ValueOnly`, `NameAndValue`, `EntityTile`, `LevelTile`, `Points`, `PointStar`, `PointPath`, `PointPathLoop`, `RadiusPx`, `RadiusGrid`, `ArrayCountWithLabel`, `ArrayCountNoLabel`, `RefLinkBetweenPivots`, `RefLinkBetweenCenters`")]
        [field: SerializeField] public EditorDisplayMode EditorDisplayMode { get; private set; }
        
        [field: Tooltip("Possible values: `Above`, `Center`, `Beneath`")]
        [field: SerializeField] public EditorDisplayPos EditorDisplayPos { get; private set; }

        [field: SerializeField] public float EditorDisplayScale { get; private set; }
        
        [field: Tooltip("Possible values: `ZigZag`, `StraightArrow`, `CurvedArrow`, `ArrowsLine`, `DashedLine`")]
        [field: SerializeField] public EditorLinkStyle EditorLinkStyle { get; private set; }

        [field: SerializeField] public bool EditorShowInWorld { get; private set; }

        [field: SerializeField] public string EditorTextPrefix { get; private set; }

        [field: SerializeField] public string EditorTextSuffix { get; private set; }
        
        [field: Tooltip("If TRUE, the field value will be exported to the `toc` project JSON field. Only applies to Entity fields.")]
        [field: SerializeField] public bool ExportToToc { get; private set; }
        
        [field: Tooltip("User defined unique identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: Tooltip("TRUE if the value is an array of multiple values")]
        [field: SerializeField] public bool IsArray { get; private set; }
        
        [field: Tooltip("Max limit for value, if applicable")]
        [field: SerializeField] public float Max { get; private set; }
        
        [field: Tooltip("Min limit for value, if applicable")]
        [field: SerializeField] public float Min { get; private set; }
        
        [field: Tooltip("Optional regular expression that needs to be matched to accept values. Expected format: `/some_reg_ex/g`, with optional \"i\" flag.")]
        [field: SerializeField] public string Regex { get; private set; }
        
        [field: Tooltip("If enabled, this field will be searchable through LDtk command palette")]
        [field: SerializeField] public bool Searchable { get; private set; }

        [field: SerializeField] public bool SymmetricalRef { get; private set; }
        
        [field: Tooltip("Possible values: &lt;`null`&gt;, `LangPython`, `LangRuby`, `LangJS`, `LangLua`, `LangC`, `LangHaxe`, `LangMarkdown`, `LangJson`, `LangXml`, `LangLog`")]
        [field: SerializeField] public TextLanguageMode TextLanguageMode { get; private set; }
        
        [field: Tooltip("UID of the tileset used for a Tile")]
        [field: SerializeField] public LDtkDefinitionObjectTileset Tileset { get; private set; }
        
        [field: Tooltip("Internal enum representing the possible field types. Possible values: F_Int, F_Float, F_String, F_Text, F_Bool, F_Color, F_Enum(...), F_Point, F_Path, F_EntityRef, F_Tile")]
        [field: SerializeField] public string FieldDefinitionType { get; private set; }
        
        [field: Tooltip("Unique Int identifier")]
        [field: SerializeField] public int Uid { get; private set; }
        
        [field: Tooltip("If TRUE, the color associated with this field will override the Entity or Level default color in the editor UI. For Enum fields, this would be the color associated to their values.")]
        [field: SerializeField] public bool UseForSmartColor { get; private set; }
        
        internal override void SetAssetName()
        {
            name = $"Field_{Uid}_{Identifier}";
        }
        
        internal override void Populate(LDtkDefinitionObjectsCache cache, FieldDefinition def)
        {
            Type = def.Type;
            AcceptFileTypes = def.AcceptFileTypes;
            AllowedRefs = def.AllowedRefs;
            AllowedRefsEntity = cache.GetObject<LDtkDefinitionObjectEntity>(def.AllowedRefsEntityUid);
            AllowedRefTags = def.AllowedRefTags;
            AllowOutOfLevelRef = def.AllowOutOfLevelRef;
            ArrayMaxLength = def.ArrayMaxLength != null ? def.ArrayMaxLength.Value : 0;
            ArrayMinLength = def.ArrayMinLength != null ? def.ArrayMinLength.Value : 0;
            AutoChainRef = def.AutoChainRef;
            CanBeNull = def.CanBeNull;
            DefaultOverride = def.DefaultOverride;
            Doc = def.Doc;
            EditorAlwaysShow = def.EditorAlwaysShow;
            EditorCutLongValues = def.EditorCutLongValues;
            EditorDisplayColor = def.EditorDisplayColor;
            EditorDisplayMode = def.EditorDisplayMode;
            EditorDisplayPos = def.EditorDisplayPos;
            EditorDisplayScale = def.EditorDisplayScale;
            EditorLinkStyle = def.EditorLinkStyle;
            EditorShowInWorld = def.EditorShowInWorld;
            EditorTextPrefix = def.EditorTextPrefix;
            EditorTextSuffix = def.EditorTextSuffix;
            ExportToToc = def.ExportToToc;
            Identifier = def.Identifier;
            IsArray = def.IsArray;
            Max = def.Max ?? float.NaN;
            Min = def.Min ?? float.NaN;
            Regex = def.Regex;
            Searchable = def.Searchable;
            SymmetricalRef = def.SymmetricalRef;
            TextLanguageMode = def.TextLanguageMode != null ? def.TextLanguageMode.Value : default;
            Tileset = cache.GetObject<LDtkDefinitionObjectTileset>(def.TilesetUid);
            FieldDefinitionType = def.FieldDefinitionType;
            Uid = def.Uid;
            UseForSmartColor = def.UseForSmartColor;
        }
    }
}