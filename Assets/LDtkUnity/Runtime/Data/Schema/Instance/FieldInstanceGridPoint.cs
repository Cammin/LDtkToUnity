using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// This object is just a grid-based coordinate used in Field values.
    /// </summary>
    public partial class FieldInstanceGridPoint
    {
        /// <summary>
        /// X grid-based coordinate
        /// </summary>
        [DataMember(Name = "cx")]
        public long Cx { get; set; }

        /// <summary>
        /// Y grid-based coordinate
        /// </summary>
        [DataMember(Name = "cy")]
        public long Cy { get; set; }
    }
}