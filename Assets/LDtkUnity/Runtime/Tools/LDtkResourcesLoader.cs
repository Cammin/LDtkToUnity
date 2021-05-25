using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public static class LDtkResourcesLoader
    {
        private const string SPRITE_PATH = "LDtkDefaultSquare";
        private const string TILE_PATH = "LDtkDefaultTile";
        
        public static Sprite LoadDefaultTileSprite()
        {
            return Resources.Load<Sprite>(SPRITE_PATH);
        }

        public static LDtkIntGridTile LoadDefaultTile()
        {
            return Resources.Load<LDtkIntGridTile>(TILE_PATH);
        }
    }
}