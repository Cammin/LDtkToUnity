using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// IntGrid value instance
    /// </summary>
    public partial class IntGridValueInstance
    {
        /// <summary>
        /// Coordinate ID in the layer grid
        /// </summary>
        [DataMember(Name = "coordId")]
        public int CoordId { get; set; }

        /// <summary>
        /// IntGrid value
        /// </summary>
        [DataMember(Name = "v")]
        public int V { get; set; }
    }
}