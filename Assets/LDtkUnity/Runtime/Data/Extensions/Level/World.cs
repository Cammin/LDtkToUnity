using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class World : ILDtkIdentifier, ILDtkIid
    {
        /// <value>
        /// Default new level size.
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityDefaultLevelSize => new Vector2Int(DefaultLevelWidth, DefaultLevelHeight);
        
        /// <value>
        /// Size of the world grid in pixels.
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityWorldGridSize => new Vector2Int(WorldGridWidth, WorldGridHeight);
    }
}