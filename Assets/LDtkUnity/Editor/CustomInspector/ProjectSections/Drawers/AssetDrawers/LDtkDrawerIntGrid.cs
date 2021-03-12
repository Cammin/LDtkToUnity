using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerIntGrid : LDtkContentDrawer<LayerDefinition>
    {
        private readonly SerializedProperty _arrayProp;
        private readonly List<LDtkDrawerIntGridValue> _intGridValueDrawers;
        private readonly LDtkDrawerIntGridValueIterator _intGridValueIterator;
        
        public LDtkDrawerIntGrid(LayerDefinition data, SerializedProperty arrayProp, LDtkDrawerIntGridValueIterator intGridValueIterator) : base(data)
        {
            _arrayProp = arrayProp;
            _intGridValueIterator = intGridValueIterator;
            _intGridValueDrawers = _data.IntGridValues.Select(GetIntGridValueDrawer).ToList();
        }
        
        public override void Draw()
        {
            //draw basic intgrid layer label
            base.Draw();
            
            //THEN the int grid values
            foreach (LDtkDrawerIntGridValue valueDrawer in _intGridValueDrawers)
            {
                valueDrawer.Draw();
            }
        }

        public override bool HasProblem()
        {
            return _intGridValueDrawers.Any(p => p.HasProblem());
        }

        private LDtkDrawerIntGridValue GetIntGridValueDrawer(IntGridValueDefinition intGridValueDef)
        {
            SerializedProperty valueObj = _arrayProp.GetArrayElementAtIndex(_intGridValueIterator.Value);
            _intGridValueIterator.Value++;

            string key = LDtkIntGridKeyFormat.GetKeyFormat(_data, intGridValueDef);

            return new LDtkDrawerIntGridValue(intGridValueDef, valueObj, key, (float) _data.DisplayOpacity);
        }
    }
}