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
        /// Actual value of the field instance. The value type may vary, depending on `__type`
        /// (Integer, Boolean, String etc.)<br/>  It can also be an `Array` of those same types.
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