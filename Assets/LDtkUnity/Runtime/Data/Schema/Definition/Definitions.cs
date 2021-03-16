using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// A structure containing all the definitions of this project
    ///
    /// If you're writing your own LDtk importer, you should probably just ignore *most* stuff in
    /// the `defs` section, as it contains data that are mostly important to the editor. To keep
    /// you away from the `defs` section and avoid some unnecessary JSON parsing, important data
    /// from definitions is often duplicated in fields prefixed with a double underscore (eg.
    /// `__identifier` or `__type`).  The 2 only definition types you might need here are
    /// **Tilesets** and **Enums**.
    /// </summary>
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