using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerIntGrid : LDtkContentDrawer<LayerDefinition>
    {
        protected override void DrawInternal(Rect controlRect, LayerDefinition data)
        {
            EditorGUI.LabelField(controlRect, data.Identifier);

        }

        
    }
}