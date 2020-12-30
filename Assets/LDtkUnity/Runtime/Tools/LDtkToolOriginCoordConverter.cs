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
        
        public static Vector2Int ConvertCell(Vector2Int cellPos, int verticalCellCount) => TopLeft2BottomLeftOriginCoord(cellPos, verticalCellCount, 1);
        public static Vector2Int ConvertPixel(Vector2Int pixelPos, int totalPixelHeight, int pixelsPerUnit) => TopLeft2BottomLeftOriginCoord(pixelPos, totalPixelHeight, pixelsPerUnit);
        public static Vector2 ConvertPixelToWorldPosition(Vector2Int pixelPos, int totalPixelHeight, int pixelsPerUnit) => (Vector2)ConvertPixel(pixelPos, totalPixelHeight, pixelsPerUnit) / pixelsPerUnit;



        private static Vector2Int LevelOriginPos(Vector2Int pos, int lvlHeight)
        {
            return new Vector2Int
            {       
                x = pos.x,
                y = -pos.y - lvlHeight
            };
        }
        
        //Input converted 
        private static Vector2Int RelativeLevelPosition(Vector2Int pos, int lvlHeight)
        {
            return new Vector2Int
            {       
                x = pos.x,
                y = -pos.y
            };
        }
        
        private static Vector2Int ConvertLevelPosition(Vector2Int pos, int lvlHeight)
        {
            return new Vector2Int
            {       
                x = pos.x,
                y = -pos.y - lvlHeight
            };
        }
        
        
        private static Vector2Int TopLeft2BottomLeftOriginCoord(Vector2Int pos, int totalHeight, int unitSize)
        {
            return new Vector2Int
            {       
                x = pos.x,
                y = totalHeight - pos.y - unitSize
            };
        }
        
        
        
    }
}