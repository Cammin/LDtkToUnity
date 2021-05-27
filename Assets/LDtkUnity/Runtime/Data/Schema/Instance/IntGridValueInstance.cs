using Newtonsoft.Json;

namespace LDtkUnity
{
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