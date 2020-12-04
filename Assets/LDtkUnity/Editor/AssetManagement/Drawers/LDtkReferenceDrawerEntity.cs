using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public class LDtkReferenceDrawerEntity : LDtkAssetReferenceDrawer<LDtkDefinitionEntity>
    {
        public LDtkReferenceDrawerEntity(SerializedProperty asset) : base(asset)
        {
        }
        
        protected override void DrawInternal(Rect controlRect, LDtkDefinitionEntity data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadEntityIcon(), data);
        }
    }
}