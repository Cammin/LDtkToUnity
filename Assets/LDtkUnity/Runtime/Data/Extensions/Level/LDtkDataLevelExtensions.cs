// ReSharper disable InconsistentNaming

using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDataLevelExtensions
    {
        public static Color BgColor(this Level data) => data.BgColor.ToColor();
        public static Vector2Int PxSize(this Level data) => new Vector2Int((int)data.PxWid, (int)data.PxHei);
        public static Vector2Int WorldCoord(this Level data) => new Vector2Int((int)data.WorldX, (int)data.WorldY);
        
        public static Bounds LevelBounds(this Level data, int pixelsPerUnit)
        {
            return new Bounds(new Vector3(data.WorldX, data.WorldY, 0),
                new Vector3(data.PxWid, data.PxHei, 0) * pixelsPerUnit);
        }

        public static Vector2 UnityWorldCoord(this Level data, int pixelsPerUnit)
        {
            return LDtkToolOriginCoordConverter.LevelPosition(data.WorldCoord(), (int)data.PxHei, pixelsPerUnit);
        }
    }
}
