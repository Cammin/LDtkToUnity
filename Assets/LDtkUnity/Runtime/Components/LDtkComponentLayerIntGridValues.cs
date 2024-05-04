using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Use for checking IntGrid values. The coordinates are usable for it's respective tilemaps. Accessible from IntGrid layer GameObjects.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_LAYER_INTGRID)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentLayerIntGridValues : MonoBehaviour
    {
        [SerializeField] internal IntGridValuePositions[] _values;
        
        private Dictionary<Vector3Int, LDtkDefinitionObjectIntGridValue> _valueOfPositionDict;
        private Dictionary<int, Vector3Int[]> _positionsOfValueDict;
        
        [Serializable]
        internal class IntGridValuePositions
        {
            public LDtkDefinitionObjectIntGridValue _value;
            public List<Vector3Int> _positions = new List<Vector3Int>();

            public IntGridValuePositions(LDtkDefinitionObjectIntGridValue value)
            {
                _value = value;
            }
        }
        
        internal void OnImport(LDtkDefinitionObjectsCache cache, LayerInstance instance)
        {
            //note: this could be optimized by not making a dictionary and adding data straight to the list instead
            Dictionary<int, IntGridValuePositions> valuePositions = new Dictionary<int, IntGridValuePositions>();
            
            int index = 0;
            Vector2Int cellSize = instance.UnityCellSize;

            Dictionary<int, IntGridValueDefinition> defs = new Dictionary<int, IntGridValueDefinition>();
            foreach (IntGridValueDefinition def in instance.Definition.IntGridValues)
            {
                defs.Add(def.Value, def);
            }

            Vector3Int coord = new Vector3Int(0, 0, 0);
            for (int y = 0; y < cellSize.y; y++)
            {
                coord.y = LDtkCoordConverter.ConvertCell(y, instance.CHei);
                for (int x = 0; x < cellSize.x; x++)
                {
                    coord.x = x;
                    int intGridValue = instance.IntGridCsv[index];
                    if (intGridValue != 0)
                    {
                        if (!valuePositions.ContainsKey(intGridValue))
                        {
                            LDtkDefinitionObjectIntGridValue value = new LDtkDefinitionObjectIntGridValue();
                            value.Populate(cache, defs[intGridValue]);
                            valuePositions.Add(intGridValue, new IntGridValuePositions(value));
                        }
                        valuePositions[intGridValue]._positions.Add(coord);
                    }
                    index++;
                }
            }
            _values = valuePositions.Select(pair => pair.Value).ToArray();
        }
        
        /// <summary>
        /// Get a IntGridValue tile at the coordinate for this layer. Returns 0 if a value at the coord doesn't exist. (empty tiles are an intgrid value of 0)
        /// </summary>
        [PublicAPI]
        public int GetValue(Vector3Int coord)
        {
            LDtkDefinitionObjectIntGridValue def = GetValueDefinition(coord);
            return def != null ? def.Value : 0;
        }
        
        /// <summary>
        /// Get a IntGridValue tile at the coordinate for this layer. Returns null if a value at the coord doesn't exist.
        /// </summary>
        [PublicAPI]
        public LDtkDefinitionObjectIntGridValue GetValueDefinition(Vector3Int coord)
        {
            TryCacheValueOfPositionDict();
            return _valueOfPositionDict.TryGetValue(coord, out LDtkDefinitionObjectIntGridValue def) ? def : null;
        }
        private void TryCacheValueOfPositionDict()
        {
            if (_valueOfPositionDict != null)
            {
                return;
            }
            
            _valueOfPositionDict = new Dictionary<Vector3Int, LDtkDefinitionObjectIntGridValue>(_values.Sum(p => p._positions.Count));
            foreach (IntGridValuePositions value in _values)
            {
                foreach (Vector3Int position in value._positions)
                {
                    if (!_valueOfPositionDict.ContainsKey(position))
                    {
                        _valueOfPositionDict.Add(position, value._value);
                        continue;
                    }

                    GameObject obj = gameObject;
                    LDtkDebug.LogWarning($"Duplicate entry found for {obj.name}, ", obj);
                }
            }
        }

        /// <summary>
        /// Get all Tilemap coordinates of a specific IntGrid value.
        /// </summary>
        [PublicAPI]
        public Vector3Int[] GetPositionsOfValueDefinition(LDtkDefinitionObjectIntGridValue value)
        {
            if (value == null)
            {
                LDtkDebug.LogError("Argument null when trying to get IntGrid positions", gameObject);
                return null;
            }
            return GetPositionsOfValue(value.Value);
        }
        
        /// <summary>
        /// Get all Tilemap coordinates of a specific IntGrid value.
        /// </summary>
        [PublicAPI]
        public Vector3Int[] GetPositionsOfValue(int value)
        {
            if (_positionsOfValueDict == null)
            {
                _positionsOfValueDict = _values.ToDictionary(obj => obj._value.Value, obj => obj._positions.ToArray());
            }
            return _positionsOfValueDict[value];
        }
    }
}
