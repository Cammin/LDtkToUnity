using System.Text.Json.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// This object represents a custom sub rectangle in a Tileset image.
    /// </summary>
    public partial class TilesetRectangle
    {
        /// <summary>
        /// Height in pixels
        /// </summary>
        [JsonPropertyName("h")]
        public long H { get; set; }

        /// <summary>
        /// UID of the tileset
        /// </summary>
        [JsonPropertyName("tilesetUid")]
        public long TilesetUid { get; set; }

        /// <summary>
        /// Width in pixels
        /// </summary>
        [JsonPropertyName("w")]
        public long W { get; set; }

        /// <summary>
        /// X pixels coordinate of the top-left corner in the Tileset image
        /// </summary>
        [JsonPropertyName("x")]
        public long X { get; set; }

        /// <summary>
        /// Y pixels coordinate of the top-left corner in the Tileset image
        /// </summary>
        [JsonPropertyName("y")]
        public long Y { get; set; }
    }
}