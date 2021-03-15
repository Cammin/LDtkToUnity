using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerEnum : LDtkContentDrawer<EnumDefinition>
    {
        public LDtkDrawerEnum(EnumDefinition data) : base(data)
        {
        }
        
        public override void Draw()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
        
            GUIContent content = new GUIContent()
            {
                text = _data.Identifier,
                tooltip = string.Join(", ", _data.Values.Select(p => p.Id))
            };
            EditorGUI.LabelField(controlRect, content);
        }
    }
}