using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public delegate Tile TileCreationAction(Sprite sprite);
    
    public static class LDtkTileFactory
    {
        /*private const string DEFAULT_TILE_PATH = "LDtkDefaultTile";
        public static Tile GetDefaultEmptyTile()
        {
            return Resources.Load<Tile>(DEFAULT_TILE_PATH);
        }*/
        
        public static Tile[] GenerateTilesForSprites(Sprite[] sprites, TileCreationAction tileCreationAction)
        {
            if (sprites == null || sprites.Length == 0)
            {
                Debug.LogError("Sprite array is null");
                return null;
            }

            return sprites.Select(tileCreationAction.Invoke).ToArray();
        }
    }

    
}