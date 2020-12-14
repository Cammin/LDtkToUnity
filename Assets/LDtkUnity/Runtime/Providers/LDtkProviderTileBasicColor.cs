using System.Collections.Generic;
using LDtkUnity.UnityAssets;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Providers
{
    public static class LDtkProviderTileBasicColor
    {
        private static Dictionary<string, Tile> _cachedTiles;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Dispose()
        {
            _cachedTiles = null;
        }
        public static void Init()
        {
            _cachedTiles = new Dictionary<string, Tile>();
        }

        public static Tile GetTile(LDtkIntGridValueAsset asset, Color color)
        {
            string name = asset.name;
        
            if (_cachedTiles.ContainsKey(name))
            {
                return _cachedTiles[name];
            }

            Tile newTile = MakeTile(asset.ReferencedAsset, color);
            _cachedTiles.Add(name, newTile);
            return newTile;
        }

        private static Tile MakeTile(Sprite sprite, Color color)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.colliderType = sprite.GetPhysicsShapeCount() == 0 ? Tile.ColliderType.None : Tile.ColliderType.Sprite;
            tile.sprite = sprite;
            tile.color = color;
            return tile;
        }
    }
}