// ReSharper disable InconsistentNaming

using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDataProjectExtensions
    {
        public static Color BgColor(this LDtkDataProject data) => data.bgColor.ToColor();
        public static Color DefaultLevelBgColor(this LDtkDataProject data) => data.defaultLevelBgColor.ToColor();
        public static Vector2 DefaultPivot(this LDtkDataProject data) => new Vector2(data.defaultPivotX, data.defaultPivotY);
        public static Vector2Int WorldGridSize(this LDtkDataProject data) => new Vector2Int(data.worldGridWidth, data.worldGridHeight);
    }
}