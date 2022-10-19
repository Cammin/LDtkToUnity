using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    internal sealed class LDtkTileArtifactFactory : LDtkArtifactFactory
    {
        public LDtkTileArtifactFactory(AssetImportContext ctx, LDtkArtifactAssets assets, string assetName) : base(ctx, assets, assetName)
        {
        }
        
        public void TryCreateTile() => TryCreateAsset(Artifacts.HasIndexedTile, CreateTile);

        private LDtkArtTile CreateTile()
        {
            Sprite sprite = Artifacts.GetIndexedSprite(AssetName);
            if (sprite == null)
            {
                LDtkDebug.LogError($"Failed to get sprite to create LDtkArtTile; sprite was null for \"{Ctx.assetPath}\"");
                return null;
            }
                
            LDtkArtTile newArtTile = ScriptableObject.CreateInstance<LDtkArtTile>();
            newArtTile.name = AssetName;
            newArtTile._artSprite = sprite;

            return newArtTile;
        }
        
        protected override bool AddArtifactAction(Object obj)
        {
            return Artifacts.AddTile((TileBase)obj);
        }
    }
}