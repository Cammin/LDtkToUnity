using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerIntGridValue : LDtkAssetReferenceDrawer<IntGridValueDefinition>
    {
        private readonly float _opacity;
        
        public LDtkReferenceDrawerIntGridValue(SerializedProperty obj, string key, float opacity) : base(obj, key)
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

            string value = $"{data.Value}";
            
            Color.RGBToHSV(data.UnityColor, out float _, out float _, out float definitionValue);

            float colorValue = 0.1f;
            Color textColor = definitionValue > 0.25f ? new Color(colorValue, colorValue, colorValue) : GUI.contentColor;
            
            Color prevColor = GUI.contentColor;
            GUI.contentColor = textColor;
            GUI.Label(controlRect, value);
            GUI.contentColor = prevColor;
            
            string label = data.Identifier;
            
            DrawLabel(controlRect, label);
            

            controlRect.x -= 15;
            
            DrawField<Sprite>(controlRect);
        }
    }
}