// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Level
{
    public struct LDtkDataTileInstance
    {
        /// <summary>
        /// Pixel coordinates of the tile in the layer ([x,y] format)
        /// </summary>
        public int[] px;
        
        /// <summary>
        /// Pixel coordinates of the tile in the tileset ([x,y] format)
        /// </summary>
        public int[] src;
        
        /// <summary>
        /// "Flip flags", a 2-bits integer to represent the mirror transformations of the tile.
        /// -Bit 0 = X flip
        /// -Bit 1 = Y flip
        /// </summary>
        public int f;
        
        /// <summary>
        /// Internal data used by the editor.
        /// For auto-layer tiles: [ruleId, coordId, tileId].
        /// For tile-layer tiles: [coordId, tileId].
        /// </summary>
        public int[] d;
    }
}