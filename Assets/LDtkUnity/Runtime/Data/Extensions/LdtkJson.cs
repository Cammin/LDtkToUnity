using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Root
    /// </summary>
    public partial class LdtkJson
    {
        /// <summary>
        /// Project background color
        /// </summary>
        [JsonIgnore] public Color UnityBgColor => BgColor.ToColor();
        
        /// <summary>
        /// Default background color of levels
        /// </summary>
        [JsonIgnore] public Color UnityDefaultLevelBgColor => DefaultLevelBgColor.ToColor();

        /// <summary>
        /// Default new level size
        /// </summary>
        [JsonIgnore] public Vector2Int UnityDefaultLevelSize => new Vector2Int((int)DefaultLevelWidth, (int)DefaultLevelHeight);
        
        /// <summary>
        /// Default pivot (0 to 1) for new entities
        /// </summary>
        [JsonIgnore] public Vector2 UnityDefaultPivot => new Vector2((int)DefaultPivotX, (int)DefaultPivotY);
        
        /// <summary>
        /// Size of the world grid in pixels.
        /// </summary>
        [JsonIgnore] public Vector2Int UnityWorldGridSize => new Vector2Int((int)WorldGridWidth, (int)WorldGridHeight);
    }
}