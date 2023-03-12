using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// In a tileset definition, user defined meta-data of a tile.
    /// </summary>
    public partial class TileCustomMetadata
    {
        [DataMember(Name = "data")]
        public string Data { get; set; }

        [DataMember(Name = "tileId")]
        public int TileId { get; set; }
    }
}