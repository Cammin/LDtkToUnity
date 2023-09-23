using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// IntGrid value group definition
    /// </summary>
    public partial class IntGridValueGroupDefinition
    {
        /// <summary>
        /// User defined color
        /// </summary>
        [DataMember(Name = "color")]
        public string Color { get; set; }

        /// <summary>
        /// User defined string identifier
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Group unique ID
        /// </summary>
        [DataMember(Name = "uid")]
        public int Uid { get; set; }
    }
}