using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Runtime.Providers
{
    public static class LDtkProviderTilesetSprite
    {
        private const int PADDING = 1;
        
        private static Dictionary<Sprite, Dictionary<Rect, Sprite>> _cachedTilesetSprites;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Dispose()
        {
            _cachedTilesetSprites = null;
        }
        public static void Init()
        {
            _cachedTilesetSprites = new Dictionary<Sprite, Dictionary<Rect, Sprite>>();
        }
        
        public static Sprite GetSpriteFromTilesetAndRect(Sprite tileset, Rect area, float pixelsPerUnit)
        {
            //if we already have it from a previous operation
            if (!_cachedTilesetSprites.ContainsKey(tileset))
            {
                Dictionary<Rect, Sprite> newDict = new Dictionary<Rect, Sprite>();
                _cachedTilesetSprites.Add(tileset, newDict);
            }
            
            if (!_cachedTilesetSprites[tileset].ContainsKey(area))
            {
                Sprite newSprite = CreateSprite(tileset, area, pixelsPerUnit);
                _cachedTilesetSprites[tileset].Add(area, newSprite);
            }
            
            return _cachedTilesetSprites[tileset][area];
        }
        
        private static Sprite CreateSprite(Sprite sourceTileset, Rect rect, float pixelsPerUnit)
        {
            RectInt intRect = new RectInt(
                Mathf.RoundToInt(rect.xMin),
                Mathf.RoundToInt(rect.yMin),
                Mathf.RoundToInt(rect.width),
                Mathf.RoundToInt(rect.height));
            
            Texture2D slicedSourceTexture = GetSingleTileTexture(sourceTileset, intRect);
            Texture2D paddedTexture = TextureWithPadding(slicedSourceTexture);

            Rect slice = new Rect(Vector2.one * PADDING, Vector2.one * pixelsPerUnit);
            Vector2 pivot = Vector2.one * 0.5f;
            Sprite sprite = Sprite.Create(paddedTexture, slice, pivot, pixelsPerUnit, 0, SpriteMeshType.FullRect);

            return sprite;
        }

        private static Texture2D GetSingleTileTexture(Sprite sourceTileset, RectInt slice)
        {
            Texture2D slicedSourceTexture = new Texture2D(slice.width, slice.height, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point, 
                wrapMode = TextureWrapMode.Clamp
            };

            for (int x = 0; x < slicedSourceTexture.width; x++)
            {
                int sourceX = slice.x + x;
                for (int y = 0; y < slicedSourceTexture.height; y++)
                {
                    int sourceY = slice.y + y;
                    Color sourceColor = sourceTileset.texture.GetPixel(sourceX, sourceY);
                    slicedSourceTexture.SetPixel(x, y, sourceColor);
                }
            }

            slicedSourceTexture.Apply(false, false);
            return slicedSourceTexture;
        }
        
        private static Texture2D TextureWithPadding(Texture2D sourceTex)
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
    }
}