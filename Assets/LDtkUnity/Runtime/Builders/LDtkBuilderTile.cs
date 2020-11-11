using System;
using System.Collections;
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
                
                GetTileFlips(tile.f, out bool flipX, out bool flipY);

                Tile cosmeticTile = ScriptableObject.CreateInstance<Tile>();
                cosmeticTile.colliderType = Tile.ColliderType.None;
                
                //TODO make a tile here somehow
               //Sprite tileSprite = TileSpriteGetter.GetSpriteFromTileID(tilesetSprite.texture, );
                
                
                //cosmeticTile.sprite = 
            }
        }
        
        private static Texture2D GetLDtkTilesetTexture(LDtkDataTile tile)
        {
            return null;
        }
        
        private static void GetTileFlips(int fValue, out bool flipX, out bool flipY)
        {
            byte[] bytes = BitConverter.GetBytes(fValue);
            BitArray array = new BitArray(bytes);
            flipX = array.Get(0);
            flipY = array.Get(1);
        }
    }
}
