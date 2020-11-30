using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow.Drawers
{
    public class LDtkReferenceDrawerEnum : LDtkReferenceDrawer<LDtkDefinitionEnum>
    {
        protected override Texture2D FieldIcon => LDtkIconLoader.LoadEnumIcon();
        protected override void DrawField(Rect fieldRect, LDtkDefinitionEnum data)
        {
            
        }
    }
}