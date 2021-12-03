using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public struct PointParseData
    {
        public Vector2 LevelPosition;
        public int LvlCellHeight;
        public int PixelsPerUnit;
        public int GridSize;
    }
}