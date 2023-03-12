using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// The `Tileset` definition is the most important part among project definitions. It
    /// contains some extra informations about each integrated tileset. If you only had to parse
    /// one definition section, that would be the one.
    /// </summary>
    public partial class TilesetDefinition
    {
        /// <summary>
        /// Grid-based height
        /// </summary>
        [DataMember(Name = "__cHei")]
        public int CHei { get; set; }

        /// <summary>
        /// Grid-based width
        /// </summary>
        [DataMember(Name = "__cWid")]
        public int CWid { get; set; }

        /// <summary>
        /// The following data is used internally for various optimizations. It's always synced with
        /// source image changes.
        /// </summary>
        [DataMember(Name = "cachedPixelData")]
        public System.Collections.Generic.Dictionary<string, object> CachedPixelData { get; set; }

        /// <summary>
        /// An array of custom tile metadata
        /// </summary>
        [DataMember(Name = "customData")]
        public TileCustomMetadata[] CustomData { get; set; }

        /// <summary>
        /// If this value is set, then it means that this atlas uses an internal LDtk atlas image
        /// instead of a loaded one. Possible values: &lt;`null`&gt;, `LdtkIcons`
        /// </summary>
        [DataMember(Name = "embedAtlas")]
        public EmbedAtlas? EmbedAtlas { get; set; }

        /// <summary>
        /// Tileset tags using Enum values specified by `tagsSourceEnumId`. This array contains 1
        /// element per Enum value, which contains an array of all Tile IDs that are tagged with it.
        /// </summary>
        [DataMember(Name = "enumTags")]
        public EnumTagValue[] EnumTags { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Distance in pixels from image borders
        /// </summary>
        [DataMember(Name = "padding")]
        public int Padding { get; set; }

        /// <summary>
        /// Image height in pixels
        /// </summary>
        [DataMember(Name = "pxHei")]
        public int PxHei { get; set; }

        /// <summary>
        /// Image width in pixels
        /// </summary>
        [DataMember(Name = "pxWid")]
        public int PxWid { get; set; }

        /// <summary>
        /// Path to the source file, relative to the current project JSON file<br/>  It can be null
        /// if no image was provided, or when using an embed atlas.
        /// </summary>
        [DataMember(Name = "relPath")]
        public string RelPath { get; set; }

        /// <summary>
        /// Array of group of tiles selections, only meant to be used in the editor
        /// </summary>
        [DataMember(Name = "savedSelections")]
        public System.Collections.Generic.Dictionary<string, object>[] SavedSelections { get; set; }

        /// <summary>
        /// Space in pixels between all tiles
        /// </summary>
        [DataMember(Name = "spacing")]
        public int Spacing { get; set; }

        /// <summary>
        /// An array of user-defined tags to organize the Tilesets
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Optional Enum definition UID used for this tileset meta-data
        /// </summary>
        [DataMember(Name = "tagsSourceEnumUid")]
        public int? TagsSourceEnumUid { get; set; }

        [DataMember(Name = "tileGridSize")]
        public int TileGridSize { get; set; }

        /// <summary>
        /// Unique Intidentifier
        /// </summary>
        [DataMember(Name = "uid")]
        public int Uid { get; set; }
    }
}