using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class EnumValueDefinition
    {
        /// <summary>
        /// **WARNING**: this deprecated value is no longer exported since version 1.4.0  Replaced
        /// by: `tileRect`
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
        /// **WARNING**: this deprecated value is no longer exported since version 1.4.0  Replaced
        /// by: `tileRect`
        /// </summary>
        [DataMember(Name = "tileId")]
        public int? TileId { get; set; }

        /// <summary>
        /// Optional tileset rectangle to represents this value
        /// </summary>
        [DataMember(Name = "tileRect")]
        public TilesetRectangle TileRect { get; set; }
    }
}