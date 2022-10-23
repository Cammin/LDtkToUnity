using System.Text.Json.Serialization;

namespace LDtkUnity
{
    public partial class LayerDefinition
    {
        /// <summary>
        /// Type of the layer (*IntGrid, Entities, Tiles or AutoLayer*)
        /// </summary>
        [JsonPropertyName("__type")]
        public string Type { get; set; }

        /// <summary>
        /// Contains all the auto-layer rule definitions.
        /// </summary>
        [JsonPropertyName("autoRuleGroups")]
        public AutoLayerRuleGroup[] AutoRuleGroups { get; set; }

        [JsonPropertyName("autoSourceLayerDefUid")]
        public long? AutoSourceLayerDefUid { get; set; }

        /// <summary>
        /// **WARNING**: this deprecated value will be *removed* completely on version 1.2.0+
        /// Replaced by: `tilesetDefUid`
        /// </summary>
        [JsonPropertyName("autoTilesetDefUid")]
        public long? AutoTilesetDefUid { get; set; }

        /// <summary>
        /// Opacity of the layer (0 to 1.0)
        /// </summary>
        [JsonPropertyName("displayOpacity")]
        public double DisplayOpacity { get; set; }

        /// <summary>
        /// An array of tags to forbid some Entities in this layer
        /// </summary>
        [JsonPropertyName("excludedTags")]
        public string[] ExcludedTags { get; set; }

        /// <summary>
        /// Width and height of the grid in pixels
        /// </summary>
        [JsonPropertyName("gridSize")]
        public long GridSize { get; set; }

        /// <summary>
        /// Height of the optional "guide" grid in pixels
        /// </summary>
        [JsonPropertyName("guideGridHei")]
        public long GuideGridHei { get; set; }

        /// <summary>
        /// Width of the optional "guide" grid in pixels
        /// </summary>
        [JsonPropertyName("guideGridWid")]
        public long GuideGridWid { get; set; }

        [JsonPropertyName("hideFieldsWhenInactive")]
        public bool HideFieldsWhenInactive { get; set; }

        /// <summary>
        /// Hide the layer from the list on the side of the editor view.
        /// </summary>
        [JsonPropertyName("hideInList")]
        public bool HideInList { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Alpha of this layer when it is not the active one.
        /// </summary>
        [JsonPropertyName("inactiveOpacity")]
        public double InactiveOpacity { get; set; }

        /// <summary>
        /// An array that defines extra optional info for each IntGrid value.<br/>  WARNING: the
        /// array order is not related to actual IntGrid values! As user can re-order IntGrid values
        /// freely, you may value "2" before value "1" in this array.
        /// </summary>
        [JsonPropertyName("intGridValues")]
        public IntGridValueDefinition[] IntGridValues { get; set; }

        /// <summary>
        /// Parallax horizontal factor (from -1 to 1, defaults to 0) which affects the scrolling
        /// speed of this layer, creating a fake 3D (parallax) effect.
        /// </summary>
        [JsonPropertyName("parallaxFactorX")]
        public double ParallaxFactorX { get; set; }

        /// <summary>
        /// Parallax vertical factor (from -1 to 1, defaults to 0) which affects the scrolling speed
        /// of this layer, creating a fake 3D (parallax) effect.
        /// </summary>
        [JsonPropertyName("parallaxFactorY")]
        public double ParallaxFactorY { get; set; }

        /// <summary>
        /// If true (default), a layer with a parallax factor will also be scaled up/down accordingly.
        /// </summary>
        [JsonPropertyName("parallaxScaling")]
        public bool ParallaxScaling { get; set; }

        /// <summary>
        /// X offset of the layer, in pixels (IMPORTANT: this should be added to the `LayerInstance`
        /// optional offset)
        /// </summary>
        [JsonPropertyName("pxOffsetX")]
        public long PxOffsetX { get; set; }

        /// <summary>
        /// Y offset of the layer, in pixels (IMPORTANT: this should be added to the `LayerInstance`
        /// optional offset)
        /// </summary>
        [JsonPropertyName("pxOffsetY")]
        public long PxOffsetY { get; set; }

        /// <summary>
        /// An array of tags to filter Entities that can be added to this layer
        /// </summary>
        [JsonPropertyName("requiredTags")]
        public string[] RequiredTags { get; set; }

        /// <summary>
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to
        /// position the tile relatively its grid cell.
        /// </summary>
        [JsonPropertyName("tilePivotX")]
        public double TilePivotX { get; set; }

        /// <summary>
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to
        /// position the tile relatively its grid cell.
        /// </summary>
        [JsonPropertyName("tilePivotY")]
        public double TilePivotY { get; set; }

        /// <summary>
        /// Reference to the default Tileset UID being used by this layer definition.<br/>
        /// **WARNING**: some layer *instances* might use a different tileset. So most of the time,
        /// you should probably use the `__tilesetDefUid` value found in layer instances.<br/>  Note:
        /// since version 1.0.0, the old `autoTilesetDefUid` was removed and merged into this value.
        /// </summary>
        [JsonPropertyName("tilesetDefUid")]
        public long? TilesetDefUid { get; set; }

        /// <summary>
        /// Type of the layer as Haxe Enum Possible values: `IntGrid`, `Entities`, `Tiles`,
        /// `AutoLayer`
        /// </summary>
        [JsonPropertyName("type")]
        public TypeEnum LayerDefinitionType { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonPropertyName("uid")]
        public long Uid { get; set; }
    }
}