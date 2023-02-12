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
        /// User defined unique identifier
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// The IntGrid value itself
        /// </summary>
        [DataMember(Name = "value")]
        public int Value { get; set; }
    }
}