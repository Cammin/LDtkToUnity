using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkTextureSpriteSlicer
    {
        private readonly Texture2D _texture;
        private readonly int _gridSize;
        
        public LDtkTextureSpriteSlicer(Texture2D texture, int ppu)
        {
            _texture = texture;
            _gridSize = ppu;

        }

        public Sprite CreateSpriteSliceForPosition(Vector2Int ldtkPos)
        {
            Vector2Int realPos = LDtkToolOriginCoordConverter.ImageSliceCoord(ldtkPos, _texture.height, _gridSize);
            
            Rect srcRect = new Rect(realPos, Vector2.one * _gridSize);

            Vector2 pivot = Vector2.one * 0.5f;

            if (!IsLegalSpriteSlice(_texture, srcRect))
            {
                Debug.LogError($"Illegal sprite slice: {srcRect}, Is the pixels per unit value set too big?");
                return null;
            }
            
            Sprite sprite = Sprite.Create(_texture, srcRect, pivot, _gridSize);
            
            return sprite;
                    
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