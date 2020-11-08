using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkTileSpriteGetter
    {
        private static Dictionary<int, Sprite> _cachedCreationSprites;

        public static Sprite GetSpriteFromTileID(Texture2D tex, int tileID, Vector2Int srcSize, Vector2Int tileSize)
        {
            //if we already have it from a previous operation
            if (_cachedCreationSprites.ContainsKey(tileID))
            {
                return _cachedCreationSprites[tileID];
            }
            
            Rect spriteRect = new Rect(srcSize, tileSize);
            Sprite newSprite = Sprite.Create(tex, spriteRect, Vector2.zero, tileSize.x);
            _cachedCreationSprites.Add(tileID, newSprite);
            return newSprite;
        }

        public static void Init()
        {
            _cachedCreationSprites = new Dictionary<int, Sprite>();
        }
        public static void Dispose()
        {
            _cachedCreationSprites = null;
        }
    }
}