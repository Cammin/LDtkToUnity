using LDtkUnity.Data;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
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