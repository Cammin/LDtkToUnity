using LDtkUnity.Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkTileFactory
    {
        public static bool GenerateTilesFromTextureSlices(Texture2D tex)
        {
            Sprite[] metaSprites = LDtkMetaSpriteFactory.GetMetaSpritesOfTexture(tex);
            return GenerateTilesFromSprites(metaSprites);
        }
        
        private static bool GenerateTilesFromSprites(Sprite[] sprites)
        {
            if (sprites == null || sprites.Length == 0)
            {
                Debug.LogError("Sprite array is null");
                return false;
            }

            foreach (Sprite sprite in sprites)
            {
                if (!GenerateTileFromSprite(sprite))
                {
                    return false;
                }
            }

            return true;
        }

        public static Tile GenerateTileFromSprite(Sprite sprite)
        {
            string matchingKey = sprite.name;
            
            //todo realize path for assetdatabase. figure out how we get our path, similar to figuring out auto-generation enum.
            //string path = 
            
            //if (AssetDatabase)
        }
        
        private Tile ContructTile(Sprite sprite)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.colliderType = Tile.ColliderType.None;
            tile.sprite = sprite;
            tile.name = LDtkTilesetSpriteKeyFormat.GetKeyFormat(sprite.texture.name, sprite.rect);
            return tile;
        }

        private void SaveTileInstance(Tile tile, string path)
        {
            
            
            if (AssetDatabase.)
        }
}
}