using Newtonsoft.Json;

namespace LDtkUnity.Data
{
    /// <summary>
    /// A structure containing all the definitions of this project
    /// </summary>
    public class Definitions
    {
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

        [JsonProperty("tilesets")]
        public TilesetDefinition[] Tilesets { get; set; }
    }
}