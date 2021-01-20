using LDtkUnity.Data;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerIntGridValue : LDtkAssetReferenceDrawer<IntGridValueDefinition>
    {
        private readonly float _opacity;
        
        public LDtkReferenceDrawerIntGridValue(SerializedProperty asset, float opacity) : base(asset)
        {
            _opacity = opacity;
        }
        
        protected override void DrawInternal(Rect controlRect, IntGridValueDefinition data)
        {
            controlRect.x += 15;
            Rect iconRect = GetLeftIconRect(controlRect);

            Color valueColor = data.UnityColor;
            valueColor.a = _opacity;
            EditorGUI.DrawRect(iconRect, valueColor);
            
            DrawLabel(controlRect, data);

            controlRect.x -= 15;
            
            if (!HasProblem)
            {
                if (string.IsNullOrEmpty(data.Identifier))
                {
                    ThrowError(controlRect, "The IntGrid Value's name in the LDtk project given a unique identifier");
                }
            }
            
            DrawFieldAndObject(controlRect, data);
            

        }
    }
}