using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class EnumValueDefinition
    {
        /// <summary>
        /// An array of 4 Int values that refers to the tile in the tileset image: `[ x, y, width,
        /// height ]`
        /// </summary>
        [DataMember(Name = "__tileSrcRect")]
        public int[] TileSrcRect { get; set; }

        /// <summary>
        /// Optional color
        /// </summary>
        [DataMember(Name = "color")]
        public int Color { get; set; }

        /// <summary>
        /// Enum value
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The optional ID of the tile
        /// </summary>
        [DataMember(Name = "tileId")]
        public int? TileId { get; set; }
    }
}