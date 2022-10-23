using System.Text.Json.Serialization;

namespace LDtkUnity
{
    public partial class EnumValueDefinition
    {
        /// <summary>
        /// An array of 4 Int values that refers to the tile in the tileset image: `[ x, y, width,
        /// height ]`
        /// </summary>
        [JsonPropertyName("__tileSrcRect")]
        public long[] TileSrcRect { get; set; }

        /// <summary>
        /// Optional color
        /// </summary>
        [JsonPropertyName("color")]
        public long Color { get; set; }

        /// <summary>
        /// Enum value
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The optional ID of the tile
        /// </summary>
        [JsonPropertyName("tileId")]
        public long? TileId { get; set; }
    }
}