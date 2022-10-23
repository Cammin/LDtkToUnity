using System.Text.Json.Serialization;

namespace LDtkUnity
{
    public partial class EnumDefinition
    {
        [JsonPropertyName("externalFileChecksum")]
        public string ExternalFileChecksum { get; set; }

        /// <summary>
        /// Relative path to the external file providing this Enum
        /// </summary>
        [JsonPropertyName("externalRelPath")]
        public string ExternalRelPath { get; set; }

        /// <summary>
        /// Tileset UID if provided
        /// </summary>
        [JsonPropertyName("iconTilesetUid")]
        public long? IconTilesetUid { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// An array of user-defined tags to organize the Enums
        /// </summary>
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonPropertyName("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// All possible enum values, with their optional Tile infos.
        /// </summary>
        [JsonPropertyName("values")]
        public EnumValueDefinition[] Values { get; set; }
    }
}