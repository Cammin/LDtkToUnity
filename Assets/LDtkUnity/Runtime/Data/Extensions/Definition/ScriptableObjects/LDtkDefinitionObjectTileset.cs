using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_TILESET_DEF_JSON)]
    public sealed class LDtkDefinitionObjectTileset : LDtkDefinitionObject<TilesetDefinition>, ILDtkUid
    {
        [field: Tooltip("Grid-based size")]
        [field: SerializeField] public Vector2Int CSize { get; private set; }
        
        [field: Tooltip("If this value is set, then it means that this atlas uses an internal LDtk atlas image instead of a loaded one.")]
        [field: SerializeField] public bool EmbedAtlas { get; private set; }
        
        [field: Tooltip("User defined unique identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: Tooltip("Distance in pixels from image borders")]
        [field: SerializeField] public int Padding { get; private set; }
        
        [field: Tooltip("Image size in pixels")]
        [field: SerializeField] public Vector2Int PxSize { get; private set; }
        
        [field: Tooltip("Path to the source file, relative to the current project JSON file. It can be null if no image was provided, or when using an embed atlas.")]
        [field: SerializeField] public string RelPath { get; private set; }
        
        [field: Tooltip("Space in pixels between all tiles")]
        [field: SerializeField] public int Spacing { get; private set; }
        
        [field: Tooltip("An array of user-defined tags to organize the Tilesets")]
        [field: SerializeField] public string[] Tags { get; private set; }
        
        [field: Tooltip("Optional Enum definition used for this tileset meta-data")]
        [field: SerializeField] public LDtkDefinitionObjectEnum TagsSourceEnum { get; private set; }

        [field: SerializeField] public int TileGridSize { get; private set; }
        
        [field: Tooltip("Unique Int identifier")]
        [field: SerializeField] public int Uid { get; private set; }
        
        #region VeryEditorOnly
        
        /*
        [field: Tooltip("Tileset tags using Enum values specified by `tagsSourceEnumId`. This array contains 1 element per Enum value, which contains an array of all Tile IDs that are tagged with it.")]
        [field: SerializeField] private EnumTagValue[] EnumTags { get; set; }
        [field: Tooltip("An array of custom tile metadata")]
        [field: SerializeField] private TileCustomMetadata[] CustomData { get; set; }
        
        [field: Header("Internal")]
        [field: Tooltip("The following data is used internally for various optimizations. It's always synced with source image changes.")]
        [field: SerializeField] private System.Collections.Generic.Dictionary<string, object> CachedPixelData { get; set; }
        
        [field: Tooltip("Array of group of tiles selections, only meant to be used in the editor")]
        [field: SerializeField] private System.Collections.Generic.Dictionary<string, object>[] SavedSelections { get; set; }
        */

        #endregion
        
        internal override void SetAssetName()
        {
            name = $"Tileset_{Uid}_{Identifier}";
        }
        
        internal override void Populate(LDtkDefinitionObjectsCache cache, TilesetDefinition def)
        {
            CSize = def.UnityCSize;
            EmbedAtlas = def.EmbedAtlas != null;
            Identifier = def.Identifier;
            Padding = def.Padding;
            PxSize = def.UnityPxSize;
            RelPath = def.RelPath;
            Spacing = def.Spacing;
            Tags = def.Tags;
            TagsSourceEnum = cache.GetObject<LDtkDefinitionObjectEnum>(def.TagsSourceEnumUid);
            TileGridSize = def.TileGridSize;
            Uid = def.Uid;
            
            //ignore these, the data is in the tiles instead
            //CustomData = def.CustomData;
            //EnumTags = def.EnumTags;
            
            //these are very editor-only. don't make serialized at all
            //CachedPixelData = def.CachedPixelData;
            //SavedSelections = def.SavedSelections;
        }
    }
}