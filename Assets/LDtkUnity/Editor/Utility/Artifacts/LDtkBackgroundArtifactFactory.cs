using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBackgroundArtifactFactory : LDtkArtifactFactory
    {
        private readonly LDtkTextureSpriteSlicer _slicer;
        private readonly string _assetName;

        public LDtkBackgroundArtifactFactory(string assetName, Texture2D srcTex, int pixelsPerUnit, Level lvl)
        {
            Rect rect = lvl.BgPos.UnityCropRect;
            rect.position = LDtkCoordConverter.LevelBackgroundImageSliceCoord(rect.position, srcTex.height, rect.height);

            Vector2 pivot = lvl.UnityBgPivot;
            pivot.y = 1f - pivot.y; 

            _slicer = new LDtkTextureSpriteSlicer(srcTex, rect, pixelsPerUnit, pivot);
            _assetName = assetName;
        }
        
        public Sprite CreateBackground()
        {
            Sprite sprite = _slicer.Slice();
            if (sprite == null)
            {
                return null;
            }
            sprite.name = _assetName;
            return sprite;
        }
    }
}