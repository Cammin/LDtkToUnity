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
        
        public TileBase TryGetOrCreateTile()
        {
            //if we already cached from a previous operation
            TileBase tile = Assets.GetTileByName(AssetName);
            if (tile != null)
            {
                return tile;
            }

            tile = CreateTile();
            AddAsset(tile); 
            return tile;
        }

        private LDtkArtTile CreateTile()
        {
            LDtkArtTile newArtTile = ScriptableObject.CreateInstance<LDtkArtTile>();
            Sprite sprite = _spriteFactory.TryGetOrCreateSprite();

            if (sprite == null)
            {
                LDtkDebug.LogError("Failed to get sprite to create LDtkArtTile; sprite was null");
                return null;
            }
                
            newArtTile.name = AssetName;
            newArtTile._artSprite = sprite;

            return newArtTile;
        }
    }
}