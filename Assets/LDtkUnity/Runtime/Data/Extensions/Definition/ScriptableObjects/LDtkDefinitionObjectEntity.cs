using System.Linq;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_ENTITY_DEF_JSON)]
    public sealed class LDtkDefinitionObjectEntity : LDtkDefinitionObject<EntityDefinition>, ILDtkUid
    {
        [field: Tooltip("Base entity color")]
        [field: SerializeField] public Color Color { get; private set; }
        
        [field: Tooltip("Pixel size")]
        [field: SerializeField] public Vector2Int Size { get; private set; }
        
        [field: Tooltip("User defined unique identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: Tooltip("An array of 4 dimensions for the up/right/down/left borders (in this order) when using 9-slice mode for `tileRenderMode`. If the tileRenderMode is not NineSlice, then this array is empty.<br/>  See: https://en.wikipedia.org/wiki/9-slice_scaling")]
        [field: SerializeField] public Rect NineSliceBorders { get; private set; }
        
        [field: Tooltip("Pivot coordinate (from 0 to 1.0)")]
        [field: SerializeField] public Vector2 Pivot { get; private set; }
        
        [field: Tooltip("An object representing a rectangle from an existing Tileset")]
        [field: SerializeField] public Sprite TileRect { get; private set; }
        
        [field: Tooltip("An enum describing how the the Entity tile is rendered inside the Entity bounds. Possible values: `Cover`, `FitInside`, `Repeat`, `Stretch`, `FullSizeCropped`, `FullSizeUncropped`, `NineSlice`")]
        [field: SerializeField] public TileRenderMode TileRenderMode { get; private set; }
        
        [field: Tooltip("Tileset ID used for optional tile display")]
        [field: SerializeField] public LDtkDefinitionObjectTileset Tileset { get; private set; }
        
        [field: Tooltip("This tile overrides the one defined in `tileRect` in the UI")]
        [field: SerializeField] public Sprite UiTileRect { get; private set; }
        
        [field: Tooltip("Unique Int identifier")]
        [field: SerializeField] public int Uid { get; private set; }
        
        #region EditorOnly
        
        [field: Header("Internal")]
        [field: Tooltip("If enabled, this entity is allowed to stay outside of the current level bounds")]
        [field: SerializeField] public bool AllowOutOfBounds { get; private set; }
        
        [field: Tooltip("User defined documentation for this element to provide help/tips to level designers.")]
        [field: SerializeField] public string Doc { get; private set; }
        
        [field: Tooltip("If enabled, all instances of this entity will be listed in the project \"Table of content\" object.")]
        [field: SerializeField] public bool ExportToToc { get; private set; }
        
        [field: Tooltip("Array of field definitions")]
        [field: SerializeField] public LDtkDefinitionObjectField[] FieldDefs { get; private set; }

        [field: SerializeField] public float FillOpacity { get; private set; }
        
        [field: SerializeField] public bool Hollow { get; private set; }
        
        [field: Tooltip("Only applies to entities resizable on both X/Y. If TRUE, the entity instance width/height will keep the same aspect ratio as the definition.")]
        [field: SerializeField] public bool KeepAspectRatio { get; private set; }
        
        [field: Tooltip("Possible values: `DiscardOldOnes`, `PreventAdding`, `MoveLastOne`")]
        [field: SerializeField] public LimitBehavior LimitBehavior { get; private set; }
        
        [field: Tooltip("If TRUE, the maxCount is a \"per world\" limit, if FALSE, it's a \"per level\". Possible values: `PerLayer`, `PerLevel`, `PerWorld`")]
        [field: SerializeField] public LimitScope LimitScope { get; private set; }

        [field: SerializeField] public float LineOpacity { get; private set; }
        
        [field: Tooltip("Max instances count")]
        [field: SerializeField] public int MaxCount { get; private set; }
        
        [field: Tooltip("Max pixel size (only applies if the entity is resizable)")]
        [field: SerializeField] public Vector2Int MaxSize { get; private set; }
        
        [field: Tooltip("Min pixel size (only applies if the entity is resizable)")]
        [field: SerializeField] public Vector2Int MinSize { get; private set; }
        
        [field: Tooltip("Possible values: `Rectangle`, `Ellipse`, `Tile`, `Cross`")]
        [field: SerializeField] public RenderMode RenderMode { get; private set; }
        
        [field: Tooltip("If TRUE, the entity instances will be resizable horizontally")]
        [field: SerializeField] public bool ResizableX { get; private set; }
        
        [field: Tooltip("If TRUE, the entity instances will be resizable vertically")]
        [field: SerializeField] public bool ResizableY { get; private set; }
        
        [field: Tooltip("Display entity name in editor")]
        [field: SerializeField] public bool ShowName { get; private set; }
        
        [field: Tooltip("An array of strings that classifies this entity")]
        [field: SerializeField] public string[] Tags { get; private set; }

        [field: SerializeField] public float TileOpacity { get; private set; }
        
        #endregion

        internal override void SetAssetName()
        {
            name = $"Entity_{Uid}_{Identifier}";
        }
        
        internal override void Populate(LDtkDefinitionObjectsCache cache, EntityDefinition def)
        {
            Color = def.UnityColor;
            Size = def.UnitySize;
            Identifier = def.Identifier;
            NineSliceBorders = def.UnityNineSliceBorders;
            Pivot = def.UnityPivot;
            TileRect = cache.GetSpriteForTilesetRectangle(def.TileRect);
            TileRenderMode = def.TileRenderMode;
            Tileset = cache.GetObject<LDtkDefinitionObjectTileset>(def.TilesetId);
            UiTileRect = cache.GetSpriteForTilesetRectangle(def.UiTileRect);
            Uid = def.Uid;
            
            //editor only
            AllowOutOfBounds = def.AllowOutOfBounds;
            Doc = def.Doc;
            ExportToToc = def.ExportToToc;
            FieldDefs = def.FieldDefs.Select(p => cache.GetObject<LDtkDefinitionObjectField>(p.Uid)).ToArray();
            FillOpacity = def.FillOpacity;
            Hollow = def.Hollow;
            KeepAspectRatio = def.KeepAspectRatio;
            LimitBehavior = def.LimitBehavior;
            LineOpacity = def.LineOpacity;
            MaxCount = def.MaxCount;
            
            MinSize = new Vector2Int(
                def.MinWidth != null ? def.MinWidth.Value : int.MinValue, 
                def.MinHeight != null ? def.MinHeight.Value : int.MinValue);
            MaxSize = new Vector2Int(
                def.MaxWidth != null ? def.MaxWidth.Value : int.MaxValue, 
                def.MaxHeight != null ? def.MaxHeight.Value : int.MaxValue);
            
            RenderMode = def.RenderMode;
            ResizableX = def.ResizableX;
            ResizableY = def.ResizableY;
            ShowName = def.ShowName;
            Tags = def.Tags;
            TileOpacity = def.TileOpacity;

            for (int i = 0; i < FieldDefs.Length; i++)
            {
                FieldDefs[i].Populate(cache, def.FieldDefs[i]);
            }
        }
    }
}