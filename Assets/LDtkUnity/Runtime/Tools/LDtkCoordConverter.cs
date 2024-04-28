using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// LDtk's coordinate system origin is based around the top-left. Convert that in order to be relative to Unity's bottom-left coordinate system.
    /// </summary>
    internal static class LDtkCoordConverter
    {
        public static Vector3Int IntGridValueCsvCoord(int csvIndex, Vector2Int cellSize)
        {
            int index = 0;
                
            for (int y = 0; y < cellSize.y; y++)
            {
                for (int x = 0; x < cellSize.x; x++)
                {
                    if (index == csvIndex)
                    {
                        return new Vector3Int(x, y, 0);
                    }
                    index++;
                }
            }

            LDtkDebug.LogError("Failed to get CSV coord");
            return Vector3Int.zero;
        }
        
        /// <summary>
        /// This tries iterating over every possible coordinate to track down a tileset index.
        /// Is only able to check for tiles that are the same size as GridSize, no additional sprites.
        /// </summary>
        public static int TilesetSliceIndex(Rect rect, TilesetDefinition def)
        {
            int gridSize = def.TileGridSize;
            
            int rectX = (int)rect.x;
            int rectY = (int)rect.y;
            int rectW = (int)rect.width;
            int rectH = (int)rect.height;
            
            if (rectW != rectH)
            {
                return -1;
            }

            //dont need to check height because of above
            if (rectW != gridSize)
            {
                return -1;
            }
            
            int i = 0;
            
            for (int y = 0; y < def.PxHei; y += def.TileGridSize)
            {
                for (int x = 0; x < def.PxWid; x += def.TileGridSize)
                {
                    if (rectX == x && rectY == y)
                    {
                        return i;
                    }
                    i++;
                }
            }

            return -1;
        }

        public static Vector3Int ConvertCell(Vector3Int cellPos, int verticalCellCount)
        {
            cellPos.y = ConvertCell(cellPos.y, verticalCellCount);
            return cellPos;
        }
        public static int ConvertCell(int cellPosY, int verticalCellCount)
        {
            cellPosY = -cellPosY;
            cellPosY += verticalCellCount - 1;
            return cellPosY;
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
        
        public static Rect ImageSlice(Rect pos, int textureHeight)
        {
            pos.y = ImageSliceY(pos.y, pos.height, textureHeight);
            return pos;
        }
        public static int ImageSliceY(int yPos, int height, int textureHeight)
        {
            yPos = -yPos;
            yPos += textureHeight - height;
            return yPos;
        }
        public static float ImageSliceY(float yPos, float height, int textureHeight)
        {
            yPos = -yPos;
            yPos += textureHeight - height;
            return yPos;
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
        
        
        private static Vector3Int NegateY(Vector3Int pos)
        {
            return new Vector3Int
            {       
                x = pos.x,
                y = - pos.y,
                z = 0,
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