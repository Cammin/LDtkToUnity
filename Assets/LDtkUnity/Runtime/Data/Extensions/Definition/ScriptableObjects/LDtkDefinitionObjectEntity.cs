using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_EntityDefJson)]
    public sealed class LDtkDefinitionObjectEntity : ScriptableObject
    {
        [field: Tooltip("Base entity color")]
        [field: SerializeField] public Color Color { get; private set; }
        
        [field: Tooltip("Pixel size")]
        [field: SerializeField] public Vector2Int Size { get; private set; }
        
        [field: Tooltip("User defined unique identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: Tooltip("An array of 4 dimensions for the up/right/down/left borders (in this order) when using\n9-slice mode for `tileRenderMode`.<br/>  If the tileRenderMode is not NineSlice, then\nthis array is empty.<br/>  See: https://en.wikipedia.org/wiki/9-slice_scaling\n")]
        [field: SerializeField] public Rect NineSliceBorders { get; private set; }
        
        [field: Tooltip("Pivot coordinate (from 0 to 1.0)")]
        [field: SerializeField] public Vector2 Pivot { get; private set; }
        
        [field: Tooltip("An object representing a rectangle from an existing Tileset")]
        [field: SerializeField] public LDtkDefinitionObjectTilesetRectangle TileRect { get; private set; }
        
        [field: Tooltip("An enum describing how the the Entity tile is rendered inside the Entity bounds. Possible\nvalues: `Cover`, `FitInside`, `Repeat`, `Stretch`, `FullSizeCropped`,\n `FullSizeUncropped`, `NineSlice`")]
        [field: SerializeField] public TileRenderMode TileRenderMode { get; private set; }
        
        [field: Tooltip("Tileset ID used for optional tile display")]
        [field: SerializeField] public LDtkDefinitionObjectTileset Tileset { get; private set; }
        
        [field: Tooltip("This tile overrides the one defined in `tileRect` in the UI")]
        [field: SerializeField] public LDtkDefinitionObjectTilesetRectangle UiTileRect { get; private set; }
        
        [field: Tooltip("Unique Int identifier")]
        [field: SerializeField] public int Uid { get; private set; }
        
        #region EditorOnly
        
        /// <summary>
        /// If enabled, this entity is allowed to stay outside of the current level bounds
        /// </summary>
        [field: Header("Internal")]
        [field: SerializeField] public bool AllowOutOfBounds { get; private set; }
        
        /// <summary>
        /// User defined documentation for this element to provide help/tips to level designers.
        /// </summary>
        [field: SerializeField] public string Doc { get; private set; }

        /// <summary>
        /// If enabled, all instances of this entity will be listed in the project "Table of content"
        /// object.
        /// </summary>
        [field: SerializeField] public bool ExportToToc { get; private set; }

        /// <summary>
        /// Array of field definitions
        /// </summary>
        [field: SerializeField] public LDtkDefinitionObjectField[] FieldDefs { get; private set; }

        [field: SerializeField] public float FillOpacity { get; private set; }
        
        [field: SerializeField] public bool Hollow { get; private set; }
        
        /// <summary>
        /// Only applies to entities resizable on both X/Y. If TRUE, the entity instance width/height
        /// will keep the same aspect ratio as the definition.
        /// </summary>
        [field: SerializeField] public bool KeepAspectRatio { get; private set; }

        /// <summary>
        /// Possible values: `DiscardOldOnes`, `PreventAdding`, `MoveLastOne`
        /// </summary>
        [field: SerializeField] public LimitBehavior LimitBehavior { get; private set; }

        /// <summary>
        /// If TRUE, the maxCount is a "per world" limit, if FALSE, it's a "per level". Possible
        /// values: `PerLayer`, `PerLevel`, `PerWorld`
        /// </summary>
        [field: SerializeField] public LimitScope LimitScope { get; private set; }

        [field: SerializeField] public float LineOpacity { get; private set; }

        /// <summary>
        /// Max instances count
        /// </summary>
        [field: SerializeField] public int MaxCount { get; private set; }

        /// <summary>
        /// Max pixel height (only applies if the entity is resizable on Y)
        /// </summary>
        [field: SerializeField] public int? MaxHeight { get; private set; }

        /// <summary>
        /// Max pixel width (only applies if the entity is resizable on X)
        /// </summary>
        [field: SerializeField] public int? MaxWidth { get; private set; }

        /// <summary>
        /// Min pixel height (only applies if the entity is resizable on Y)
        /// </summary>
        [field: SerializeField] public int? MinHeight { get; private set; }

        /// <summary>
        /// Min pixel width (only applies if the entity is resizable on X)
        /// </summary>
        [field: SerializeField] public int? MinWidth { get; private set; }
        
        /// <summary>
        /// Possible values: `Rectangle`, `Ellipse`, `Tile`, `Cross`
        /// </summary>
        [field: SerializeField] public RenderMode RenderMode { get; private set; }

        /// <summary>
        /// If TRUE, the entity instances will be resizable horizontally
        /// </summary>
        [field: SerializeField] public bool ResizableX { get; private set; }

        /// <summary>
        /// If TRUE, the entity instances will be resizable vertically
        /// </summary>
        [field: SerializeField] public bool ResizableY { get; private set; }

        /// <summary>
        /// Display entity name in editor
        /// </summary>
        [field: SerializeField] public bool ShowName { get; private set; }

        /// <summary>
        /// An array of strings that classifies this entity
        /// </summary>
        [field: SerializeField] public string[] Tags { get; private set; }

        [field: SerializeField] public float TileOpacity { get; private set; }
        
        #endregion
        
        internal void Populate(LDtkDefinitionObjectsCache cache, EntityDefinition def)
        {
            name = $"Entity_{def.Identifier}";
            
            Color = def.UnityColor;
            Size = def.UnitySize;
            Identifier = def.Identifier;
            NineSliceBorders = def.UnityNineSliceBorders;
            Pivot = def.UnityPivot;
            if (def.TileRect != null)
            {
                TileRect = ScriptableObject.CreateInstance<LDtkDefinitionObjectTilesetRectangle>();
                TileRect.Populate(cache, def.TileRect);
            }
            TileRenderMode = def.TileRenderMode;
            Tileset = cache.GetObject(cache.Tilesets, def.TilesetId);
            if (def.UiTileRect != null)
            {
                UiTileRect = ScriptableObject.CreateInstance<LDtkDefinitionObjectTilesetRectangle>();
                UiTileRect.Populate(cache, def.UiTileRect);
            }
            
            Uid = def.Uid;
            
            //editor only
            AllowOutOfBounds = def.AllowOutOfBounds;
            Doc = def.Doc;
            ExportToToc = def.ExportToToc;
            FieldDefs = def.FieldDefs.Select(p => cache.GetObject(cache.EntityFields, p.Uid)).ToArray();
            FillOpacity = def.FillOpacity;
            Hollow = def.Hollow;
            KeepAspectRatio = def.KeepAspectRatio;
            LimitBehavior = def.LimitBehavior;
            LineOpacity = def.LineOpacity;
            MaxCount = def.MaxCount;
            MaxHeight = def.MaxHeight; //todo make serializable
            MaxWidth = def.MaxWidth; //todo make serializable
            MinHeight = def.MinHeight; //todo make serializable
            MinWidth = def.MinWidth; //todo make serializable
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