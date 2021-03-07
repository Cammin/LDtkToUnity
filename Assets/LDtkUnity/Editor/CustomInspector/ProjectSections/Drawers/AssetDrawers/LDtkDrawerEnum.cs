using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerEnum : LDtkContentDrawer<EnumDefinition>
    {
        protected override void DrawInternal(Rect controlRect, EnumDefinition data)
        {
            GUIContent content = new GUIContent()
            {
                text = data.Identifier,
                tooltip = string.Join(", ", data.Values.Select(p => p.Id))
            };
            EditorGUI.LabelField(controlRect, content);
        }
    }
}