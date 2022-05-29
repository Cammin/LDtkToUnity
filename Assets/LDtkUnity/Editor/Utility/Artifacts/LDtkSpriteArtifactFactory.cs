using UnityEditor.AssetImporters;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkSpriteArtifactFactory : LDtkArtifactFactory
    {
        private readonly LDtkTextureSpriteSlicer _slicer;

        public LDtkSpriteArtifactFactory(AssetImportContext ctx, LDtkArtifactAssets artifacts, Texture2D srcTex, Rect srcPos, int pixelsPerUnit, string assetName) : base(ctx, artifacts, assetName)
        {
            _slicer = new LDtkTextureSpriteSlicer(srcTex, srcPos, pixelsPerUnit, new Vector2(0.5f, 0.5f));
        }
        
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

        protected override bool AddArtifactAction(Object obj)
        {
            return Artifacts.AddSprite((Sprite)obj);
        }
    }
}