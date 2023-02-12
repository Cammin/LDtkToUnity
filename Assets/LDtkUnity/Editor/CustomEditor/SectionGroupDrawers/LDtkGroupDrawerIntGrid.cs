using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkGroupDrawerIntGrid : LDtkGroupDrawer<IntGridValueDefinition, LayerDefinition, LDtkDrawerIntGridValue>
    {
        private readonly LDtkDrawerIntGridValueIterator _intGridValueIterator;
        
        public LDtkGroupDrawerIntGrid(LayerDefinition data, SerializedProperty arrayProp, LDtkDrawerIntGridValueIterator intGridValueIterator) : base(data, arrayProp)
        {
            _intGridValueIterator = intGridValueIterator;
        }
        
        protected override List<LDtkDrawerIntGridValue> GetDrawersForGroup()
        {
            return _data.IntGridValues.Select(GetIntGridValueDrawer).ToList();
        }
        
        private LDtkDrawerIntGridValue GetIntGridValueDrawer(IntGridValueDefinition intGridValueDef)
        {
            if (_intGridValueIterator.Value >= ArrayProp.arraySize)
            {
                LDtkDebug.LogError("Array index out of bounds, the serialized array likely wasn't constructed properly for IntGrid layer");
                return null;
            }
            
            SerializedProperty valueObj = ArrayProp.GetArrayElementAtIndex(_intGridValueIterator.Value);
            _intGridValueIterator.Value++;

            return new LDtkDrawerIntGridValue(intGridValueDef, valueObj, _data.DisplayOpacity);
        }
    }
}