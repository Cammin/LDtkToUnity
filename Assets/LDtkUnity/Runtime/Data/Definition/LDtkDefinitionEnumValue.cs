// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#24-enum-definition
    public struct LDtkDefinitionEnumValue
    {
        /// <summary>
        /// An array of 4 Int values that refers to the tile in the tileset image: [ x, y, width, height ]
        /// </summary>
        public int[] __tileSrcRect;

        /// <summary>
        /// Enum value
        /// </summary>
        public string id;
        
        /// <summary>
        /// The optional ID of the tile
        /// </summary>
        public int? tileId;

        public Rect SourceRect => __tileSrcRect.ToRect();
    }
}