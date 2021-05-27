using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class FieldDefinition
    {
        /// <summary>
        /// Human readable value type (eg. `Int`, `Float`, `Point`, etc.). If the field is an array,
        /// this field will look like `Array<...>` (eg. `Array<Int>`, `Array<Point>` etc.)
        /// </summary>
        [JsonProperty("__type")]
        public string Type { get; set; }

        /// <summary>
        /// Optional list of accepted file extensions for FilePath value type. Includes the dot:
        /// `.ext`
        /// </summary>
        [JsonProperty("acceptFileTypes")]
        public string[] AcceptFileTypes { get; set; }

        /// <summary>
        /// Array max length
        /// </summary>
        [JsonProperty("arrayMaxLength")]
        public long? ArrayMaxLength { get; set; }

        /// <summary>
        /// Array min length
        /// </summary>
        [JsonProperty("arrayMinLength")]
        public long? ArrayMinLength { get; set; }

        /// <summary>
        /// TRUE if the value can be null. For arrays, TRUE means it can contain null values
        /// (exception: array of Points can't have null values).
        /// </summary>
        [JsonProperty("canBeNull")]
        public bool CanBeNull { get; set; }

        /// <summary>
        /// Default value if selected value is null or invalid.
        /// </summary>
        [JsonProperty("defaultOverride")]
        public object DefaultOverride { get; set; }

        [JsonProperty("editorAlwaysShow")]
        public bool EditorAlwaysShow { get; set; }

        [JsonProperty("editorCutLongValues")]
        public bool EditorCutLongValues { get; set; }

        /// <summary>
        /// Possible values: `Hidden`, `ValueOnly`, `NameAndValue`, `EntityTile`, `Points`,
        /// `PointStar`, `PointPath`, `PointPathLoop`, `RadiusPx`, `RadiusGrid`
        /// </summary>
        [JsonProperty("editorDisplayMode")]
        public EditorDisplayMode EditorDisplayMode { get; set; }

        /// <summary>
        /// Possible values: `Above`, `Center`, `Beneath`
        /// </summary>
        [JsonProperty("editorDisplayPos")]
        public EditorDisplayPos EditorDisplayPos { get; set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// TRUE if the value is an array of multiple values
        /// </summary>
        [JsonProperty("isArray")]
        public bool IsArray { get; set; }

        /// <summary>
        /// Max limit for value, if applicable
        /// </summary>
        [JsonProperty("max")]
        public double? Max { get; set; }

        /// <summary>
        /// Min limit for value, if applicable
        /// </summary>
        [JsonProperty("min")]
        public double? Min { get; set; }

        /// <summary>
        /// Optional regular expression that needs to be matched to accept values. Expected format:
        /// `/some_reg_ex/g`, with optional "i" flag.
        /// </summary>
        [JsonProperty("regex")]
        public string Regex { get; set; }

        /// <summary>
        /// Possible values: &lt;`null`&gt;, `LangPython`, `LangRuby`, `LangJS`, `LangLua`, `LangC`,
        /// `LangHaxe`, `LangMarkdown`, `LangJson`, `LangXml`
        /// </summary>
        [JsonProperty("textLangageMode")]
        public TextLangageMode? TextLangageMode { get; set; }

        /// <summary>
        /// Internal type enum
        /// </summary>
        [JsonProperty("type")]
        public object FieldDefinitionType { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty("uid")]
        public long Uid { get; set; }
    }
}