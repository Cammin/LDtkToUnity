using Newtonsoft.Json;

namespace LDtkUnity.Data
{
    public class FieldInstance
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("__identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Type of the field, such as Int, Float, Enum(enum_name), Bool, etc.
        /// </summary>
        [JsonProperty("__type")]
        public string Type { get; set; }

        /// <summary>
        /// Actual value of the field instance. The value type may vary, depending on `__type`
        /// (Integer, Boolean, String etc.)<br/>  It can also be an `Array` of those same types.
        /// </summary>
        [JsonProperty("__value")]
        public dynamic Value { get; set; }

        /// <summary>
        /// Reference of the **Field definition** UID
        /// </summary>
        [JsonProperty("defUid")]
        public long DefUid { get; set; }

        [JsonProperty("realEditorValues")]
        public dynamic[] RealEditorValues { get; set; }
    }
}