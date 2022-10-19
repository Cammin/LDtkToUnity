using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBackgroundArtifactFactory : LDtkArtifactFactory
    {
        private readonly LDtkTextureSpriteSlicer _slicer;

        public LDtkBackgroundArtifactFactory(AssetImportContext ctx, LDtkArtifactAssets artifacts, string assetName, Texture2D srcTex, int pixelsPerUnit, Level lvl) : base(ctx, artifacts, assetName)
        {
            Rect rect = lvl.BgPos.UnityCropRect;
            rect.position = LDtkCoordConverter.LevelBackgroundImageSliceCoord(rect.position, srcTex.height, rect.height);
            _slicer = new LDtkTextureSpriteSlicer(srcTex, rect, pixelsPerUnit, Vector2.up);
        }
        
        public bool TryCreateBackground() => TryCreateAsset(Artifacts.HasIndexedBackground, CreateBackground);

        private Sprite CreateBackground()
        {
            Sprite sprite = _slicer.Slice();

            if (sprite == null)
            {
                LDtkDebug.LogError($"Couldn't retrieve a sliced sprite for background for \"{Ctx.assetPath}\"");
                return null;
            }
            
            sprite.name = AssetName;
            return sprite;
        }

        protected override bool AddArtifactAction(Object obj)
        {
            return Artifacts.AddBackground((Sprite)obj);
        }
    }
}