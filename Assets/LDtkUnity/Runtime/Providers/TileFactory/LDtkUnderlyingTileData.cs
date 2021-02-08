using System;
using LDtkUnity.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Providers
{
    [Serializable]
    public class LDtkUnderlyingTileData
    {
        private const int PADDING = 1;

        [SerializeField] private string _name;
        [SerializeField] private Tile _tile;
        [SerializeField] private Sprite _spriteSlice;
        [SerializeField] private Texture2D _textureSlice;

        
        public string Key => _name;
        public Tile Tile => _tile;
        

        public LDtkUnderlyingTileData(Texture2D grandTileset, Vector2Int srcRect, int pixelsPerUnit)
        {
            _name = GetName(grandTileset, srcRect, pixelsPerUnit);
            _textureSlice = ContructTexture(grandTileset, srcRect, pixelsPerUnit);
            _spriteSlice = ContructSprite(pixelsPerUnit);
            _tile = ContructTile();
        }

        public static string GetName(Texture2D grandTileset, Vector2Int srcRect, int pixelsPerUnit)
        {
            return $"{grandTileset.name}_{srcRect}_{pixelsPerUnit}";
        }

        private Texture2D ContructTexture(Texture2D grandTileset, Vector2Int srcRect, int pixelsPerUnit)
        {
            Rect sliceArea = GetSrcRectConverted(grandTileset.height, srcRect, pixelsPerUnit);
            Texture2D slicedTex = GetSliceOfTexture(grandTileset, sliceArea);
            Texture2D paddedSlicedTex = CloneTextureWithPadding(slicedTex);
            paddedSlicedTex.name = _name;
            return paddedSlicedTex;
        }

        private Sprite ContructSprite(int pixelsPerUnit)
        {
            Sprite spr = CreatePaddedSprite(_textureSlice, pixelsPerUnit);
            spr.name = _name;
            return spr;
        }

        private Tile ContructTile()
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.colliderType = Tile.ColliderType.None;
            tile.sprite = _spriteSlice;
            tile.name = _name;
            return tile;
        }

        private static Texture2D GetSliceOfTexture(Texture2D sourceTileset, Rect srcRect)
        {
            RectInt slice = new RectInt(
                Mathf.RoundToInt(srcRect.xMin),
                Mathf.RoundToInt(srcRect.yMin),
                Mathf.RoundToInt(srcRect.width),
                Mathf.RoundToInt(srcRect.height));
            
            Texture2D slicedSourceTexture = new Texture2D(slice.width, slice.height, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point, 
                wrapMode = TextureWrapMode.Clamp
            };

            for (int x = 0; x < slicedSourceTexture.width; x++)
            {
                for (int y = 0; y < slicedSourceTexture.height; y++)
                {
                    int sourceX = slice.x + x;
                    int sourceY = slice.y + y;
                    
                    Color sourceColor = sourceTileset.GetPixel(sourceX, sourceY);
                    slicedSourceTexture.SetPixel(x, y, sourceColor);
                }
            }

            slicedSourceTexture.Apply(false, false);
            return slicedSourceTexture;
        }
        
        private static Rect GetSrcRectConverted(int grandTextureHeight, Vector2Int sourceCathodeRayPos, int pixelsPerUnit)
        {
            Debug.Assert(pixelsPerUnit != 0);
            
            sourceCathodeRayPos = LDtkToolOriginCoordConverter.ImageSliceCoord(sourceCathodeRayPos, grandTextureHeight, pixelsPerUnit);
            
            Vector2Int tileUnitSize = Vector2Int.one * pixelsPerUnit;
            return new Rect(sourceCathodeRayPos, tileUnitSize);
        }
        
        private static Texture2D CloneTextureWithPadding(Texture2D sourceTex)
        {
            Texture2D paddedTexture = new Texture2D(
                sourceTex.width + PADDING * 2, 
                sourceTex.height + PADDING * 2,
                TextureFormat.RGBA32, false);

            for (int x = 0; x < paddedTexture.width; x++)
            {
                for (int y = 0; y < paddedTexture.height; y++)
                {
                    Color sourceColor = sourceTex.GetPixel(x-1, y-1);
                    paddedTexture.SetPixel(x, y, sourceColor);
                }
            }

            paddedTexture.filterMode = FilterMode.Point;
            paddedTexture.Apply(true, false);
            return paddedTexture;
        }
        
        private static Sprite CreatePaddedSprite(Texture2D slicedPaddedTex, float pixelsPerUnit)
        {
            Rect slice = new Rect(Vector2.one * PADDING, Vector2.one * pixelsPerUnit);
            Vector2 pivot = Vector2.one * 0.5f;
            
            return Sprite.Create(slicedPaddedTex, slice, pivot, pixelsPerUnit, 0, SpriteMeshType.FullRect);
        }
    }
}