using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.SCRIPTABLE_OBJECT_TILEMAP_COLLECTION)]
    public class LDtkTileCollection : ScriptableObject
    {
        public const string PROP_TILE_LIST = nameof(_cachedTiles);

        [SerializeField] private Tile[] _cachedTiles = new Tile[0];

        public Tile GetByName(string tileName)
        {
            if (tileName.NullOrEmpty())
            {
                return null;
            }
            
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