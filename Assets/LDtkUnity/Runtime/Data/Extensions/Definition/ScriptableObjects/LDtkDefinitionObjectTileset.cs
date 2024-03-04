using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_TilesetDefJson)]
    public sealed class LDtkDefinitionObjectTileset : ScriptableObject
    {
        /// <summary>
        /// Grid-based size
        /// </summary>
        [field: SerializeField] public Vector2Int CSize { get; private set; }
        
        /// <summary>
        /// An array of custom tile metadata
        /// </summary>
        [field: SerializeField] public TileCustomMetadata[] CustomData { get; private set; }

        /// <summary>
        /// If this value is set, then it means that this atlas uses an internal LDtk atlas image
        /// instead of a loaded one. Possible values: &lt;`null`&gt;, `LdtkIcons`
        /// </summary>
        [field: SerializeField] public EmbedAtlas? EmbedAtlas { get; private set; }

        /// <summary>
        /// Tileset tags using Enum values specified by `tagsSourceEnumId`. This array contains 1
        /// element per Enum value, which contains an array of all Tile IDs that are tagged with it.
        /// </summary>
        [field: SerializeField] public EnumTagValue[] EnumTags { get; private set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [field: SerializeField] public string Identifier { get; private set; }

        /// <summary>
        /// Distance in pixels from image borders
        /// </summary>
        [field: SerializeField] public int Padding { get; private set; }

        /// <summary>
        /// Image size in pixels
        /// </summary>
        [field: SerializeField] public Vector2Int PxSize { get; private set; }

        /// <summary>
        /// Path to the source file, relative to the current project JSON file<br/>  It can be null
        /// if no image was provided, or when using an embed atlas.
        /// </summary>
        [field: SerializeField] public string RelPath { get; private set; }
        
        /// <summary>
        /// Space in pixels between all tiles
        /// </summary>
        [field: SerializeField] public int Spacing { get; private set; }

        /// <summary>
        /// An array of user-defined tags to organize the Tilesets
        /// </summary>
        [field: SerializeField] public string[] Tags { get; private set; }

        /// <summary>
        /// Optional Enum definition used for this tileset meta-data
        /// </summary>
        [field: SerializeField] public LDtkDefinitionObjectEnum TagsSourceEnum { get; private set; }

        [field: SerializeField] public int TileGridSize { get; private set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [field: SerializeField] public int Uid { get; private set; }
        
        #region EditorOnly

        /// <summary>
        /// The following data is used internally for various optimizations. It's always synced with
        /// source image changes.
        /// </summary>
        [field: Header("Internal")]
        [field: SerializeField] public System.Collections.Generic.Dictionary<string, object> CachedPixelData { get; private set; }
        
        /// <summary>
        /// Array of group of tiles selections, only meant to be used in the editor
        /// </summary>
        [field: SerializeField] public System.Collections.Generic.Dictionary<string, object>[] SavedSelections { get; private set; }

        #endregion
        
        internal void Populate(LDtkDefinitionObjectsCache cache, TilesetDefinition def)
        {
            name = $"Tileset_{def.Identifier}";
            
            CSize = def.UnityCSize;
            CustomData = def.CustomData; //todo make serialized
            EmbedAtlas = def.EmbedAtlas; //todo make serialized
            EnumTags = def.EnumTags; //todo make serialized
            Identifier = def.Identifier;
            Padding = def.Padding;
            PxSize = def.UnityPxSize;
            RelPath = def.RelPath;
            Spacing = def.Spacing;
            Tags = def.Tags;
            TagsSourceEnum = cache.GetObject(cache.Enums, def.TagsSourceEnumUid);
            TileGridSize = def.TileGridSize;
            Uid = def.Uid;
            CachedPixelData = def.CachedPixelData; //todo make serialized
            SavedSelections = def.SavedSelections; //todo make serialized
        }
    }
}