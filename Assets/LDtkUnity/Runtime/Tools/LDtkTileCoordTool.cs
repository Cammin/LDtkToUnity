using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkTileCoordTool
    {
        public static Vector2Int GetCellPositionFromCoordID(int coordId, Vector2Int gridCellSize)
        {
            int cellX = Mathf.FloorToInt((float)coordId / gridCellSize.x);
            int cellY = coordId - cellX * gridCellSize.x;

            cellX = CorrectYValue(cellX, gridCellSize.y);
            
            return new Vector2Int(cellY, cellX);
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