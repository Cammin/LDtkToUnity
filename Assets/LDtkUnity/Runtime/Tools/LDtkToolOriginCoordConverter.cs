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
        

        
        public static Vector2 LevelPosition(Vector2Int pixelPos, int pixelHeight, int pixelsPerUnit)
        {
            return NegateY(Up2DownOrigin(pixelPos, pixelHeight)) / pixelsPerUnit;
        }

        private static Vector2Int Up2DownOrigin(Vector2Int pixelPos, int pixelHeight)
        {
            return pixelPos + Vector2Int.up * pixelHeight;
        }

        public static Vector2 EntityPosition(Vector2Int pixelPos, int pixelsPerUnit)
        {
            return NegateY(pixelPos) / pixelsPerUnit;
        }

        public static Vector2Int ConvertCell(Vector2Int cellPos, int verticalCellCount)
        {
            return TopLeft2BottomLeftPixelCoord(cellPos, verticalCellCount, 1);
        }

        private static Vector2Int NegateY(Vector2Int pos)
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
        

        
        public static Vector2Int TopLeft2BottomLeftPixelCoordForTileImages(Vector2Int pos, int textureHeight, int pixelsPerUnit)
        {
            return new Vector2Int
            {       
                x = pos.x,
                y = textureHeight - pos.y - pixelsPerUnit
            };
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