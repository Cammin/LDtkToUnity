using System.Collections.Generic;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Increments for each occurrence of a tilemap or entity when building layers to ensure everything is layered correctly.
    /// This can be overridden to jump to custom-set sorting order numbers if needed.
    /// </summary>
    internal sealed class LDtkSortingOrder
    {
        public int SortingOrderValue { get; private set; } = 0;
        private readonly Dictionary<string, int> _layerNameOverrides;
        private readonly HashSet<string> _layerNameOccurrences = new HashSet<string>();

        public LDtkSortingOrder(Dictionary<string, int> layerNameOverrides = null)
        {
            _layerNameOverrides = layerNameOverrides;
        }
        
        public void Next(string layerIdentifier = null)
        {
            //note: this supports repeated layer occurrences, even if that never happens.
            
            //Jump to the custom order if we get the first occurrence of a layer
            if (layerIdentifier != null && _layerNameOccurrences.Add(layerIdentifier))
            {
                if (_layerNameOverrides != null && _layerNameOverrides.TryGetValue(layerIdentifier, out int order))
                {
                    SortingOrderValue = order;
                    return;
                }
            }
            
            SortingOrderValue--;
        }
    }
}