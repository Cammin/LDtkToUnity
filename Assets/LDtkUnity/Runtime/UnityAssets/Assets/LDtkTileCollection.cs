using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    //we do not use create asset, we create this from 
    //[CreateAssetMenu(fileName = nameof(LDtkTileCollection), menuName = LDtkToolScriptableObj.SO_ROOT, order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkTileCollection : ScriptableObject
    {
        public const string PROP_TILE_LIST = nameof(_cachedTiles);

        [SerializeField] private Tile[] _cachedTiles = new Tile[0];
        
        public Tile GetByName(string tileName)
        {
            return _cachedTiles.FirstOrDefault(p => p.name.Equals(tileName));
        }
    }
}
