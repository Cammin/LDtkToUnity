// ReSharper disable InconsistentNaming

using LDtkUnity.Data;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity
{
    public partial class Level : ILDtkUid, ILDtkIdentifier
    {
        public Color UnityBgColor => BgColor.ToColor();
        public Vector2Int PxSize => new Vector2Int((int)PxWid, (int)PxHei);
        public Vector2Int WorldCoord => new Vector2Int((int)WorldX, (int)WorldY);
        
        public Bounds UnityWorldBounds(int pixelsPerUnit)
        {
            Vector3 size = new Vector3(PxWid, PxHei, 0) / pixelsPerUnit;
            return new Bounds((Vector3)UnityWorldCoord(pixelsPerUnit) + size/2, size);
        }

        public Vector2 UnityWorldCoord(int pixelsPerUnit) => LDtkToolOriginCoordConverter.LevelPosition(WorldCoord, (int)PxHei, pixelsPerUnit);
    }
}
