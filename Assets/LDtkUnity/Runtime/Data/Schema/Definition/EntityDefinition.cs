using System.Text.Json.Serialization;

namespace LDtkUnity
{
    public partial class EntityDefinition
    {
        /// <summary>
        /// Base entity color
        /// </summary>
        [JsonPropertyName("color")]
        public string Color { get; set; }

        /// <summary>
        /// Array of field definitions
        /// </summary>
        [JsonPropertyName("fieldDefs")]
        public FieldDefinition[] FieldDefs { get; set; }

        [JsonPropertyName("fillOpacity")]
        public double FillOpacity { get; set; }

        /// <summary>
        /// Pixel height
        /// </summary>
        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("hollow")]
        public bool Hollow { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Only applies to entities resizable on both X/Y. If TRUE, the entity instance width/height
        /// will keep the same aspect ratio as the definition.
        /// </summary>
        [JsonPropertyName("keepAspectRatio")]
        public bool KeepAspectRatio { get; set; }

        /// <summary>
        /// Possible values: `DiscardOldOnes`, `PreventAdding`, `MoveLastOne`
        /// </summary>
        [JsonPropertyName("limitBehavior")]
        public LimitBehavior LimitBehavior { get; set; }

        /// <summary>
        /// If TRUE, the maxCount is a "per world" limit, if FALSE, it's a "per level". Possible
        /// values: `PerLayer`, `PerLevel`, `PerWorld`
        /// </summary>
        [JsonPropertyName("limitScope")]
        public LimitScope LimitScope { get; set; }

        [JsonPropertyName("lineOpacity")]
        public double LineOpacity { get; set; }

        /// <summary>
        /// Max instances count
        /// </summary>
        [JsonPropertyName("maxCount")]
        public long MaxCount { get; set; }

        /// <summary>
        /// An array of 4 dimensions for the up/right/down/left borders (in this order) when using
        /// 9-slice mode for `tileRenderMode`.<br/>  If the tileRenderMode is not NineSlice, then
        /// this array is empty.<br/>  See: https://en.wikipedia.org/wiki/9-slice_scaling
        /// </summary>
        [JsonPropertyName("nineSliceBorders")]
        public long[] NineSliceBorders { get; set; }

        /// <summary>
        /// Pivot X coordinate (from 0 to 1.0)
        /// </summary>
        [JsonPropertyName("pivotX")]
        public double PivotX { get; set; }

        /// <summary>
        /// Pivot Y coordinate (from 0 to 1.0)
        /// </summary>
        [JsonPropertyName("pivotY")]
        public double PivotY { get; set; }

        /// <summary>
        /// Possible values: `Rectangle`, `Ellipse`, `Tile`, `Cross`
        /// </summary>
        [JsonPropertyName("renderMode")]
        public RenderMode RenderMode { get; set; }

        /// <summary>
        /// If TRUE, the entity instances will be resizable horizontally
        /// </summary>
        [JsonPropertyName("resizableX")]
        public bool ResizableX { get; set; }

        /// <summary>
        /// If TRUE, the entity instances will be resizable vertically
        /// </summary>
        [JsonPropertyName("resizableY")]
        public bool ResizableY { get; set; }

        /// <summary>
        /// Display entity name in editor
        /// </summary>
        [JsonPropertyName("showName")]
        public bool ShowName { get; set; }

        /// <summary>
        /// An array of strings that classifies this entity
        /// </summary>
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// **WARNING**: this deprecated value will be *removed* completely on version 1.2.0+
        /// Replaced by: `tileRect`
        /// </summary>
        [JsonPropertyName("tileId")]
        public long? TileId { get; set; }

        [JsonPropertyName("tileOpacity")]
        public double TileOpacity { get; set; }

        /// <summary>
        /// An object representing a rectangle from an existing Tileset
        /// </summary>
        [JsonPropertyName("tileRect")]
        public TilesetRectangle TileRect { get; set; }

        /// <summary>
        /// An enum describing how the the Entity tile is rendered inside the Entity bounds. Possible
        /// values: `Cover`, `FitInside`, `Repeat`, `Stretch`, `FullSizeCropped`,
        /// `FullSizeUncropped`, `NineSlice`
        /// </summary>
        [JsonPropertyName("tileRenderMode")]
        public TileRenderMode TileRenderMode { get; set; }

        /// <summary>
        /// Tileset ID used for optional tile display
        /// </summary>
        [JsonPropertyName("tilesetId")]
        public long? TilesetId { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonPropertyName("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// Pixel width
        /// </summary>
        [JsonPropertyName("width")]
        public long Width { get; set; }
    }
}