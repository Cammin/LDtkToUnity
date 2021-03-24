using UnityEngine;

namespace LDtkUnity
{
    public partial class TilesetDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// Image size in pixels
        /// </summary>
        public Vector2Int UnityPxSize => new Vector2Int((int)PxWid, (int)PxHei);
    }
}