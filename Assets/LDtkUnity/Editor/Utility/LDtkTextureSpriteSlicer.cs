using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkTextureSpriteSlicer
    {
        private readonly Texture2D _texture;
        private readonly int _pixelsPerUnit;
        private readonly Rect _srcRect;
        private readonly Vector2 _pivot;

        public Rect ImageSlice => _srcRect; //used by entity icon factory
        
        public LDtkTextureSpriteSlicer(Texture2D texture, Rect srcRect, int ppu, Vector2 pivot)
        {
            if (texture == null)
            {
                LDtkDebug.LogError("Issue constructing LDtkTextureSpriteSlicer, the source texture was null.");
                return;
            }
            
            _texture = texture;
            _pixelsPerUnit = ppu;
            _pivot = pivot;

            _srcRect = LDtkCoordConverter.ImageSlice(srcRect, _texture.height);
        }

        public Sprite Slice()
        {
            if (_texture == null)
            {
                LDtkDebug.LogError("Texture null when trying to slice a sprite");
                return null;
            }

            if (!LDtkCoordConverter.IsLegalSpriteSlice(_texture, _srcRect))
            {
                LDtkDebug.LogError($"Illegal sprite slice: {_srcRect} in {_texture.name}:({_texture.width}, {_texture.height}), Is the pixels per unit value set too big, or is the texture resolution incorrect?", _texture);
                return null;
            }
            
            return LDtkTextureUtility.CreateSprite(_texture, _srcRect, _pivot, _pixelsPerUnit);
        }
    }
}