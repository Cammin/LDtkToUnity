using Newtonsoft.Json;

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
        [JsonProperty("coordId")]
        public long CoordId { get; set; }

        /// <summary>
        /// IntGrid value
        /// </summary>
        [JsonProperty("v")]
        public long V { get; set; }
    }
}