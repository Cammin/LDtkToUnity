using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class TilesetDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// Image size in pixels
        /// </summary>
        [JsonIgnore] public Vector2Int UnityPxSize => new Vector2Int((int)PxWid, (int)PxHei);
    }
}