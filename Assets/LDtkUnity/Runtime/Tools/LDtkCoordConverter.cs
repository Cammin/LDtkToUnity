using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// LDtk's coordinate system origin is based around the top-left. Convert that in order to be relative to Unity's bottom-left coordinate system.
    /// </summary>
    internal static class LDtkCoordConverter
    {
        public static Vector2Int IntGridValueCsvCoord(int csvIndex, Vector2Int cellSize)
        {
            int index = 0;
                
            for (int y = 0; y < cellSize.y; y++)
            {
                for (int x = 0; x < cellSize.x; x++)
                {
                    if (index == csvIndex)
                    {
                        return new Vector2Int(x, y);
                    }
                    index++;
                }
            }

            LDtkDebug.LogError("Failed to get CSV coord");
            return Vector2Int.zero;
        }

        public static Vector2Int ConvertCell(Vector2Int cellPos, int verticalCellCount)
        {
            cellPos = NegateY(cellPos);
            cellPos.y += verticalCellCount - 1;
            return cellPos;
        }
        
        public static Vector2 ConvertParsedPointValue(Vector2Int cellPos, PointParseData data)
        {
            float scaleFactor = (data.GridSize / (float)data.PixelsPerUnit);
            //Debug.Log($"scale {scaleFactor}");
            
            cellPos = NegateY(cellPos);
            cellPos.y += data.LvlCellHeight - 1;
            
            Vector2 extraHalfUnit = new Vector2(0.5f, 0.5f);
            Vector2 totalOffset = (cellPos + extraHalfUnit) * scaleFactor;
            return data.LevelPosition + totalOffset;
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
        
        public static Vector2 EntityPivotOffset(Vector2 pivot, Vector2 size)
        {
            Vector2 halfUnit = Vector2.one * 0.5f;
            Vector2 properPivot = pivot - halfUnit;
            Vector2 pivotSize = size * properPivot;
            Vector2 offset = Vector2.right * pivotSize.x * -2;
            return pivotSize + offset;
        }

        //always float based; the source background slice is composed of floats
        public static Vector2 LevelBackgroundImagePosition(Vector2 topLeftPx, int pixelsPerUnit, int levelPxHei)
        {
            topLeftPx = NegateY(topLeftPx);
            topLeftPx += Vector2.up * (levelPxHei);
            return topLeftPx / pixelsPerUnit;
        }
        
        public static Rect ImageSlice(Rect pos, int textureHeight)
        {
            pos.position = NegateY(pos.position);
            pos.y += textureHeight - pos.height;
            return pos;
        }
        public static Vector2 LevelBackgroundImageSliceCoord(Vector2 pos, int textureHeight, float sliceHeight)
        {
            pos = NegateY(pos);
            pos.y += textureHeight - sliceHeight;
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
        private static Vector2 NegateY(Vector2 pos)
        {
            return new Vector2
            {       
                x = pos.x,
                y = - pos.y
            };
        }
        
        public static bool IsLegalSpriteSlice(Texture2D tex, Rect rect)
        {
            if (rect.x < 0 || rect.x + Mathf.Max(0, rect.width) > tex.width + 0.001f)
            {
                return false;
            }
            
            if (rect.y < 0 || rect.y + Mathf.Max(0, rect.height) > tex.height + 0.001f)
            {
                return false;
            }

            return true;
        }
    }
}