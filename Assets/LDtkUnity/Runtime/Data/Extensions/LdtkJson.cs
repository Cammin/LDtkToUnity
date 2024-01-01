using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LdtkJson : ILDtkIid
    {
        /// <value>
        /// Project background color
        /// </value>
        [IgnoreDataMember] public Color UnityBgColor => BgColor.ToColor();
        
        /// <value>
        /// Default background color of levels
        /// </value>
        [IgnoreDataMember] public Color UnityDefaultLevelBgColor => DefaultLevelBgColor.ToColor();

        /// <value>
        /// Default new level size
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityDefaultLevelSize => DefaultLevelWidth == null || DefaultLevelHeight == null ? Vector2Int.zero : new Vector2Int((int)DefaultLevelWidth, (int)DefaultLevelHeight);

        /// <value>
        /// Default size for new entities
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityDefaultEntitySize => new Vector2Int(DefaultEntityWidth, DefaultEntityHeight);
        
        /// <value>
        /// Default pivot (0 to 1) for new entities
        /// </value>
        [IgnoreDataMember] public Vector2 UnityDefaultPivot => new Vector2((int)DefaultPivotX, (int)DefaultPivotY);
        
        /// <value>
        /// Size of the world grid in pixels.
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityWorldGridSize => WorldGridWidth == null || WorldGridHeight == null ? Vector2Int.zero : new Vector2Int(WorldGridWidth.Value, WorldGridHeight.Value);

        /// <value>
        /// Get the worlds, while accounting for the soon-to-be deprecated levels array in this json root if the old level array was populated instead.
        /// How worlds are formulated: https://github.com/deepnight/ldtk/wiki/%5B0.10.0%5D-Multi-worlds
        /// </value>
        [IgnoreDataMember] public World[] UnityWorlds
        {
            get
            {
                if (!Worlds.IsNullOrEmpty())
                {
                    return Worlds;
                }
                
                Debug.Assert(WorldLayout != null, "json.WorldLayout != null");
                Debug.Assert(WorldGridWidth != null, "json.WorldGridWidth != null");
                Debug.Assert(WorldGridHeight != null, "json.WorldGridHeight != null");
                Worlds = new World[] { new World 
                {
                    Identifier = "World",
                    Iid = DummyWorldIid,
                    Levels = Levels,
                    WorldLayout = WorldLayout.Value,
                    WorldGridWidth = WorldGridWidth.Value,
                    WorldGridHeight = WorldGridHeight.Value
                }};
                Levels = Array.Empty<Level>();
                return Worlds;
            }
        }
    }
}