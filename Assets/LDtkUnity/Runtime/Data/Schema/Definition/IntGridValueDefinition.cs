using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// IntGrid value definition
    /// </summary>
    public partial class IntGridValueDefinition
    {
        [DataMember(Name = "color")]
        public string Color { get; set; }

        /// <summary>
        /// Parent group identifier (0 if none)
        /// </summary>
        [DataMember(Name = "groupUid")]
        public int GroupUid { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        [DataMember(Name = "tile")]
        public TilesetRectangle Tile { get; set; }

        /// <summary>
        /// The IntGrid value itself
        /// </summary>
        [DataMember(Name = "value")]
        public int Value { get; set; }
    }
}