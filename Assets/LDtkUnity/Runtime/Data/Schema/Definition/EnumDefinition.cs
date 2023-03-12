using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class EnumDefinition
    {
        [DataMember(Name = "externalFileChecksum")]
        public string ExternalFileChecksum { get; set; }

        /// <summary>
        /// Relative path to the external file providing this Enum
        /// </summary>
        [DataMember(Name = "externalRelPath")]
        public string ExternalRelPath { get; set; }

        /// <summary>
        /// Tileset UID if provided
        /// </summary>
        [DataMember(Name = "iconTilesetUid")]
        public int? IconTilesetUid { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// An array of user-defined tags to organize the Enums
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [DataMember(Name = "uid")]
        public int Uid { get; set; }

        /// <summary>
        /// All possible enum values, with their optional Tile infos.
        /// </summary>
        [DataMember(Name = "values")]
        public EnumValueDefinition[] Values { get; set; }
    }
}