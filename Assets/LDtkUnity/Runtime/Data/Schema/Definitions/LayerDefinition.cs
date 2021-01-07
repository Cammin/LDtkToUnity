using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDtkUnity.Data
{
    public class LayerDefinition : ILDtkUid, ILDtkIdentifier
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
        public Dictionary<string, dynamic>[] AutoRuleGroups { get; set; }

        [JsonProperty("autoSourceLayerDefUid")]
        public long? AutoSourceLayerDefUid { get; set; }

        /// <summary>
        /// Reference to the Tileset UID being used by this auto-layer rules
        /// </summary>
        [JsonProperty("autoTilesetDefUid")]
        public long? AutoTilesetDefUid { get; set; }

        /// <summary>
        /// Opacity of the layer (0 to 1.0)
        /// </summary>
        [JsonProperty("displayOpacity")]
        public double DisplayOpacity { get; set; }

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

        [JsonProperty("intGridValues")]
        public Dictionary<string, dynamic>[] IntGridValues { get; set; }

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
        /// Reference to the Tileset UID being used by this tile layer
        /// </summary>
        [JsonProperty("tilesetDefUid")]
        public long? TilesetDefUid { get; set; }

        /// <summary>
        /// Type of the layer as Haxe Enum Possible values: `IntGrid`, `Entities`, `Tiles`,
        /// `AutoLayer`
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public TypeEnum? LayerDefinitionType { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty("uid")]
        public long Uid { get; set; }
    }
}