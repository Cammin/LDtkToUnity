// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Level
{
    public struct LDtkDataEntityInstanceTile
    {
        /// <summary>
        /// An array of 4 Int values that refers to the tile in the tileset image: [ x, y, width, height ]
        /// </summary>
        public int[] srcRect;
        
        /// <summary>
        /// Tileset ID
        /// </summary>
        public int tilesetUid;
    }
}