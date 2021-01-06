using UnityEngine;

namespace LDtkUnity.Tools
{
    //LDtk's coordinate system origin is based around the top-left. Convert that in order to be relative to Unity's (0, 0) coordinate system.
    public static class LDtkToolOriginCoordConverter
    {
        public static Vector2Int IntGridCoordID(int coordId, int cellSize)
        {
            int yCellPos = Mathf.FloorToInt((float)coordId / cellSize);
            int xCellPos = coordId - yCellPos * cellSize;
            return new Vector2Int(xCellPos, yCellPos);
        }
        

        public static Vector2Int ConvertCell(Vector2Int cellPos, int verticalCellCount)
        {
            cellPos = NegateY(cellPos);
            cellPos.y += verticalCellCount - 1;
            return cellPos;
        }
        
        //todo give this the level origin to anchor from
        public static Vector2 ConvertParsedValue(Vector2 relativeOrigin, Vector2Int cellPos, int lvlCellHeight)
        {
            cellPos = NegateY(cellPos);
            cellPos.y += lvlCellHeight - 1;
            Vector2 extraHalfUnit = Vector2.one * 0.5f;
            return relativeOrigin + cellPos + extraHalfUnit;
        }
        
        
        public static Vector2 LevelPosition(Vector2Int pixelPos, int pixelHeight, int pixelsPerUnit)
        {
            pixelPos += Vector2Int.up * pixelHeight;
            return (Vector2)NegateY(pixelPos) / pixelsPerUnit;
        }
        public static Vector2 EntityLocalPosition(Vector2Int pixelPos, int pixelHeight, int pixelsPerUnit)
        {
            pixelPos += Vector2Int.down * pixelHeight;
            return (Vector2)NegateY(pixelPos) / pixelsPerUnit;
        }

        public static Vector2Int ImageSliceCoord(Vector2Int pos, int textureHeight, int pixelsPerUnit)
        {
            pos = NegateY(pos);
            pos.y += textureHeight - pixelsPerUnit;
            return pos;
        }


        private static Vector2Int NegateY(Vector2Int pos)
        {
            return new Vector2Int
            {       
                x = pos.x,
                y = - pos.y
            };
        }
    }
}