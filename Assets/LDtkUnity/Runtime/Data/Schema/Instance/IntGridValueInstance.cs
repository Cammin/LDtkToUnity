using System.Text.Json.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// IntGrid value instance
    /// </summary>
    public partial class IntGridValueInstance
    {
        /// <summary>
        /// Coordinate ID in the layer grid
        /// </summary>
        [JsonPropertyName("coordId")]
        public long CoordId { get; set; }

        /// <summary>
        /// IntGrid value
        /// </summary>
        [JsonPropertyName("v")]
        public long V { get; set; }
    }
}