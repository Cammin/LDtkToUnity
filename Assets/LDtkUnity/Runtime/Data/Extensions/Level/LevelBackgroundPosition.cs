using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LevelBackgroundPosition
    {
        public Rect UnityCropRect => CropRect.ToRect();
        public Vector2 UnityScale => Scale.ToVector2();
        public Vector2Int UnityTopLeftPx => TopLeftPx.ToVector2Int();
    }
}
