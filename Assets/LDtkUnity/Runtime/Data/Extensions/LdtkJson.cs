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
        [JsonIgnore] public Vector2Int UnityWorldGridSize => WorldGridWidth == null || WorldGridHeight == null ? Vector2Int.zero : new Vector2Int((int)WorldGridWidth.Value, (int)WorldGridHeight.Value);

        /// <value>
        /// Get the worlds, while accounting for the soon-to-be deprecated levels array in this json root if the old level array was populated instead.
        /// </value>
        [JsonIgnore] public World[] UnityWorlds => Worlds.IsNullOrEmpty() ? new World[] { DeprecatedWorld } : Worlds;

        // How worlds are formulated: https://github.com/deepnight/ldtk/wiki/%5B0.10.0%5D-Multi-worlds
        [JsonIgnore] public World DeprecatedWorld
        {
            get
            {
                Debug.Assert(WorldLayout != null, "json.WorldLayout != null");
                Debug.Assert(WorldGridWidth != null, "json.WorldGridWidth != null");
                Debug.Assert(WorldGridHeight != null, "json.WorldGridHeight != null");

                return new World
                {
                    Identifier = "World",
                    Iid = string.Empty, //todo understand if this is the right iid to choose for world references
                    Levels = Levels,
                    WorldLayout = WorldLayout.Value,
                    WorldGridWidth = WorldGridWidth.Value,
                    WorldGridHeight = WorldGridHeight.Value
                };
            }
        }
        
    }
}