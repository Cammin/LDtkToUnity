using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// In a tileset definition, user defined meta-data of a tile.
    /// </summary>
    public partial class LdtkTileCustomMetadata
    {
        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("tileId")]
        public long TileId { get; set; }
    }
}