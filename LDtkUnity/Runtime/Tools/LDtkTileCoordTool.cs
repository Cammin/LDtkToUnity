using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkTileCoordTool
    {
        public static Vector2Int GetCorrectTileCoord(int coordId, Vector2Int gridCellSize)
        {
            int gridBasedY = Mathf.FloorToInt((float)coordId / gridCellSize.x);
            int gridBasedX = coordId - gridBasedY * gridCellSize.x;

            gridBasedY = CorrectYValue(gridBasedY, gridCellSize.y);
            
            return new Vector2Int(gridBasedX, gridBasedY);
        }
        
        public static Vector2Int GetCorrectTileCoord(Vector2Int cellCoord, Vector2Int gridCellSize)
        {
            cellCoord.y = CorrectYValue(cellCoord.y, gridCellSize.y);
            return cellCoord;
        }

        public static Vector2 GetCorrectPixelCoord(Vector2Int pixelPosition, Vector2Int layerSize, int pixelsPerUnit)
        {
            Vector2 pos = (Vector2) pixelPosition / pixelsPerUnit;
            pos.y = CorrectYInstanceValue(pos.y, layerSize.y);
            return pos;
        }

        private static int CorrectYValue(int gridBasedY, int gridSizeY)
        {
            gridBasedY = -gridBasedY + gridSizeY - 1;
            return gridBasedY;
        }
        private static float CorrectYInstanceValue(float gridBasedY, float gridSizeY)
        {
            //TODO this can be written better though alternate functions. kinda hacky.
            return -gridBasedY + gridSizeY;
        }
    }
}