using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.UnityAssets.Entity;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow.Drawers
{
    public class LDtkReferenceDrawerEntity : LDtkReferenceDrawer<LDtkDefinitionEntity>
    {
        protected override Texture2D FieldIcon => LDtkIconLoader.LoadEntityIcon();
        protected override void DrawField(Rect fieldRect, LDtkDefinitionEntity data)
        {
            LDtkEntityAsset t = null;
            
            t = (LDtkEntityAsset)EditorGUI.ObjectField(fieldRect, t, typeof(LDtkEntityAsset), false);
        }
    }
}