// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#111-tile-instance--
    public struct LDtkDataTile
    {
        /// <summary>
        /// Internal data used by the editor.
        /// For auto-layer tiles: [ruleId, coordId, tileId].
        /// For tile-layer tiles: [coordId, tileId].
        /// </summary>
        public int[] d;
        
        /// <summary>
        /// "Flip flags", a 2-bits integer to represent the mirror transformations of the tile.
        /// -Bit 0 = X flip
        /// -Bit 1 = Y flip
        /// </summary>
        public int f;
        
        /// <summary>
        /// Pixel coordinates of the tile in the layer ([x,y] format). Don't forget optional layer offsets, if they exist!
        /// </summary>
        public int[] px;
        
        /// <summary>
        /// Pixel coordinates of the tile in the tileset ([x,y] format)
        /// </summary>
        public int[] src;
    }
}