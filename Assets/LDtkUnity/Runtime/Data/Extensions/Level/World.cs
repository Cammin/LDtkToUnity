using System;
using Newtonsoft.Json;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace LDtkUnity
{
    public partial class World : ILDtkIdentifier, ILDtkIid
    {
        /// <value>
        /// Returns true if this neighbour is above the relative level.
        /// </value>
        [JsonIgnore] public Vector2Int UnityWorldGridSize => new Vector2Int((int)WorldGridWidth, (int)WorldGridHeight);

        internal static World FromJsonRoot(LdtkJson json)
        {
            Debug.Assert(json.WorldLayout != null, "json.WorldLayout != null");
            Debug.Assert(json.WorldGridWidth != null, "json.WorldGridWidth != null");
            Debug.Assert(json.WorldGridHeight != null, "json.WorldGridHeight != null");
            
            return new World
            {
                Identifier = "World",
                Iid = string.Empty,
                Levels = json.Levels,
                WorldLayout = json.WorldLayout.Value,
                WorldGridWidth = json.WorldGridWidth.Value,
                WorldGridHeight = json.WorldGridHeight.Value
            };
        }
    }
}