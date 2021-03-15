using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    /*public class LDtkIntGridValueFactory
    {
        private readonly Dictionary<string, Tile> _cachedTiles = new Dictionary<string, Tile>();
        
        public Tile GetTile(Sprite spriteAsset, Color color)
        {
            string name = spriteAsset.name;
        
            if (_cachedTiles.ContainsKey(name))
            {
                return _cachedTiles[name];
            }

            Tile newTile = MakeTile(spriteAsset, color, name);
            _cachedTiles.Add(name, newTile);
            return newTile;
        }

        private static Tile MakeTile(Sprite sprite, Color color, string name)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.colliderType = sprite.GetPhysicsShapeCount() == 0 ? Tile.ColliderType.None : Tile.ColliderType.Sprite;
            tile.sprite = sprite;
            tile.color = color;
            tile.name = name;
            return tile;
        }
    }*/
}