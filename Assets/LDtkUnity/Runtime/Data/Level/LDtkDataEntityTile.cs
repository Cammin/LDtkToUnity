// ReSharper disable InconsistentNaming

using UnityEngine;

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#112-entity-instance
    public struct LDtkDataEntityTile
    {
        /// <summary>
        /// An array of 4 Int values that refers to the tile in the tileset image: [ x, y, width, height ]
        /// </summary>
        public int[] srcRect;
        
        /// <summary>
        /// Tileset ID
        /// </summary>
        public bool tilesetUid;

        
        public Rect SourceRect => new Rect(srcRect[0], srcRect[1], srcRect[2], srcRect[3]);
    }
}