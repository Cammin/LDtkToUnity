using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkProviderSprite
    {
        private static Dictionary<int, Sprite> _cachedSprites;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Dispose()
        {
            _cachedSprites = null;
        }
        
        public static Sprite GetSpriteFromTileID(Texture2D tex, int tileID, Vector2Int sourcePosition, Vector2Int cellPixelSize)
        {
            //if we already have it from a previous operation
            if (_cachedSprites.ContainsKey(tileID))
            {
                return _cachedSprites[tileID];
            }
            
            Rect spriteRect = new Rect(sourcePosition, cellPixelSize);
            Sprite newSprite = Sprite.Create(tex, spriteRect, Vector2.zero, cellPixelSize.x);
            CacheSprite(tileID, newSprite);
            return newSprite;
        }



        public static void Init()
        {
            _cachedSprites = new Dictionary<int, Sprite>();
        }
        
        private static void CacheSprite(int tileID, Sprite newSprite)
        {
            if (Application.isPlaying) return;
            _cachedSprites.Add(tileID, newSprite);
        }


    }
}