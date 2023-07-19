using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkDrawerIntGridValue : LDtkAssetDrawer<IntGridValueDefinition, TileBase>
    {
        private readonly float _opacity;
        
        public LDtkDrawerIntGridValue(IntGridValueDefinition def, SerializedProperty obj, float opacity) : base(def, obj)
        {
            _opacity = opacity;
        }
        
        public override void Draw()
        {
            Color color = _data.UnityColor;
            color.a = _opacity;
            DrawField(color);
            
            Rect controlRect = GUILayoutUtility.GetLastRect();
            DrawIconIndex(controlRect, _data);
        }

        private static void DrawIconIndex(Rect controlRect, IntGridValueDefinition data)
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
    }
}