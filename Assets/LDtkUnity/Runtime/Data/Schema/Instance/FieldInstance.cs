using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class FieldInstance
    {
        /// <summary>
        /// Field definition identifier
        /// </summary>
        [JsonProperty("__identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Type of the field, such as `Int`, `Float`, `Enum(my_enum_name)`, `Bool`, etc.
        /// </summary>
        [JsonProperty("__type")]
        public string Type { get; set; }

        /// <summary>
        /// Actual value of the field instance. The value type varies, depending on `__type`:<br/>
        /// - For **classic types** (ie. Integer, Float, Boolean, String, Text and FilePath), you
        /// just get the actual value with the expected type.<br/>   - For **Color**, the value is an
        /// hexadecimal string using "#rrggbb" format.<br/>   - For **Enum**, the value is a String
        /// representing the selected enum value.<br/>   - For **Point**, the value is an object `{
        /// cx : Int, cy : Int }` containing grid-based coordinates.<br/>   - For **Tile**, the value
        /// will be an `FieldInstanceTile` object (see below).<br/>   - For **EntityRef**, the value
        /// will be an `EntityReferenceInfos` object (see below).<br/><br/>  If the field is an
        /// array, then this `__value` will also be a JSON array.
        /// </summary>
        [JsonProperty("__value")]
        public object Value { get; set; }

        /// <summary>
        /// Reference of the **Field definition** UID
        /// </summary>
        [JsonProperty("defUid")]
        public long DefUid { get; set; }

        /// <summary>
        /// Editor internal raw values
        /// </summary>
        [JsonProperty("realEditorValues")]
        public object[] RealEditorValues { get; set; }
    }
}