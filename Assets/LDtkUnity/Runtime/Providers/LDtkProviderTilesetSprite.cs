using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Runtime.Providers
{
    public static class LDtkProviderTilesetSprite
    {
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

        private static Sprite CreateSprite(Sprite tileset, Rect rect, float pixelsPerUnit)
        {
            Vector2 pivot = Vector2.one / 2;
            Sprite sprite = Sprite.Create(tileset.texture, rect, pivot, pixelsPerUnit, 0, SpriteMeshType.FullRect);
            
            return sprite;
        }
    }
}