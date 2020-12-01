using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.UnityAssets.Entity;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public class LDtkReferenceDrawerEntity : LDtkAssetReferenceDrawer<LDtkDefinitionEntity, LDtkEntityAsset>
    {
        public LDtkReferenceDrawerEntity(LDtkDefinitionEntity data, LDtkEntityAsset asset) : base(data, asset)
        {
        }
        
        protected override void DrawInternal(Rect controlRect, LDtkDefinitionEntity data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadEntityIcon(), data);
        }


    }
}