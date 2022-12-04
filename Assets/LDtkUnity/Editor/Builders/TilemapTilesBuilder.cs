using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// A wrapper on the tilemap that is only meant to cache up the tiles and positions until we call SetTiles in one fell swoop to be performant
    /// </summary>
    internal sealed class TilemapTilesBuilder
    {
        public static implicit operator Tilemap(TilemapTilesBuilder builder) => builder._map;

        private readonly Tilemap _map;
        private readonly Dictionary<Vector3Int, TileBase> _tilesToBuild = new Dictionary<Vector3Int, TileBase>();

        public TilemapTilesBuilder(Tilemap map)
        {
            _map = map;
        }
        
        public void SetTile(Vector3Int cell, TileBase tileAsset)
        {
            if (_tilesToBuild.ContainsKey(cell))
            {
                LDtkDebug.Log("Tried adding a tile to a dict that already has that position");
                return;
            }
            _tilesToBuild.Add(cell, tileAsset);
        }

        public void SetTiles()
        {
            Vector3Int[] positions = _tilesToBuild.Keys.ToArray();
            TileBase[] tiles = _tilesToBuild.Values.ToArray();
            _map.SetTiles(positions, tiles);
        }
    }
}