using UnityEngine;

namespace LDtkUnity
{
    public partial class EnumValueDefinition
    {
        /// <summary>
        /// Rect that refers to the tile in the tileset image of this enum value's definition
        /// </summary>
        public Rect UnityTileSrcRect => TileSrcRect.ToRect();
    }
}