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
    [HelpURL(LDtkHelpURL.COMPONENT_LAYER)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentLayerIntGridValues : MonoBehaviour
    {
        [SerializeField] internal IntGridValuePositions[] _values;
        
        private Dictionary<Vector3Int, LDtkDefinitionObjectIntGridValue> _valuesDict;
        
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

        private void TryInitialize()
        {
            if (_valuesDict == null)
            {
                Initialize();
            }
        }
        
        /// <summary>
        /// Indexes the serialized data. Call this to re-cache data if needed.
        /// </summary>
        [PublicAPI]
        public void Initialize()
        {
            _valuesDict = new Dictionary<Vector3Int, LDtkDefinitionObjectIntGridValue>(_values.Sum(p => p._positions.Count));
            foreach (IntGridValuePositions value in _values)
            {
                foreach (Vector3Int position in value._positions)
                {
                    if (_valuesDict.ContainsKey(position))
                    {
                        GameObject obj = gameObject;
                        LDtkDebug.LogWarning($"Duplicate entry found for {obj.name}, ", obj);
                        continue;
                    }
                    _valuesDict.Add(position, value._value);
                }
            }
        }
        
        /// <summary>
        /// Get a IntGridValue tile at the coordinate for this layer. Can be null.
        /// </summary>
        //todo needs a unit test
        public LDtkDefinitionObjectIntGridValue GetValueDefinition(Vector3Int coord)
        {
            TryInitialize();
            return _valuesDict.TryGetValue(coord, out LDtkDefinitionObjectIntGridValue def) ? def : null;
        }
        
        /// <summary>
        /// Get a IntGridValue tile at the coordinate for this layer
        /// </summary>
        //todo needs a unit test
        public int GetValue(Vector3Int coord)
        {
            LDtkDefinitionObjectIntGridValue def = GetValueDefinition(coord);
            return def != null ? def.Value : 0;
        }

        /// <summary>
        /// Get all Tilemap coordinates of a specific IntGrid value.
        /// </summary>
        //todo needs a unit test
        public Vector3Int[] GetPositionsOfValue(LDtkDefinitionObjectIntGridValue value)
        {
            TryInitialize();
            return _valuesDict
                .Where(p => p.Value == value)
                .Select(p => p.Key).ToArray();
        }
        
        /// <summary>
        /// Get all Tilemap coordinates of a specific IntGrid value.
        /// </summary>
        //todo needs a unit test
        public Vector3Int[] GetPositionsOfValue(int value)
        {
            TryInitialize();
            return _valuesDict
                .Where(p => p.Value.Value == value)
                .Select(p => p.Key).ToArray();
        }
    }
}
