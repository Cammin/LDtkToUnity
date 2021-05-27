using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class EntityInstanceTile
    {
        /// <summary>
        /// An array of 4 Int values that refers to the tile in the tileset image: `[ x, y, width,
        /// height ]`
        /// </summary>
        [JsonProperty("srcRect")]
        public long[] SrcRect { get; set; }

        /// <summary>
        /// Tileset ID
        /// </summary>
        [JsonProperty("tilesetUid")]
        public long TilesetUid { get; set; }
    }
}