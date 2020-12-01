using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Level;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public class LDtkReferenceDrawerLevelIdentifier : LDtkReferenceDrawer<LDtkDataLevel>
    {
        protected override void DrawInternal(Rect controlRect, LDtkDataLevel data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadWorldIcon(), data);
        }

    }
}