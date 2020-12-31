using UnityEngine;

namespace LDtkUnity.Tools
{
    //LDtk's coordinate system origin is based around the top-left. Convert that in order to be relative to Unity's (0, 0) coordinate system.
    public static class LDtkToolOriginCoordConverter
    {
        public static Vector2Int GetTopLeftOriginCellCoordFromCoordID(int coordId, int cellSize)
        {
            int yCellPos = Mathf.FloorToInt((float)coordId / cellSize);
            int xCellPos = coordId - yCellPos * cellSize;
            return new Vector2Int(xCellPos, yCellPos);
        }
        

        public static Vector2 ConvertLevelPointToWorld(Vector2Int pixelPos, int totalPixelHeight, int pixelsPerUnit) => TopLeft2BottomLeftPixelCoord(pixelPos, totalPixelHeight, pixelsPerUnit)/pixelsPerUnit;
        public static Vector2 ConvertPointToWorld(Vector2Int pixelPos, int pixelsPerUnit) => NegateY(pixelPos, pixelsPerUnit)/pixelsPerUnit;
        
        public static Vector2Int ConvertCell(Vector2Int cellPos, int verticalCellCount) => TopLeft2BottomLeftPixelCoord(cellPos, verticalCellCount, 1);

        private static Vector2Int NegateY(Vector2Int pos, int unitSize)
        {
            return new Vector2Int
            {       
                x = pos.x,
                y = - pos.y
            };
        }
        
        //this should only be used for levels. points should stay out of it
        private static Vector2Int TopLeft2BottomLeftPixelCoord(Vector2Int pos, int totalHeight, int unitSize)
        {
            pos = new Vector2Int
            {       
                x = pos.x,
                y = -pos.y + totalHeight - unitSize
            };
            return pos;
        }
        
        /*
 * For Points:
-get the pixel point.
-add the level height in pixels (downward direction) so that the point is originated by bottom left.
-negate the y value so that it's flipped on the y axis.
-convert it to world space.

 */
        
        
        
    }
}