using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// IntGrid value definition
    /// </summary>
    public partial class IntGridValueDefinition
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// The IntGrid value itself
        /// </summary>
        [JsonProperty("value")]
        public long Value { get; set; }
    }
}