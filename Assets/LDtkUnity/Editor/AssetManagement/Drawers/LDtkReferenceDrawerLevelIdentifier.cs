using LDtkUnity.Data;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerLevelIdentifier : LDtkReferenceDrawer<LDtkDataLevel>
    {
        protected override void DrawInternal(Rect controlRect, LDtkDataLevel data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadWorldIcon(), data);
        }

    }
}