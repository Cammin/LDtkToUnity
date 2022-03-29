using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class World : ILDtkIdentifier, ILDtkIid
    {
        /// <value>
        /// Default new level size.
        /// </value>
        [JsonIgnore] public Vector2Int UnityDefaultLevelSize => new Vector2Int((int)DefaultLevelWidth, (int)DefaultLevelHeight);
        
        /// <value>
        /// Size of the world grid in pixels.
        /// </value>
        [JsonIgnore] public Vector2Int UnityWorldGridSize => new Vector2Int((int)WorldGridWidth, (int)WorldGridHeight);
    }
}