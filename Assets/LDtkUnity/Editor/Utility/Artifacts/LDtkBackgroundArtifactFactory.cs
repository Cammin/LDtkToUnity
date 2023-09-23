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
            _slicer = new LDtkTextureSpriteSlicer(srcTex, rect, pixelsPerUnit, Vector2.up);
            _assetName = assetName;
        }
        
        public Sprite CreateBackground()
        {
            Sprite sprite = _slicer.Slice();
            if (sprite == null)
            {
                LDtkDebug.LogError($"Couldn't retrieve a sliced sprite for background for \"{_assetName}\"");
                return null;
            }
            sprite.name = _assetName;
            return sprite;
        }
    }
}