using UnityEngine;

namespace LDtkUnity
{
    public partial class TilesetDefinition : ILDtkUid, ILDtkIdentifier
    {
        public Vector2Int UnityPxSize => new Vector2Int((int)PxWid, (int)PxHei);
    }
}