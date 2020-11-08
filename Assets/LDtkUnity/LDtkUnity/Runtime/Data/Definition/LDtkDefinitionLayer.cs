// ReSharper disable InconsistentNaming

using System;

namespace LDtkUnity.Runtime.Data.Definition
{
    [Serializable]
    public struct LDtkDefinitionLayer
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string identifier;
        
        /// <summary>
        /// Type of the layer (IntGrid, Entities, Tiles or AutoLayer)
        /// </summary>
        public string type;
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int uid;
        
        /// <summary>
        /// 
        /// </summary>
        public int gridSize;
        
        /// <summary>
        /// Opacity of the layer (0 to 1.0)
        /// </summary>
        public float displayOpacity;
        
        /// <summary>
        /// Only IntGrid layer.
        /// </summary>
        public LDtkDefinitionLayerIntGridValue[] intGridValues;
        
        /// <summary>
        /// Only Auto-layers.
        /// Reference to the Tileset UID being used by this auto-layer rules
        /// </summary>
        public int autoTilesetDefUid;
        
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
        /// Only Tile layers.
        /// Reference to the Tileset UID being used by this tile layer
        /// </summary>
        public int tilesetDefUid;

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
    }
}