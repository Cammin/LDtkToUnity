using LDtkUnity.Data;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerEnum : LDtkReferenceDrawer<LDtkDefinitionEnum>
    {
        protected override void DrawInternal(Rect controlRect, LDtkDefinitionEnum data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadEnumIcon(), data);
        }
    }
}