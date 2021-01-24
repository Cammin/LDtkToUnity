using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class EnumDefinition
    {
        [JsonProperty("externalFileChecksum")]
        public string ExternalFileChecksum { get; set; }

        /// <summary>
        /// Relative path to the external file providing this Enum
        /// </summary>
        [JsonProperty("externalRelPath")]
        public string ExternalRelPath { get; set; }

        /// <summary>
        /// Tileset UID if provided
        /// </summary>
        [JsonProperty("iconTilesetUid")]
        public long? IconTilesetUid { get; set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// All possible enum values, with their optional Tile infos.
        /// </summary>
        [JsonProperty("values")]
        public EnumValueDefinition[] Values { get; set; }
    }
}