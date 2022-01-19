using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkDrawerIntGridValue : LDtkAssetDrawer<IntGridValueDefinition, LDtkIntGridTile>
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
                x = controlRect.x + 1,
                y = controlRect.y + 1,
                height = controlRect.height - 2,
                width = controlRect.height - 2
                
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