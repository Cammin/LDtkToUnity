using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class Definitions
    {
        /// <summary>
        /// All entities, including their custom fields
        /// </summary>
        [JsonProperty("entities")]
        public EntityDefinition[] Entities { get; set; }

        [JsonProperty("enums")]
        public EnumDefinition[] Enums { get; set; }

        /// <summary>
        /// Note: external enums are exactly the same as `enums`, except they have a `relPath` to
        /// point to an external source file.
        /// </summary>
        [JsonProperty("externalEnums")]
        public EnumDefinition[] ExternalEnums { get; set; }

        [JsonProperty("layers")]
        public LayerDefinition[] Layers { get; set; }

        /// <summary>
        /// An array containing all custom fields available to all levels.
        /// </summary>
        [JsonProperty("levelFields")]
        public FieldDefinition[] LevelFields { get; set; }

        [JsonProperty("tilesets")]
        public TilesetDefinition[] Tilesets { get; set; }
    }
}