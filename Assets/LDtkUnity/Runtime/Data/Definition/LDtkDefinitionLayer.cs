// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Providers;
using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#21-layer-definition
    public struct LDtkDefinitionLayer : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// Type of the layer (IntGrid, Entities, Tiles or AutoLayer)
        /// </summary>
        [JsonProperty] public string __type { get; private set; }
        
        /// <summary>
        /// Only Auto-layers.
        /// Contains all the auto-layer rule definitions.
        /// </summary>
        [JsonProperty] public LDtkDefinitionLayerAutoRuleGroup[] autoRuleGroups { get; private set; }
        
        /// <summary>
        /// Only Auto-layers.
        /// Reference to the IntGrid Layer defintion using this auto tile layer(?) //todo confirm this statement validity
        /// </summary>
        [JsonProperty] public int autoSourceLayerDefUid { get; private set; }
        
        /// <summary>
        /// Only Auto-layers.
        /// Reference to the Tileset UID being used by this auto-layer rules
        /// </summary>
        [JsonProperty] public int autoTilesetDefUid { get; private set; }
        
        /// <summary>
        /// Opacity of the layer (0 to 1.0)
        /// </summary>
        [JsonProperty] public float displayOpacity { get; private set; }
        
        /// <summary>
        /// Width and height of the grid in pixels
        /// </summary>
        [JsonProperty] public int gridSize { get; private set; }
        
        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty] public string identifier { get; private set; }

        /// <summary>
        /// Only IntGrid layer.
        /// </summary>
        [JsonProperty] public LDtkDefinitionIntGridValue[] intGridValues { get; private set; }

        /// <summary>
        /// X offset of the layer, in pixels (IMPORTANT: this should be added to the LayerInstance optional offset)
        /// </summary>
        /// <returns></returns>
        [JsonProperty] public int pxOffsetX { get; private set; }
        
        /// <summary>
        /// Y offset of the layer, in pixels (IMPORTANT: this should be added to the LayerInstance optional offset)
        /// </summary>
        /// <returns></returns>
        [JsonProperty] public int pxOffsetY { get; private set; }
        
        /// <summary>
        /// Only Tile layers.
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to position the tile relatively its grid cell.
        /// </summary>
        [JsonProperty] public float tilePivotX { get; private set; }
        
        /// <summary>
        /// Only Tile layers.
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to position the tile relatively its grid cell.
        /// </summary>
        [JsonProperty] public float tilePivotY { get; private set; }
        
        /// <summary>
        /// Only Tile layers.
        /// Reference to the Tileset UID being used by this tile layer
        /// </summary>
        [JsonProperty] public int tilesetDefUid { get; private set; }
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty] public int uid { get; private set; }

        public LDtkDefinitionLayer AutoSourceLayerDefinition => LDtkProviderUid.GetUidData<LDtkDefinitionLayer>(autoSourceLayerDefUid);
        
        public LDtkDefinitionTileset AutoTilesetDefinition => LDtkProviderUid.GetUidData<LDtkDefinitionTileset>(autoTilesetDefUid);
        public LDtkDefinitionTileset TileLayerDefinition => LDtkProviderUid.GetUidData<LDtkDefinitionTileset>(tilesetDefUid);
    }
}