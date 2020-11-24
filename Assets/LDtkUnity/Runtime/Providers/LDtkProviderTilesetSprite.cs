using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Runtime.Providers
{
    public static class LDtkProviderTilesetSprite
    {
        private static Dictionary<Sprite, Dictionary<Rect, Sprite>> _cachedTilesetSprites;

#if UNITY_2019_2_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
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

        //private static List<Texture2D> _runtimeGeneratedTextures = new List<Texture2D>();
        
        private static Sprite CreateSprite(Sprite sourceTileset, Rect rect, float pixelsPerUnit)
        {
            RectInt intRect = new RectInt(
                Mathf.RoundToInt(rect.xMin),
                Mathf.RoundToInt(rect.yMin),
                Mathf.RoundToInt(rect.width),
                Mathf.RoundToInt(rect.height));
            
            Texture2D slicedTexture = new Texture2D(intRect.width, intRect.height, TextureFormat.RGBA32, false);
            slicedTexture.filterMode = FilterMode.Point;
            
            for (int x = 0; x < slicedTexture.width; x++)
            {
                int sourceX = intRect.x + x;
                for (int y = 0; y < slicedTexture.height; y++)
                {
                    int sourceY = intRect.y + y;
                    
                    Color sourceColor = sourceTileset.texture.GetPixel(sourceX, sourceY);
                    //Debug.Log(sourceColor);
                    
                    slicedTexture.SetPixel(x, y, sourceColor);
                }
            }
            slicedTexture.Apply(true, false);
            //_runtimeGeneratedTextures.Add(slicedTexture);
            
            Texture2D paddedTexture = new Texture2D(slicedTexture.width+2, slicedTexture.height+2);
            paddedTexture.filterMode = FilterMode.Point;
            
            for (int x = 0; x < slicedTexture.width; x++)
            {
                bool isLeftFace = x == 0;
                bool isRightFace = x == slicedTexture.width-1;
                
                int sourceX;
                if (isLeftFace)
                {
                    sourceX = 0;
                }
                else if (isRightFace)
                {
                    sourceX = slicedTexture.width - 1;
                }
                else
                {
                    sourceX = x - 1;
                }

                for (int y = 0; y < slicedTexture.height; y++)
                {
                    bool isTopFace = y == slicedTexture.height-1;
                    bool isBottomFace = y == 0;

                    int sourceY;
                    if (isBottomFace)
                    {
                        sourceY = 0;
                    }
                    else if (isTopFace)
                    {
                        sourceY = slicedTexture.height - 1;
                    }
                    else
                    {
                        sourceY = y - 1;
                    }

                    Color sourceColor = slicedTexture.GetPixel(sourceX, sourceY);
                    //Debug.Log(sourceColor);
                    
                    paddedTexture.SetPixel(x, y, sourceColor);
                }
            }
            paddedTexture.Apply(true, false);
            
 
            
            Rect slice = new Rect(Vector2.zero, Vector2.one * pixelsPerUnit+Vector2.one);
            Vector2 pivot = Vector2.one / 2;
            Sprite sprite = Sprite.Create(paddedTexture, slice, pivot, pixelsPerUnit, 0, SpriteMeshType.Tight);

            return sprite;
        }
    }
}