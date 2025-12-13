using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkSortingOrder
    {
        public int SortingOrderValue { get; private set; } = 0;
        private readonly Dictionary<string, int> _overrides;
        private int _autoValue = 0;

        public LDtkSortingOrder(Dictionary<string, int> overrides = null)
        {
            _overrides = overrides;
        }

        public void Next(string layerIdentifier = null)
        {
            if (layerIdentifier != null && _overrides != null && _overrides.TryGetValue(layerIdentifier, out int order))
            {
                SortingOrderValue = order;
                Debug.Log(order);
                return;
            }
            
            SortingOrderValue = --_autoValue;
        }
    }
}