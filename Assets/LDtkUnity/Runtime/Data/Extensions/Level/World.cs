using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class World : ILDtkIdentifier, ILDtkIid
    {
        /// <value>
        /// Returns true if this neighbour is above the relative level.
        /// </value>
        [JsonIgnore] public Vector2Int UnityWorldGridSize => new Vector2Int((int)WorldGridWidth, (int)WorldGridHeight);
    }
}