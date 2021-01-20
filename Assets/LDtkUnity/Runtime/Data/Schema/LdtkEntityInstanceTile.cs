using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// Optional Tile used to display this entity (it could either be the default Entity tile, or
    /// some tile provided by a field value, like an Enum).
    ///
    /// Description of a tile used by an EntityInstance
    /// </summary>
    public partial class LdtkEntityInstanceTile
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