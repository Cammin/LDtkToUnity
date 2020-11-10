// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#21-layer-definition
    public struct LDtkDefinitionLayer
    {
        /// <summary>
        /// Type of the layer (IntGrid, Entities, Tiles or AutoLayer)
        /// </summary>
        public string __type;
        
        /// <summary>
        /// Only Auto-layers.
        /// Contains all the auto-layer rule definitions.
        /// </summary>
        public LDtkDefinitionLayerAutoRuleGroup[] autoRuleGroups;
        
        /// <summary>
        /// Only Auto-layers
        /// </summary>
        public int autoSourceLayerDefUid;
        
        /// <summary>
        /// Only Auto-layers.
        /// Reference to the Tileset UID being used by this auto-layer rules
        /// </summary>
        public int autoTilesetDefUid;
        
        /// <summary>
        /// Opacity of the layer (0 to 1.0)
        /// </summary>
        public float displayOpacity;
        
        /// <summary>
        /// Width and height of the grid in pixels
        /// </summary>
        public int gridSize;
        
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string identifier;

        /// <summary>
        /// Only IntGrid layer.
        /// </summary>
        public LDtkDefinitionLayerIntGridValue[] intGridValues;

        /// <summary>
        /// X offset of the layer, in pixels (IMPORTANT: this should be added to the LayerInstance optional offset)
        /// </summary>
        /// <returns></returns>
        public int pxOffsetX;
        
        /// <summary>
        /// Y offset of the layer, in pixels (IMPORTANT: this should be added to the LayerInstance optional offset)
        /// </summary>
        /// <returns></returns>
        public int pxOffsetY;
        
        /// <summary>
        /// Only Tile layers.
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to position the tile relatively its grid cell.
        /// </summary>
        public float tilePivotX;
        
        /// <summary>
        /// Only Tile layers.
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to position the tile relatively its grid cell.
        /// </summary>
        public float tilePivotY;
        
        /// <summary>
        /// Only Tile layers.
        /// Reference to the Tileset UID being used by this tile layer
        /// </summary>
        public int tilesetDefUid;
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int uid;
    }
}