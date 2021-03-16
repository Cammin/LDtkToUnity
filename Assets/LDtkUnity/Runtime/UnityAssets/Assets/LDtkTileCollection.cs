using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    public class LDtkTileCollection : ScriptableObject
    {
        public const string PROP_TILE_LIST = nameof(_cachedTiles);

        [SerializeField] private Tile[] _cachedTiles = new Tile[0];

        public Tile GetByName(string tileName)
        {
            Tile tile = _cachedTiles.FirstOrDefault(p => p.name.Equals(tileName));
            if (tile != null)
            {
                return tile;
            }
            
            Debug.LogError($"No tile named \"{tileName}\" found in \"{name}\"", this);
            return null;
        }
    }
}