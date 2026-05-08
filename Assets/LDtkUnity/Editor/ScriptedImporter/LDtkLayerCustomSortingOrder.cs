using System;

namespace LDtkUnity.Editor
{
    [Serializable]
    public struct LDtkLayerCustomSortingOrder
    {
        public const string NAME = nameof(_ldtkLayerName);
        public const string ORDER = nameof(_ldtkLayerOrder);
        
        public string _ldtkLayerName;
        public int _ldtkLayerOrder;
    }
}