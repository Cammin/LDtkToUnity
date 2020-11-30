using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.UnityAssets;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow.Drawers
{
    public class LDtkReferenceDrawerLevelIdentifier : LDtkReferenceDrawer<LDtkDataLevel>
    {
        protected override Texture2D FieldIcon => LDtkIconLoader.LoadWorldIcon();
        protected override void DrawField(Rect fieldRect, LDtkDataLevel data)
        {
            //nothing
        }
    }
}