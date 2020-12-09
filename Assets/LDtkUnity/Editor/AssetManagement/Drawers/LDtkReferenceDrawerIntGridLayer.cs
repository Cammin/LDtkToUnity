using LDtkUnity.Data;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerIntGridLayer : LDtkReferenceDrawer<LDtkDefinitionLayer>
    {
        protected override void DrawInternal(Rect controlRect, LDtkDefinitionLayer data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadIntGridIcon(), data);
        }
    }
}