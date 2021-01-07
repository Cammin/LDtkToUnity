using LDtkUnity.Data;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerLevelIdentifier : LDtkReferenceDrawer<Level>
    {
        protected override void DrawInternal(Rect controlRect, Level data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadWorldIcon(), data);
        }

    }
}