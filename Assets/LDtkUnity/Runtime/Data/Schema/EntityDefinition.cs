using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class EntityDefinition
    {
        /// <summary>
        /// Base entity color
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// Array of field definitions
        /// </summary>
        [JsonProperty("fieldDefs")]
        public FieldDefinition[] FieldDefs { get; set; }

        /// <summary>
        /// Pixel height
        /// </summary>
        [JsonProperty("height")]
        public long Height { get; set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Possible values: `DiscardOldOnes`, `PreventAdding`, `MoveLastOne`
        /// </summary>
        [JsonProperty("limitBehavior", NullValueHandling = NullValueHandling.Ignore)]
        public LimitBehavior? LimitBehavior { get; set; }

        /// <summary>
        /// Max instances per level
        /// </summary>
        [JsonProperty("maxPerLevel")]
        public long MaxPerLevel { get; set; }

        /// <summary>
        /// Pivot X coordinate (from 0 to 1.0)
        /// </summary>
        [JsonProperty("pivotX")]
        public double PivotX { get; set; }

        /// <summary>
        /// Pivot Y coordinate (from 0 to 1.0)
        /// </summary>
        [JsonProperty("pivotY")]
        public double PivotY { get; set; }

        /// <summary>
        /// Possible values: `Rectangle`, `Ellipse`, `Tile`, `Cross`
        /// </summary>
        [JsonProperty("renderMode", NullValueHandling = NullValueHandling.Ignore)]
        public RenderMode? RenderMode { get; set; }

        /// <summary>
        /// Display entity name in editor
        /// </summary>
        [JsonProperty("showName")]
        public bool ShowName { get; set; }

        /// <summary>
        /// Tile ID used for optional tile display
        /// </summary>
        [JsonProperty("tileId")]
        public long? TileId { get; set; }

        /// <summary>
        /// Possible values: `Stretch`, `Crop`
        /// </summary>
        [JsonProperty("tileRenderMode", NullValueHandling = NullValueHandling.Ignore)]
        public TileRenderMode? TileRenderMode { get; set; }

        /// <summary>
        /// Tileset ID used for optional tile display
        /// </summary>
        [JsonProperty("tilesetId")]
        public long? TilesetId { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// Pixel width
        /// </summary>
        [JsonProperty("width")]
        public long Width { get; set; }
    }
}