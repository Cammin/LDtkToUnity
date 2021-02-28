using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerIntGridLayer : LDtkReferenceDrawer<LayerDefinition>
    {
        protected override void DrawInternal(Rect controlRect, LayerDefinition data)
        {
            DrawIconAndLabel(controlRect, LDtkIconLoader.LoadIntGridIcon(), data);
        }
    }
}