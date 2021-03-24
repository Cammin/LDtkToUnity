using System;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LdtkJson
    {
        /// <summary>
        /// Project background color
        /// </summary>
        public Color UnityBgColor => BgColor.ToColor();
        
        /// <summary>
        /// Default background color of levels
        /// </summary>
        public Color UnityDefaultLevelBgColor => DefaultLevelBgColor.ToColor();

        /// <summary>
        /// Default new level size
        /// </summary>
        public Vector2Int UnityDefaultLevelSize => new Vector2Int((int)DefaultLevelWidth, (int)DefaultLevelHeight);
        
        /// <summary>
        /// Default pivot (0 to 1) for new entities
        /// </summary>
        public Vector2 UnityDefaultPivot => new Vector2((int)DefaultPivotX, (int)DefaultPivotY);
        
        /// <summary>
        /// Size of the world grid in pixels.
        /// </summary>
        public Vector2Int UnityWorldGridSize => new Vector2Int((int)WorldGridWidth, (int)WorldGridHeight);
    }
}