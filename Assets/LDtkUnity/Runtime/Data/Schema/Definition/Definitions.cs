using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// If you're writing your own LDtk importer, you should probably just ignore *most* stuff in
    /// the `defs` section, as it contains data that are mostly important to the editor. To keep
    /// you away from the `defs` section and avoid some unnecessary JSON parsing, important data
    /// from definitions is often duplicated in fields prefixed with a double underscore (eg.
    /// `__identifier` or `__type`).  The 2 only definition types you might need here are
    /// **Tilesets** and **Enums**.
    ///
    /// A structure containing all the definitions of this project
    /// </summary>
    public partial class Definitions
    {
        /// <summary>
        /// All entities definitions, including their custom fields
        /// </summary>
        [DataMember(Name = "entities")]
        public EntityDefinition[] Entities { get; set; }

        /// <summary>
        /// All internal enums
        /// </summary>
        [DataMember(Name = "enums")]
        public EnumDefinition[] Enums { get; set; }

        /// <summary>
        /// Note: external enums are exactly the same as `enums`, except they have a `relPath` to
        /// point to an external source file.
        /// </summary>
        [DataMember(Name = "externalEnums")]
        public EnumDefinition[] ExternalEnums { get; set; }

        /// <summary>
        /// All layer definitions
        /// </summary>
        [DataMember(Name = "layers")]
        public LayerDefinition[] Layers { get; set; }

        /// <summary>
        /// All custom fields available to all levels.
        /// </summary>
        [DataMember(Name = "levelFields")]
        public FieldDefinition[] LevelFields { get; set; }

        /// <summary>
        /// All tilesets
        /// </summary>
        [DataMember(Name = "tilesets")]
        public TilesetDefinition[] Tilesets { get; set; }
    }
}