using UnityEngine;

namespace LDtkUnity
{
    public partial class IntGridValueInstance
    {
        public Vector2Int UnityCellCoord(int cellSize)
        {
            int yCellPos = Mathf.FloorToInt((float) CoordId / cellSize);
            int xCellPos = (int) CoordId - yCellPos * cellSize;
            return new Vector2Int(xCellPos, yCellPos);
        }
    }
}