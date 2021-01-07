using LDtkUnity.Data;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerEnum : LDtkReferenceDrawer<EnumDefinition>
    {
        protected override void DrawInternal(Rect controlRect, EnumDefinition data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadEnumIcon(), data);
        }
    }
}