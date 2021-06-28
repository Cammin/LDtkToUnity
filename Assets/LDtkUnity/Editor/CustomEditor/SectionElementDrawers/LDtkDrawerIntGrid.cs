using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerIntGrid : LDtkContentDrawer<LayerDefinition>
    {
        public readonly List<LDtkDrawerIntGridValue> IntGridValueDrawers;
        
        private readonly SerializedProperty _arrayProp;
        private readonly LDtkDrawerIntGridValueIterator _intGridValueIterator;
        
        public LDtkDrawerIntGrid(LayerDefinition data, SerializedProperty arrayProp, LDtkDrawerIntGridValueIterator intGridValueIterator) : base(data)
        {
            _arrayProp = arrayProp;
            _intGridValueIterator = intGridValueIterator;
            IntGridValueDrawers = _data.IntGridValues.Select(GetIntGridValueDrawer).ToList();
        }
        
        public override void Draw()
        {
            //draw basic intgrid layer label
            DrawIntGridLabel();

            //Then the int grid values
            foreach (LDtkDrawerIntGridValue valueDrawer in IntGridValueDrawers)
            {
                valueDrawer.Draw();
            }
        }

        private void DrawIntGridLabel()
        {
            GUILayout.Space(3);
            Rect controlRect = EditorGUILayout.GetControlRect(GUILayout.Height(11));
            EditorGUI.LabelField(controlRect, _data.Identifier, EditorStyles.miniLabel);
        }

        public override bool HasProblem()
        {
            return IntGridValueDrawers.Any(p => p.HasProblem());
        }

        private LDtkDrawerIntGridValue GetIntGridValueDrawer(IntGridValueDefinition intGridValueDef)
        {
            SerializedProperty valueObj = _arrayProp.GetArrayElementAtIndex(_intGridValueIterator.Value);
            _intGridValueIterator.Value++;

            string key = LDtkKeyFormatUtil.IntGridValueFormat(_data, intGridValueDef);

            return new LDtkDrawerIntGridValue(intGridValueDef, valueObj, key, (float) _data.DisplayOpacity);
        }
    }
}