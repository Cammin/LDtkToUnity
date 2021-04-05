using UnityEngine;

namespace LDtkUnity
{
    public static class LDtkResourcesLoader
    {
        private const string GRID_PREFAB_PATH = "LDtkDefaultGrid";
        private const string TILE_PREFAB_PATH = "LDtkDefaultSquare";
        public static GameObject LoadDefaultGridPrefab()
        {
            return Resources.Load<GameObject>(GRID_PREFAB_PATH);
        }

        public static Sprite LoadDefaultTileSprite()
        {
            return Resources.Load<Sprite>(TILE_PREFAB_PATH);
        }
    }
}