using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Root
    /// </summary>
    public partial class LdtkJson
    {
        /// <value>
        /// Project background color
        /// </value>
        [JsonIgnore] public Color UnityBgColor => BgColor.ToColor();
        
        /// <value>
        /// Default background color of levels
        /// </value>
        [JsonIgnore] public Color UnityDefaultLevelBgColor => DefaultLevelBgColor.ToColor();

        /// <value>
        /// Default new level size
        /// </value>
        [JsonIgnore] public Vector2Int UnityDefaultLevelSize => new Vector2Int((int)DefaultLevelWidth, (int)DefaultLevelHeight);
        
        /// <value>
        /// Default pivot (0 to 1) for new entities
        /// </value>
        [JsonIgnore] public Vector2 UnityDefaultPivot => new Vector2((int)DefaultPivotX, (int)DefaultPivotY);
        
        /// <value>
        /// Size of the world grid in pixels.
        /// </value>
        [JsonIgnore] public Vector2Int UnityWorldGridSize => new Vector2Int((int)WorldGridWidth, (int)WorldGridHeight);
    }
}