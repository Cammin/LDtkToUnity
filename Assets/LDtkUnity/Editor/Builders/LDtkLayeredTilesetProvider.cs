using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkLayeredTilesetProvider
    {
        public delegate Tilemap TilemapCreation();
        
        private readonly Dictionary<Vector2Int, int> _layering = new Dictionary<Vector2Int, int>();
        private readonly Dictionary<int, Tilemap> _tilemaps = new Dictionary<int, Tilemap>();
        private readonly LDtkSortingOrder _sortingOrder;

        private readonly TilemapCreation _tilemapGetter;

        public LDtkLayeredTilesetProvider(LDtkSortingOrder sortingOrder, TilemapCreation tilemapGetter)
        {
            _sortingOrder = sortingOrder;
            _tilemapGetter = tilemapGetter;
        }

        public IEnumerable<Tilemap> Tilemaps => _tilemaps.Values;


        public Tilemap GetAppropriatelyLayeredTilemap(Vector2Int pos)
        {


            int layer = GetTilemapLayerToBuildOn(pos);

            //Debug.Log($"Trying to get tilemap for tile {pos}, got {layer}");
            
            
            if (_tilemaps.ContainsKey(layer))
            {
                return _tilemaps[layer];
            }
            
            _sortingOrder.Next();
            
            Tilemap tilemap = _tilemapGetter.Invoke();
            _tilemaps.Add(layer, tilemap);
            return tilemap;
        }

        /// <summary>
        /// Input a pixel position, and spits out the correct index of tilemap we need to build on.
        /// if we had already built on this position before, then we need to use the next tilemap component because that space is already occupied by a tile,
        /// and we can only have one tile in a position for a tilemap.
        ///
        /// ACTUALLY, the tiles have a z component in the tilemap, so position them this was instead by ordingering them.
        /// </summary>
        /// <param name="pos">
        /// the pixel position.
        /// </param>
        /// <returns>
        /// the index of tilemap we'd wish to use.
        /// </returns>
        private int GetTilemapLayerToBuildOn(Vector2Int pos)
        {
            if (_layering.ContainsKey(pos))
            {
                return ++_layering[pos];
            }
            
            _layering.Add(pos, 0);
            return 0;
        }

        public void Clear()
        {
            _layering.Clear();
            _tilemaps.Clear();
        }
    }
}