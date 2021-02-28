using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerEnum : LDtkReferenceDrawer<EnumDefinition>
    {
        protected override void DrawInternal(Rect controlRect, EnumDefinition data)
        {
            DrawIconAndLabel(controlRect, LDtkIconLoader.LoadEnumIcon(), data);
        }
    }
}