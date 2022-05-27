using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal class LDtkTileArtifactFactory : LDtkArtifactFactory
    {
        private readonly LDtkSpriteArtifactFactory _spriteFactory;
        
        public LDtkTileArtifactFactory(LDtkProjectImporter importer, LDtkArtifactAssets assets, LDtkSpriteArtifactFactory spriteFactory, string assetName) : base(importer, assets, assetName)
        {
            _spriteFactory = spriteFactory;
        }
        
        public TileBase TryGetTile() => TryGetAsset(Artifacts.GetIndexedTile);
        public bool TryCreateTile() => TryCreateAsset(Artifacts.HasIndexedTile, CreateTile);

        private LDtkArtTile CreateTile()
        {
            Sprite sprite = _spriteFactory.TryGetSprite();
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
    }
}