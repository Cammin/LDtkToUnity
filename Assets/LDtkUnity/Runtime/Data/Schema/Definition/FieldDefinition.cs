using System.Text.Json.Serialization;

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
        /// ExternEnum.XXX, LocalEnum.XXX, Point, FilePath`.<br/>  If the field is an array, this
        /// field will look like `Array<...>` (eg. `Array<Int>`, `Array<Point>` etc.)<br/>  NOTE: if
        /// you enable the advanced option **Use Multilines type**, you will have "*Multilines*"
        /// instead of "*String*" when relevant.
        /// </summary>
        [JsonPropertyName("__type")]
        public string Type { get; set; }

        /// <summary>
        /// Optional list of accepted file extensions for FilePath value type. Includes the dot:
        /// `.ext`
        /// </summary>
        [JsonPropertyName("acceptFileTypes")]
        public string[] AcceptFileTypes { get; set; }

        /// <summary>
        /// Possible values: `Any`, `OnlySame`, `OnlyTags`
        /// </summary>
        [JsonPropertyName("allowedRefs")]
        public AllowedRefs AllowedRefs { get; set; }

        [JsonPropertyName("allowedRefTags")]
        public string[] AllowedRefTags { get; set; }

        [JsonPropertyName("allowOutOfLevelRef")]
        public bool AllowOutOfLevelRef { get; set; }

        /// <summary>
        /// Array max length
        /// </summary>
        [JsonPropertyName("arrayMaxLength")]
        public long? ArrayMaxLength { get; set; }

        /// <summary>
        /// Array min length
        /// </summary>
        [JsonPropertyName("arrayMinLength")]
        public long? ArrayMinLength { get; set; }

        [JsonPropertyName("autoChainRef")]
        public bool AutoChainRef { get; set; }

        /// <summary>
        /// TRUE if the value can be null. For arrays, TRUE means it can contain null values
        /// (exception: array of Points can't have null values).
        /// </summary>
        [JsonPropertyName("canBeNull")]
        public bool CanBeNull { get; set; }

        /// <summary>
        /// Default value if selected value is null or invalid.
        /// </summary>
        [JsonPropertyName("defaultOverride")]
        public object DefaultOverride { get; set; }

        [JsonPropertyName("editorAlwaysShow")]
        public bool EditorAlwaysShow { get; set; }

        [JsonPropertyName("editorCutLongValues")]
        public bool EditorCutLongValues { get; set; }

        /// <summary>
        /// Possible values: `Hidden`, `ValueOnly`, `NameAndValue`, `EntityTile`, `Points`,
        /// `PointStar`, `PointPath`, `PointPathLoop`, `RadiusPx`, `RadiusGrid`,
        /// `ArrayCountWithLabel`, `ArrayCountNoLabel`, `RefLinkBetweenPivots`,
        /// `RefLinkBetweenCenters`
        /// </summary>
        [JsonPropertyName("editorDisplayMode")]
        public EditorDisplayMode EditorDisplayMode { get; set; }

        /// <summary>
        /// Possible values: `Above`, `Center`, `Beneath`
        /// </summary>
        [JsonPropertyName("editorDisplayPos")]
        public EditorDisplayPos EditorDisplayPos { get; set; }

        [JsonPropertyName("editorTextPrefix")]
        public string EditorTextPrefix { get; set; }

        [JsonPropertyName("editorTextSuffix")]
        public string EditorTextSuffix { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// TRUE if the value is an array of multiple values
        /// </summary>
        [JsonPropertyName("isArray")]
        public bool IsArray { get; set; }

        /// <summary>
        /// Max limit for value, if applicable
        /// </summary>
        [JsonPropertyName("max")]
        public double? Max { get; set; }

        /// <summary>
        /// Min limit for value, if applicable
        /// </summary>
        [JsonPropertyName("min")]
        public double? Min { get; set; }

        /// <summary>
        /// Optional regular expression that needs to be matched to accept values. Expected format:
        /// `/some_reg_ex/g`, with optional "i" flag.
        /// </summary>
        [JsonPropertyName("regex")]
        public string Regex { get; set; }

        [JsonPropertyName("symmetricalRef")]
        public bool SymmetricalRef { get; set; }

        /// <summary>
        /// Possible values: &lt;`null`&gt;, `LangPython`, `LangRuby`, `LangJS`, `LangLua`, `LangC`,
        /// `LangHaxe`, `LangMarkdown`, `LangJson`, `LangXml`, `LangLog`
        /// </summary>
        [JsonPropertyName("textLanguageMode")]
        public TextLanguageMode? TextLanguageMode { get; set; }

        /// <summary>
        /// UID of the tileset used for a Tile
        /// </summary>
        [JsonPropertyName("tilesetUid")]
        public long? TilesetUid { get; set; }

        /// <summary>
        /// Internal enum representing the possible field types. Possible values: F_Int, F_Float,
        /// F_String, F_Text, F_Bool, F_Color, F_Enum(...), F_Point, F_Path, F_EntityRef, F_Tile
        /// </summary>
        [JsonPropertyName("type")]
        public string FieldDefinitionType { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonPropertyName("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// If TRUE, the color associated with this field will override the Entity or Level default
        /// color in the editor UI. For Enum fields, this would be the color associated to their
        /// values.
        /// </summary>
        [JsonPropertyName("useForSmartColor")]
        public bool UseForSmartColor { get; set; }
    }
}