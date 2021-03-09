using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkTileFactory
    {
        public static Tile[] GenerateTilesForTextureMetas(Texture2D tex)
        {
            Sprite[] metaSprites = GetMetaSpritesOfTexture(tex);
            return GenerateTilesFromSprites(metaSprites);
        }
        
        private static Tile[] GenerateTilesFromSprites(Sprite[] sprites)
        {
            if (sprites == null || sprites.Length == 0)
            {
                Debug.LogError("Sprite array is null");
                return null;
            }

            return sprites.Select(ContructTile).ToArray();
        }

        private static Tile ContructTile(Sprite sprite)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.colliderType = Tile.ColliderType.None;
            tile.sprite = sprite;
            tile.name = LDtkTilesetSpriteKeyFormat.GetKeyFormat(sprite.texture, sprite.rect.position);
            return tile;
        }
        
        private static Sprite[] GetMetaSpritesOfTexture(Texture2D spriteSheet)
        {
            if (spriteSheet == null)
            {
                Debug.LogError("Texture2D null");
                return null;
            }
            
            string spriteSheetPath = AssetDatabase.GetAssetPath(spriteSheet);
            return AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath).OfType<Sprite>().ToArray();
        }
    }
}