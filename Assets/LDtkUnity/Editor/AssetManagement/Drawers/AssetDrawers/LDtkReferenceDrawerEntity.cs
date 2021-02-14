using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerEntity : LDtkAssetReferenceDrawer<EntityDefinition>
    {
        public LDtkReferenceDrawerEntity(SerializedObject obj, string key) : base(obj, key)
        {
        }
        
        protected override void DrawInternal(Rect controlRect, EntityDefinition data)
        {
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadEntityIcon(), data);
        }
    }
}