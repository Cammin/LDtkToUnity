using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal class LDtkTileArtifactFactory : LDtkArtifactFactory
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
                LDtkDebug.LogError($"Failed to get sprite to create LDtkArtTile; sprite was null for \"{AssetName}\"");
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