// ReSharper disable InconsistentNaming

using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDataProjectExtensions
    {
        public static Color UnityBgColor(this LdtkJson data) => data.BgColor.ToColor();
        public static Color UnityDefaultLevelBgColor(this LdtkJson data) => data.DefaultLevelBgColor.ToColor();
        public static Vector2 UnityDefaultPivot(this LdtkJson data) => new Vector2((int)data.DefaultPivotX, (int)data.DefaultPivotY);
        public static Vector2Int UnityWorldGridSize(this LdtkJson data) => new Vector2Int((int)data.WorldGridWidth, (int)data.WorldGridHeight);
    }
}