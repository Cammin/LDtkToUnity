// ReSharper disable InconsistentNaming

using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDataLevelExtensions
    {
        public static Color BgColor(this LDtkDataLevel data) => data.__bgColor.ToColor();
        public static Vector2Int PxSize(this LDtkDataLevel data) => new Vector2Int(data.pxWid, data.pxHei);
        public static Vector2Int WorldCoord(this LDtkDataLevel data) => new Vector2Int(data.worldX, data.worldY);
        
        public static Bounds LevelBounds(this LDtkDataLevel data, int pixelsPerUnit)
        {
            return new Bounds(new Vector3(data.worldX, data.worldY, 0),
                new Vector3(data.pxWid, data.pxHei, 0) * pixelsPerUnit);
        }

        public static Vector2 UnityWorldCoord(this LDtkDataLevel data, int pixelsPerUnit)
        {
            return LDtkToolOriginCoordConverter.LevelPosition(data.WorldCoord(), data.pxHei, pixelsPerUnit);
        }
    }
}
