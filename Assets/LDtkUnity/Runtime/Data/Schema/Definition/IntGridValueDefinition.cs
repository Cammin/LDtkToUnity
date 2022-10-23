using System.Text.Json.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// IntGrid value definition
    /// </summary>
    public partial class IntGridValueDefinition
    {
        [JsonPropertyName("color")]
        public string Color { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// The IntGrid value itself
        /// </summary>
        [JsonPropertyName("value")]
        public long Value { get; set; }
    }
}