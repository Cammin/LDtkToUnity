using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkTextureSpriteSlicer
    {
        private readonly Texture2D _texture;
        private readonly int _pixelsPerUnit;
        private readonly RectInt _srcRect;
        
        public LDtkTextureSpriteSlicer(Texture2D texture, RectInt srcRect, int ppu)
        {
            _texture = texture;
            _pixelsPerUnit = ppu;
            
            _srcRect = LDtkCoordConverter.ImageSlice(srcRect, _texture.height);
        }
        
        public Sprite Slice()
        {
            Rect rect = _srcRect.ToRect();
            if (!LDtkCoordConverter.IsLegalSpriteSlice(_texture, rect))
            {
                Debug.LogError($"LDtk: Illegal sprite slice: {_srcRect} for {_texture.name}, Is the pixels per unit value set too big, or is the texture resolution incorrect?");
                return null;
            }
            
            Vector2 pivot = Vector2.one * 0.5f;
            Sprite sprite = Sprite.Create(_texture, rect, pivot, _pixelsPerUnit);

            return sprite;
        }
    }
}