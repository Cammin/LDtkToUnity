using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public static class LDtkTileFactory
    {
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