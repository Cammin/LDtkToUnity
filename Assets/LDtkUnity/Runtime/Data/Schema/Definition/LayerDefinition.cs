using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class LayerDefinition
    {
        /// <summary>
        /// Type of the layer (*IntGrid, Entities, Tiles or AutoLayer*)
        /// </summary>
        [JsonProperty("__type")]
        public string Type { get; set; }

        /// <summary>
        /// Contains all the auto-layer rule definitions.
        /// </summary>
        [JsonProperty("autoRuleGroups")]
        public AutoLayerRuleGroup[] AutoRuleGroups { get; set; }

        [JsonProperty("autoSourceLayerDefUid")]
        public long? AutoSourceLayerDefUid { get; set; }

        /// <summary>
        /// Reference to the Tileset UID being used by this auto-layer rules. WARNING: some layer
        /// *instances* might use a different tileset. So most of the time, you should probably use
        /// the `__tilesetDefUid` value from layer instances.
        /// </summary>
        [JsonProperty("autoTilesetDefUid")]
        public long? AutoTilesetDefUid { get; set; }

        /// <summary>
        /// Opacity of the layer (0 to 1.0)
        /// </summary>
        [JsonProperty("displayOpacity")]
        public double DisplayOpacity { get; set; }

        /// <summary>
        /// An array of tags to forbid some Entities in this layer
        /// </summary>
        [JsonProperty("excludedTags")]
        public string[] ExcludedTags { get; set; }

        /// <summary>
        /// Width and height of the grid in pixels
        /// </summary>
        [JsonProperty("gridSize")]
        public long GridSize { get; set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// An array that defines extra optional info for each IntGrid value. The array is sorted
        /// using value (ascending).
        /// </summary>
        [JsonProperty("intGridValues")]
        public IntGridValueDefinition[] IntGridValues { get; set; }

        /// <summary>
        /// X offset of the layer, in pixels (IMPORTANT: this should be added to the `LayerInstance`
        /// optional offset)
        /// </summary>
        [JsonProperty("pxOffsetX")]
        public long PxOffsetX { get; set; }

        /// <summary>
        /// Y offset of the layer, in pixels (IMPORTANT: this should be added to the `LayerInstance`
        /// optional offset)
        /// </summary>
        [JsonProperty("pxOffsetY")]
        public long PxOffsetY { get; set; }

        /// <summary>
        /// An array of tags to filter Entities that can be added to this layer
        /// </summary>
        [JsonProperty("requiredTags")]
        public string[] RequiredTags { get; set; }

        /// <summary>
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to
        /// position the tile relatively its grid cell.
        /// </summary>
        [JsonProperty("tilePivotX")]
        public double TilePivotX { get; set; }

        /// <summary>
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to
        /// position the tile relatively its grid cell.
        /// </summary>
        [JsonProperty("tilePivotY")]
        public double TilePivotY { get; set; }

        /// <summary>
        /// Reference to the Tileset UID being used by this Tile layer. WARNING: some layer
        /// *instances* might use a different tileset. So most of the time, you should probably use
        /// the `__tilesetDefUid` value from layer instances.
        /// </summary>
        [JsonProperty("tilesetDefUid")]
        public long? TilesetDefUid { get; set; }

        /// <summary>
        /// Type of the layer as Haxe Enum Possible values: `IntGrid`, `Entities`, `Tiles`,
        /// `AutoLayer`
        /// </summary>
        [JsonProperty("type")]
        public TypeEnum LayerDefinitionType { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty("uid")]
        public long Uid { get; set; }
    }
}