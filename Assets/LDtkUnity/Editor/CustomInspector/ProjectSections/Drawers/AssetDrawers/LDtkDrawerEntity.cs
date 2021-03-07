using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerEntity : LDtkAssetDrawer<EntityDefinition, GameObject>
    {
        public LDtkDrawerEntity(SerializedProperty obj, string key) : base(obj, key)
        {
        }
        
        protected override void DrawInternal(Rect controlRect, EntityDefinition data)
        {
            DrawField(controlRect, data);
        }


    }
}