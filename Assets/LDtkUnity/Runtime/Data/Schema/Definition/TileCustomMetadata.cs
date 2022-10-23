using System.Text.Json.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// In a tileset definition, user defined meta-data of a tile.
    /// </summary>
    public partial class TileCustomMetadata
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("tileId")]
        public long TileId { get; set; }
    }
}