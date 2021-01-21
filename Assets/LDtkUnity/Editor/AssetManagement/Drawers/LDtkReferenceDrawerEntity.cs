using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerEntity : LDtkAssetReferenceDrawer<EntityDefinition>
    {
        public LDtkReferenceDrawerEntity(SerializedProperty asset) : base(asset)
        {
        }
        
        protected override void DrawInternal(Rect controlRect, EntityDefinition data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadEntityIcon(), data);
        }
    }
}