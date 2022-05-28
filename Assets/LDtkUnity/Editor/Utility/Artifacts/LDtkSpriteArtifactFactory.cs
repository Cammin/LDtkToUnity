using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkSpriteArtifactFactory : LDtkArtifactFactory
    {
        private readonly LDtkTextureSpriteSlicer _slicer;

        public LDtkSpriteArtifactFactory(LDtkProjectImporter importer, LDtkArtifactAssets artifacts, Texture2D srcTex, RectInt srcPos, int pixelsPerUnit, string assetName) : base(importer, artifacts, assetName)
        {
            _slicer = new LDtkTextureSpriteSlicer(srcTex, srcPos, pixelsPerUnit);
        }
        
        public Sprite TryGetSprite() => TryGetAsset(Artifacts.GetIndexedSprite);
        public bool TryCreateSprite() => TryCreateAsset(Artifacts.HasIndexedSprite, CreateSprite);

        private Sprite CreateSprite()
        {
            Sprite sprite = _slicer.Slice();

            if (sprite == null)
            {
                LDtkDebug.LogError("LDtk: Couldn't retrieve a sliced sprite");
                return null;
            }
            
            sprite.name = AssetName;
            return sprite;
        }
    }
}