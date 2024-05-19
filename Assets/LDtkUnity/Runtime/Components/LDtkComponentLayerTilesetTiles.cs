using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.COMPONENT_LAYER_TILESET)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentLayerTilesetTiles : MonoBehaviour
    {
        //todo this needs more actual data, the TileInstance. A or T values
        
        [SerializeField] private Tilemap _tilemap;
        
        private Dictionary<Type, Vector3Int[]> _positionsOfEnumValues;
        
        public Tilemap Tilemap => _tilemap;
        
        internal void OnImport(Tilemap tilemap)
        {
            _tilemap = tilemap;
        }

        /// <summary>
        /// Get all Tilemap positions in this layer that use a particular Enum value
        /// </summary>
        /// <returns></returns>
        public Vector3Int[] GetCoordinatesOfEnumValue<TEnum>() where TEnum : struct
        {
            Type type = typeof(TEnum);
            if (!type.IsEnum)
            {
                LDtkDebug.LogError($"Input type {type.Name} is not an enum");
                return null;
            }

            TryCacheCoordsOfType(type);
            
            return _positionsOfEnumValues[type];
        }

        private void TryCacheCoordsOfType(Type enumType)
        {
            if (_positionsOfEnumValues == null || !_positionsOfEnumValues.ContainsKey(enumType))
            {
                CacheCoordsOfType(enumType);
            }
        }

        private void CacheCoordsOfType(Type enumType)
        {
            List<Vector3Int> positions = new List<Vector3Int>();
            Vector3Int coordinate = Vector3Int.zero;
            
            BoundsInt bounds = _tilemap.cellBounds;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                coordinate.x = x;
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    coordinate.y = y;
                        
                    LDtkTilesetTile tile = _tilemap.GetTile<LDtkTilesetTile>(coordinate);
                    if (tile != null && tile.HasEnumTagValue(enumType))
                    {
                        positions.Add(coordinate);
                    }
                }
            }

            if (_positionsOfEnumValues == null)
            {
                _positionsOfEnumValues = new Dictionary<Type, Vector3Int[]>(1);
            }
            
            _positionsOfEnumValues[enumType] = positions.ToArray();
        }

        /// <summary>
        /// Get tiles at the coordinate from all tilemaps, even null tiles.
        /// Typically, there will only be one tile within a coordinate, but there can be multiple if rules had generated it.
        /// </summary>
        public LDtkTilesetTile[] GetTilesetTiles(Vector3Int coord)
        {
            LDtkTilesetTile[] tiles = new LDtkTilesetTile[_tilemap.cellBounds.zMax - _tilemap.cellBounds.zMin];

            for (int z = _tilemap.cellBounds.zMin; z < _tilemap.cellBounds.zMax; z++)
            {
                Vector3Int pos = new Vector3Int(coord.x, coord.y, z);
                LDtkTilesetTile tile = _tilemap.GetTile<LDtkTilesetTile>(pos);
                
                // Add the tile to the array at the corresponding index
                tiles[z - _tilemap.cellBounds.zMin] = tile;
            }

            return tiles;
        }
    }
}