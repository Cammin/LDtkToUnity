using System;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LdtkJson
    {
        public Color UnityBgColor => BgColor.ToColor();
        public Color UnityDefaultLevelBgColor => DefaultLevelBgColor.ToColor();
        public Vector2 UnityDefaultPivot => new Vector2((int)DefaultPivotX, (int)DefaultPivotY);
        public Vector2Int UnityWorldGridSize => new Vector2Int((int)WorldGridWidth, (int)WorldGridHeight);
    }
}