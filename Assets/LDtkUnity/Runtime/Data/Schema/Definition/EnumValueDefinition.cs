using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class EnumValueDefinition
    {
        /// <summary>
        /// An array of 4 Int values that refers to the tile in the tileset image: `[ x, y, width,
        /// height ]`
        /// </summary>
        [JsonProperty("__tileSrcRect")]
        public long[] TileSrcRect { get; set; }

        /// <summary>
        /// Optional color
        /// </summary>
        [JsonProperty("color")]
        public long Color { get; set; }

        /// <summary>
        /// Enum value
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The optional ID of the tile
        /// </summary>
        [JsonProperty("tileId")]
        public long? TileId { get; set; }
    }
}