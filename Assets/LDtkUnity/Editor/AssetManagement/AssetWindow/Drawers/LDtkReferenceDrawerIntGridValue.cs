using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.UnityAssets.IntGridValue;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow.Drawers
{
    public class LDtkReferenceDrawerIntGridValue : LDtkReferenceDrawer<LDtkDefinitionIntGridValue>
    {
        protected override Texture2D FieldIcon => LDtkIconLoader.LoadIntGridIcon();
        protected override void DrawField(Rect fieldRect, LDtkDefinitionIntGridValue data)
        {
            throw new System.NotImplementedException();
        }
    }
}