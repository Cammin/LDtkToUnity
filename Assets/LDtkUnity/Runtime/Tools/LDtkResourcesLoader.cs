using UnityEngine;

namespace LDtkUnity
{
    public static class LDtkResourcesLoader
    {
        private const string TILE_PREFAB_PATH = "LDtkDefaultSquare";
        
        public static Sprite LoadDefaultTileSprite()
        {
            return Resources.Load<Sprite>(TILE_PREFAB_PATH);
        }
    }
}