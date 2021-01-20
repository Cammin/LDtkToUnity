// ReSharper disable InconsistentNaming

using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public partial class Level : ILDtkUid, ILDtkIdentifier
    {
        public Color UnityBgColor => BgColor.ToColor();
        public Vector2Int PxSize => new Vector2Int((int)PxWid, (int)PxHei);
        public Vector2Int WorldCoord => new Vector2Int((int)WorldX, (int)WorldY);
        
        public Bounds LevelBounds(int pixelsPerUnit) => new Bounds(new Vector3(WorldX, WorldY, 0), new Vector3(PxWid, PxHei, 0) * pixelsPerUnit);

        public Vector2 UnityWorldCoord(int pixelsPerUnit) => LDtkToolOriginCoordConverter.LevelPosition(WorldCoord, (int)PxHei, pixelsPerUnit);
    }
}
