using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace LDtkUnity.Editor
{
    internal class LDtkDrawerIntGrid : LDtkGroupDrawer<IntGridValueDefinition, LayerDefinition, LDtkDrawerIntGridValue>
    {
        private readonly LDtkDrawerIntGridValueIterator _intGridValueIterator;
        
        public LDtkDrawerIntGrid(LayerDefinition data, SerializedProperty arrayProp, LDtkDrawerIntGridValueIterator intGridValueIterator) : base(data, arrayProp)
        {
            _intGridValueIterator = intGridValueIterator;
            Drawers = GetDrawers().ToList();
        }

        protected override List<LDtkDrawerIntGridValue> GetDrawers()
        {
            return _data.IntGridValues.Select(GetIntGridValueDrawer).ToList();
        }
        
        private LDtkDrawerIntGridValue GetIntGridValueDrawer(IntGridValueDefinition intGridValueDef)
        {
            SerializedProperty valueObj = ArrayProp.GetArrayElementAtIndex(_intGridValueIterator.Value);
            _intGridValueIterator.Value++;

            string key = LDtkKeyFormatUtil.IntGridValueFormat(_data, intGridValueDef);

            return new LDtkDrawerIntGridValue(intGridValueDef, valueObj, key, (float) _data.DisplayOpacity);
        }
    }
}