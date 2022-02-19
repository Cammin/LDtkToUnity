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

        public Sprite TryGetOrCreateSprite()
        {
            //if we already cached from a previous operation
            Sprite item = Assets.GetSpriteByName(AssetName);
            if (item != null)
            {
                return item;
            }
            
            //otherwise make a new one
            item = CreateSprite();
            AddAsset(item);
            return item;
        }

        private Sprite CreateSprite()
        {
            Sprite sprite = _slicer.Slice();

            if (sprite == null)
            {
                Debug.LogError("LDtk: Couldn't retrieve a sliced sprite");
                return null;
            }
            
            sprite.name = AssetName;
            return sprite;
        }
    }
}