using UnityEngine;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    internal sealed class LDtkSpriteArtifactFactory : LDtkArtifactFactory
    {
        private readonly LDtkTextureSpriteSlicer _slicer;

        public LDtkSpriteArtifactFactory(AssetImportContext ctx, LDtkArtifactAssets artifacts, Texture2D srcTex, Rect srcPos, int pixelsPerUnit, string assetName) : base(ctx, artifacts, assetName)
        {
            _slicer = new LDtkTextureSpriteSlicer(srcTex, srcPos, pixelsPerUnit, new Vector2(0.5f, 0.5f));//todo this 0.5 should be customizable
        }
        
        public bool TryCreateSprite() => TryCreateAsset(Artifacts.HasIndexedSprite, CreateSprite);

        private Sprite CreateSprite()
        {
            Sprite sprite = _slicer.Slice();

            if (sprite == null)
            {
                LDtkDebug.LogError($"Couldn't retrieve a sliced sprite for \"{Ctx.assetPath}\"");
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