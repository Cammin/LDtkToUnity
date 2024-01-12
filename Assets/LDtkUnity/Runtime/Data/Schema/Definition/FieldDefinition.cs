using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// This section is mostly only intended for the LDtk editor app itself. You can safely
    /// ignore it.
    /// </summary>
    public partial class FieldDefinition
    {
        /// <summary>
        /// Human readable value type. Possible values: `Int, Float, String, Bool, Color,
        /// ExternEnum.XXX, LocalEnum.XXX, Point, FilePath`.<br/>
        /// </summary>
        [DataMember(Name = "__type")]
        public string Type { get; set; }

        /// <summary>
        /// Optional list of accepted file extensions for FilePath value type. Includes the dot:
        /// `.ext`
        /// </summary>
        [DataMember(Name = "acceptFileTypes")]
        public string[] AcceptFileTypes { get; set; }

        /// <summary>
        /// Possible values: `Any`, `OnlySame`, `OnlyTags`, `OnlySpecificEntity`
        /// </summary>
        [DataMember(Name = "allowedRefs")]
        public AllowedRefs AllowedRefs { get; set; }

        [DataMember(Name = "allowedRefsEntityUid")]
        public int? AllowedRefsEntityUid { get; set; }

        [DataMember(Name = "allowedRefTags")]
        public string[] AllowedRefTags { get; set; }

        [DataMember(Name = "allowOutOfLevelRef")]
        public bool AllowOutOfLevelRef { get; set; }

        /// <summary>
        /// Array max length
        /// </summary>
        [DataMember(Name = "arrayMaxLength")]
        public int? ArrayMaxLength { get; set; }

        /// <summary>
        /// Array min length
        /// </summary>
        [DataMember(Name = "arrayMinLength")]
        public int? ArrayMinLength { get; set; }

        [DataMember(Name = "autoChainRef")]
        public bool AutoChainRef { get; set; }

        /// <summary>
        /// TRUE if the value can be null. For arrays, TRUE means it can contain null values
        /// (exception: array of Points can't have null values).
        /// </summary>
        [DataMember(Name = "canBeNull")]
        public bool CanBeNull { get; set; }

        /// <summary>
        /// Default value if selected value is null or invalid.
        /// </summary>
        [DataMember(Name = "defaultOverride")]
        public object DefaultOverride { get; set; }

        /// <summary>
        /// User defined documentation for this field to provide help/tips to level designers about
        /// accepted values.
        /// </summary>
        [DataMember(Name = "doc")]
        public string Doc { get; set; }

        [DataMember(Name = "editorAlwaysShow")]
        public bool EditorAlwaysShow { get; set; }

        [DataMember(Name = "editorCutLongValues")]
        public bool EditorCutLongValues { get; set; }

        [DataMember(Name = "editorDisplayColor")]
        public string EditorDisplayColor { get; set; }

        /// <summary>
        /// Possible values: `Hidden`, `ValueOnly`, `NameAndValue`, `EntityTile`, `LevelTile`,
        /// `Points`, `PointStar`, `PointPath`, `PointPathLoop`, `RadiusPx`, `RadiusGrid`,
        /// `ArrayCountWithLabel`, `ArrayCountNoLabel`, `RefLinkBetweenPivots`,
        /// `RefLinkBetweenCenters`
        /// </summary>
        [DataMember(Name = "editorDisplayMode")]
        public EditorDisplayMode EditorDisplayMode { get; set; }

        /// <summary>
        /// Possible values: `Above`, `Center`, `Beneath`
        /// </summary>
        [DataMember(Name = "editorDisplayPos")]
        public EditorDisplayPos EditorDisplayPos { get; set; }

        [DataMember(Name = "editorDisplayScale")]
        public float EditorDisplayScale { get; set; }

        /// <summary>
        /// Possible values: `ZigZag`, `StraightArrow`, `CurvedArrow`, `ArrowsLine`, `DashedLine`
        /// </summary>
        [DataMember(Name = "editorLinkStyle")]
        public EditorLinkStyle EditorLinkStyle { get; set; }

        [DataMember(Name = "editorShowInWorld")]
        public bool EditorShowInWorld { get; set; }

        [DataMember(Name = "editorTextPrefix")]
        public string EditorTextPrefix { get; set; }

        [DataMember(Name = "editorTextSuffix")]
        public string EditorTextSuffix { get; set; }

        /// <summary>
        /// If TRUE, the field value will be exported to the `toc` project JSON field. Only applies
        /// to Entity fields.
        /// </summary>
        [DataMember(Name = "exportToToc")]
        public bool ExportToToc { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// TRUE if the value is an array of multiple values
        /// </summary>
        [DataMember(Name = "isArray")]
        public bool IsArray { get; set; }

        /// <summary>
        /// Max limit for value, if applicable
        /// </summary>
        [DataMember(Name = "max")]
        public float? Max { get; set; }

        /// <summary>
        /// Min limit for value, if applicable
        /// </summary>
        [DataMember(Name = "min")]
        public float? Min { get; set; }

        /// <summary>
        /// Optional regular expression that needs to be matched to accept values. Expected format:
        /// `/some_reg_ex/g`, with optional "i" flag.
        /// </summary>
        [DataMember(Name = "regex")]
        public string Regex { get; set; }

        /// <summary>
        /// If enabled, this field will be searchable through LDtk command palette
        /// </summary>
        [DataMember(Name = "searchable")]
        public bool Searchable { get; set; }

        [DataMember(Name = "symmetricalRef")]
        public bool SymmetricalRef { get; set; }

        /// <summary>
        /// Possible values: &lt;`null`&gt;, `LangPython`, `LangRuby`, `LangJS`, `LangLua`, `LangC`,
        /// `LangHaxe`, `LangMarkdown`, `LangJson`, `LangXml`, `LangLog`
        /// </summary>
        [DataMember(Name = "textLanguageMode")]
        public TextLanguageMode? TextLanguageMode { get; set; }

        /// <summary>
        /// UID of the tileset used for a Tile
        /// </summary>
        [DataMember(Name = "tilesetUid")]
        public int? TilesetUid { get; set; }

        /// <summary>
        /// Internal enum representing the possible field types. Possible values: F_Int, F_Float,
        /// F_String, F_Text, F_Bool, F_Color, F_Enum(...), F_Point, F_Path, F_EntityRef, F_Tile
        /// </summary>
        [DataMember(Name = "type")]
        public string FieldDefinitionType { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [DataMember(Name = "uid")]
        public int Uid { get; set; }

        /// <summary>
        /// If TRUE, the color associated with this field will override the Entity or Level default
        /// color in the editor UI. For Enum fields, this would be the color associated to their
        /// values.
        /// </summary>
        [DataMember(Name = "useForSmartColor")]
        public bool UseForSmartColor { get; set; }
    }
}