using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerIntGridValue : LDtkAssetDrawer<IntGridValueDefinition, Sprite>
    {
        private readonly float _opacity;
        
        public LDtkDrawerIntGridValue(IntGridValueDefinition def, SerializedProperty obj, string key, float opacity) : base(def, obj, key)
        {
            _opacity = opacity;
        }
        
        public override void Draw()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
        
            Rect iconRect = new Rect(controlRect)
            {
                width = controlRect.height
            };

            DrawValueColorBox(_data, iconRect);
            DrawBoxLabel(controlRect, _data);
            
            DrawField(controlRect, controlRect.height);
        }

        public override bool HasProblem()
        {
            return false;
        }

        private static void DrawBoxLabel(Rect controlRect, IntGridValueDefinition data)
        {
            Color.RGBToHSV(data.UnityColor, out float _, out float _, out float definitionValue);

            float colorValue = 0.1f;
            Color textColor = definitionValue > 0.33f ? new Color(colorValue, colorValue, colorValue) : GUI.contentColor;

            string value = $"{data.Value}";
            Color prevColor = GUI.contentColor;
            GUI.contentColor = textColor;
            GUI.Label(controlRect, value);
            GUI.contentColor = prevColor;
        }

        private void DrawValueColorBox(IntGridValueDefinition data, Rect iconRect)
        {
            Color valueColor = data.UnityColor;
            valueColor.a = _opacity;
            EditorGUI.DrawRect(iconRect, valueColor);
        }
    }
}