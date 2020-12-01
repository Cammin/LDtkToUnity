using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public class LDtkReferenceDrawerEnum : LDtkReferenceDrawer<LDtkDefinitionEnum>
    {
        protected override void DrawInternal(Rect controlRect, LDtkDefinitionEnum data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadEnumIcon(), data);
        }
    }
}