using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerIntGridValue : LDtkAssetDrawer<IntGridValueDefinition, LDtkIntGridTile>
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
            Color color = HandleUtil.GetTextColorForIntGridValueNumber(data.UnityColor);

            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                normal = new GUIStyleState()
                {
                    textColor = color
                }
            };

            string value = $"{data.Value}";
            GUI.Label(controlRect, value, style);

        }

        private void DrawValueColorBox(IntGridValueDefinition data, Rect iconRect)
        {
            Color valueColor = data.UnityColor;
            valueColor.a = _opacity;
            EditorGUI.DrawRect(iconRect, valueColor);
        }
    }
}