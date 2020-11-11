using LDtkUnity.Runtime.Data.Level;
using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkToolBoundsCalculator
    {
        public static Bounds GetLevelBounds(this LDtkDataLevel lvl, int pixelsPerUnit)
        {
            
            
            Vector2Int lvlUnitSize = new Vector2Int(lvl.pxWid, lvl.pxHei) / pixelsPerUnit;
            
            Bounds lvlBounds = new Bounds((Vector2)lvlUnitSize / 2, (Vector3Int)lvlUnitSize);
            return lvlBounds;
        }
    }
}
