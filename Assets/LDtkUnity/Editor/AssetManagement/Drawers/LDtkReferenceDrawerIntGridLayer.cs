using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public class LDtkReferenceDrawerIntGridLayer : LDtkReferenceDrawer<LDtkDefinitionLayer>
    {
        protected override void DrawInternal(Rect controlRect, LDtkDefinitionLayer data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadIntGridIcon(), data);
        }
    }
}