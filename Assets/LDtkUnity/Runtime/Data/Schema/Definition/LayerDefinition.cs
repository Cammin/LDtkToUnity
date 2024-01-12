using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class LayerDefinition
    {
        /// <summary>
        /// Type of the layer (*IntGrid, Entities, Tiles or AutoLayer*)
        /// </summary>
        [DataMember(Name = "__type")]
        public string Type { get; set; }

        /// <summary>
        /// Contains all the auto-layer rule definitions.
        /// </summary>
        [DataMember(Name = "autoRuleGroups")]
        public AutoLayerRuleGroup[] AutoRuleGroups { get; set; }

        [DataMember(Name = "autoSourceLayerDefUid")]
        public int? AutoSourceLayerDefUid { get; set; }

        /// <summary>
        /// **WARNING**: this deprecated value is no longer exported since version 1.2.0  Replaced
        /// by: `tilesetDefUid`
        /// </summary>
        [DataMember(Name = "autoTilesetDefUid")]
        public int? AutoTilesetDefUid { get; set; }

        [DataMember(Name = "autoTilesKilledByOtherLayerUid")]
        public int? AutoTilesKilledByOtherLayerUid { get; set; }

        [DataMember(Name = "biomeFieldUid")]
        public int? BiomeFieldUid { get; set; }

        /// <summary>
        /// Allow editor selections when the layer is not currently active.
        /// </summary>
        [DataMember(Name = "canSelectWhenInactive")]
        public bool CanSelectWhenInactive { get; set; }

        /// <summary>
        /// Opacity of the layer (0 to 1.0)
        /// </summary>
        [DataMember(Name = "displayOpacity")]
        public float DisplayOpacity { get; set; }

        /// <summary>
        /// User defined documentation for this element to provide help/tips to level designers.
        /// </summary>
        [DataMember(Name = "doc")]
        public string Doc { get; set; }

        /// <summary>
        /// An array of tags to forbid some Entities in this layer
        /// </summary>
        [DataMember(Name = "excludedTags")]
        public string[] ExcludedTags { get; set; }

        /// <summary>
        /// Width and height of the grid in pixels
        /// </summary>
        [DataMember(Name = "gridSize")]
        public int GridSize { get; set; }

        /// <summary>
        /// Height of the optional "guide" grid in pixels
        /// </summary>
        [DataMember(Name = "guideGridHei")]
        public int GuideGridHei { get; set; }

        /// <summary>
        /// Width of the optional "guide" grid in pixels
        /// </summary>
        [DataMember(Name = "guideGridWid")]
        public int GuideGridWid { get; set; }

        [DataMember(Name = "hideFieldsWhenInactive")]
        public bool HideFieldsWhenInactive { get; set; }

        /// <summary>
        /// Hide the layer from the list on the side of the editor view.
        /// </summary>
        [DataMember(Name = "hideInList")]
        public bool HideInList { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Alpha of this layer when it is not the active one.
        /// </summary>
        [DataMember(Name = "inactiveOpacity")]
        public float InactiveOpacity { get; set; }

        /// <summary>
        /// An array that defines extra optional info for each IntGrid value.<br/>  WARNING: the
        /// array order is not related to actual IntGrid values! As user can re-order IntGrid values
        /// freely, you may value "2" before value "1" in this array.
        /// </summary>
        [DataMember(Name = "intGridValues")]
        public IntGridValueDefinition[] IntGridValues { get; set; }

        /// <summary>
        /// Group informations for IntGrid values
        /// </summary>
        [DataMember(Name = "intGridValuesGroups")]
        public IntGridValueGroupDefinition[] IntGridValuesGroups { get; set; }

        /// <summary>
        /// Parallax horizontal factor (from -1 to 1, defaults to 0) which affects the scrolling
        /// speed of this layer, creating a fake 3D (parallax) effect.
        /// </summary>
        [DataMember(Name = "parallaxFactorX")]
        public float ParallaxFactorX { get; set; }

        /// <summary>
        /// Parallax vertical factor (from -1 to 1, defaults to 0) which affects the scrolling speed
        /// of this layer, creating a fake 3D (parallax) effect.
        /// </summary>
        [DataMember(Name = "parallaxFactorY")]
        public float ParallaxFactorY { get; set; }

        /// <summary>
        /// If true (default), a layer with a parallax factor will also be scaled up/down accordingly.
        /// </summary>
        [DataMember(Name = "parallaxScaling")]
        public bool ParallaxScaling { get; set; }

        /// <summary>
        /// X offset of the layer, in pixels (IMPORTANT: this should be added to the `LayerInstance`
        /// optional offset)
        /// </summary>
        [DataMember(Name = "pxOffsetX")]
        public int PxOffsetX { get; set; }

        /// <summary>
        /// Y offset of the layer, in pixels (IMPORTANT: this should be added to the `LayerInstance`
        /// optional offset)
        /// </summary>
        [DataMember(Name = "pxOffsetY")]
        public int PxOffsetY { get; set; }

        /// <summary>
        /// If TRUE, the content of this layer will be used when rendering levels in a simplified way
        /// for the world view
        /// </summary>
        [DataMember(Name = "renderInWorldView")]
        public bool RenderInWorldView { get; set; }

        /// <summary>
        /// An array of tags to filter Entities that can be added to this layer
        /// </summary>
        [DataMember(Name = "requiredTags")]
        public string[] RequiredTags { get; set; }

        /// <summary>
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to
        /// position the tile relatively its grid cell.
        /// </summary>
        [DataMember(Name = "tilePivotX")]
        public float TilePivotX { get; set; }

        /// <summary>
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to
        /// position the tile relatively its grid cell.
        /// </summary>
        [DataMember(Name = "tilePivotY")]
        public float TilePivotY { get; set; }

        /// <summary>
        /// Reference to the default Tileset UID being used by this layer definition.<br/>
        /// **WARNING**: some layer *instances* might use a different tileset. So most of the time,
        /// you should probably use the `__tilesetDefUid` value found in layer instances.<br/>  Note:
        /// since version 1.0.0, the old `autoTilesetDefUid` was removed and merged into this value.
        /// </summary>
        [DataMember(Name = "tilesetDefUid")]
        public int? TilesetDefUid { get; set; }

        /// <summary>
        /// Type of the layer as Haxe Enum Possible values: `IntGrid`, `Entities`, `Tiles`,
        /// `AutoLayer`
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum LayerDefinitionType { get; set; }

        /// <summary>
        /// User defined color for the UI
        /// </summary>
        [DataMember(Name = "uiColor")]
        public string UiColor { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [DataMember(Name = "uid")]
        public int Uid { get; set; }

        /// <summary>
        /// Display tags
        /// </summary>
        [DataMember(Name = "uiFilterTags")]
        public string[] UiFilterTags { get; set; }

        /// <summary>
        /// Asynchronous rendering option for large/complex layers
        /// </summary>
        [DataMember(Name = "useAsyncRender")]
        public bool UseAsyncRender { get; set; }
    }
}