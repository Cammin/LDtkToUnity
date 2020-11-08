using System;
using System.Collections;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Tileset
{
    public static class LDtkTileBuilder
    {
        public static void BuildTileLayerInstances(LDtkDataTileInstance[] tiles,  LDtkTileset tileset)
        {
            //Tilemap tileMap = null;
            Sprite tilesetSprite = tileset.Asset;
            
            foreach (LDtkDataTileInstance tile in tiles)
            {
                
                Bool2 flips = GetTileFlips(tile.f);

                Tile cosmeticTile = ScriptableObject.CreateInstance<Tile>();
                cosmeticTile.colliderType = Tile.ColliderType.None;
                
                //TODO make a tile here somehow
               //Sprite tileSprite = TileSpriteGetter.GetSpriteFromTileID(tilesetSprite.texture, );
                
                
                //cosmeticTile.sprite = 
            }
        }
        
        private static Texture2D GetLEdTilesetTexture(LDtkDataTileInstance tile)
        {
            return null;
        }
        
        private static Bool2 GetTileFlips(int fValue)
        {
            byte[] bytes = BitConverter.GetBytes(fValue);
            BitArray array = new BitArray(bytes);
            bool flipX = array.Get(0);
            bool flipY = array.Get(1);
            return new Bool2(flipX, flipY);
        }
    }
}
