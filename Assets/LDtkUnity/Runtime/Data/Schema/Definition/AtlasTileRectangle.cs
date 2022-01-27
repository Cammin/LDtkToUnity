using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// An array of 4 integers (`[x,y,width,height]` in pixels) representing a custom rectangle
    /// from a Tileset image.
    /// </summary>
    public partial class AtlasTileRectangle
    {
        [JsonProperty("h")]
        public long H { get; set; }

        [JsonProperty("w")]
        public long W { get; set; }

        /// <summary>
        /// X pixel coord of the tile in the Tileset atlas
        /// </summary>
        [JsonProperty("x")]
        public long X { get; set; }

        /// <summary>
        /// Y pixel coord of the tile in the Tileset atlas
        /// </summary>
        [JsonProperty("y")]
        public long Y { get; set; }
    }
}