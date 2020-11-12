using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.Builders
{
    public static class LDtkBuilderTile
    {
        public static void BuildTileLayerInstances(LDtkDataTile[] tiles,  LDtkTilesetCollection tilesets)
        {
            //Tilemap tileMap = null;
            //Sprite tilesetSprite = tileset.Asset;
            
            
            foreach (LDtkDataTile tile in tiles)
            {
                bool flipX = tile.FlipX;
                bool flipY = tile.FlipY;
                
                

                Tile cosmeticTile = ScriptableObject.CreateInstance<Tile>();
                cosmeticTile.colliderType = Tile.ColliderType.None;
                
                //TODO make a tile here
               //Sprite tileSprite = TileSpriteGetter.GetSpriteFromTileID(tilesetSprite.texture, );
                
                
                //cosmeticTile.sprite = 
            }
        }
        
        private static Texture2D GetLDtkTilesetTexture(LDtkDataTile tile)
        {
            return null;
        }
        
        
    }
}
