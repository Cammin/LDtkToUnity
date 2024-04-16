using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.COMPONENT_LAYER)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentLayerTilesetTiles : MonoBehaviour
    {
        //todo this needs more actual data, the TileInstance
        
        [SerializeField] private List<Tilemap> _tilemaps = new List<Tilemap>();
        
        public IReadOnlyList<Tilemap> Tilemaps => _tilemaps;

        internal void OnImport(List<Tilemap> tilemaps)
        {
            _tilemaps = tilemaps;
        }

        /// <summary>
        /// Get all Tilemap positions in this layer that use a particular Enum value
        /// </summary>
        /// <returns></returns>
        /// todo we might be able to improve this by caching all coordinates for every tile
        /// todo needs a unit test
        public List<Vector3Int> GetCoordinatesOfEnumValue<TEnum>(TEnum value) where TEnum : struct
        {
            Type type = typeof(TEnum);
            if (!type.IsEnum)
            {
                LDtkDebug.LogError($"Input type {type.Name} is not an enum");
                return null;
            }
            
            List<Vector3Int> coords = new List<Vector3Int>();
            foreach (Tilemap tilemap in _tilemaps)
            {
                BoundsInt bounds = tilemap.cellBounds;
                Vector3Int coord = bounds.min;
                for (; coord.x < bounds.xMax; coord.x++)
                {
                    for (; coord.y < bounds.yMax; coord.y++)
                    {
                        LDtkTilesetTile tile = tilemap.GetTile<LDtkTilesetTile>(coord);
                        if (tile && tile.HasEnumTagValue(value))
                        {
                            coords.Add(coord);
                        }
                    }
                }
            }
            return coords;
        }

        /// <summary>
        /// Get tiles at the coordinate from all tilemaps, even null tiles.
        /// Typically there will only be one tile within a coordinate, but there can be multiple if rules had generated it.
        /// </summary>
        /// todo needs a unit test
        public LDtkTilesetTile[] GetTilesetTiles(Vector3Int coord)
        {
            LDtkTilesetTile[] tiles = new LDtkTilesetTile[_tilemaps.Count];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = _tilemaps[i].GetTile<LDtkTilesetTile>(coord);
            }
            return tiles;
        }
    }
}