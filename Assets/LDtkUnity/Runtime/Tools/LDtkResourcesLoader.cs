using UnityEngine;

namespace LDtkUnity
{
    internal static class LDtkResourcesLoader
    {
        private const string SPRITE_PATH = "LDtkDefaultSquare";
        private const string TILE_PATH = "LDtkDefaultTile";

        private static Sprite _defaultSprite;
        private static LDtkIntGridTile _defaultTile;
        
        public static Sprite LoadDefaultTileSprite()
        {
            if (_defaultSprite)
            {
                return _defaultSprite;
            }
            _defaultSprite = Resources.Load<Sprite>(SPRITE_PATH);
            return _defaultSprite;
        }

        public static LDtkIntGridTile LoadDefaultTile()
        {
            if (_defaultTile)
            {
                return _defaultTile;
            }
            _defaultTile = Resources.Load<LDtkIntGridTile>(TILE_PATH);
            return _defaultTile;
        }

        public static bool IsDefaultAsset(Object o)
        {
            if (o == null)
            {
                return false;
            }

            if (o is Sprite)
            {
                return _defaultSprite != null && _defaultSprite == o;
            }
            
            if (o is LDtkIntGridTile)
            {
                return _defaultTile != null && _defaultTile == o;
            }

            return false;
        }
    }
}