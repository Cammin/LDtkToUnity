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
        
        [SerializeField] private List<Tilemap> _tilemaps = new List<Tilemap>();
        
        private Dictionary<Type, Vector3Int[]> _positionsOfEnumValues;
        
        public IReadOnlyList<Tilemap> Tilemaps => _tilemaps;
        
        internal void OnImport(List<Tilemap> tilemaps)
        {
            _tilemaps = tilemaps;
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
            
            foreach (Tilemap tilemap in _tilemaps)
            {
                BoundsInt bounds = tilemap.cellBounds;
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    coordinate.x = x;
                    for (int y = bounds.yMin; y < bounds.yMax; y++)
                    {
                        coordinate.y = y;
                        
                        LDtkTilesetTile tile = tilemap.GetTile<LDtkTilesetTile>(coordinate);
                        if (tile != null && tile.HasEnumTagValue(enumType))
                        {
                            positions.Add(coordinate);
                        }
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
            LDtkTilesetTile[] tiles = new LDtkTilesetTile[_tilemaps.Count];
            for (int i = 0; i < _tilemaps.Count; i++)
            {
                tiles[i] = _tilemaps[i].GetTile<LDtkTilesetTile>(coord);
            }
            return tiles;
        }
    }
}