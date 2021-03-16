using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerEntity : LDtkAssetDrawer<EntityDefinition, GameObject>
    {
        public LDtkDrawerEntity(EntityDefinition def, SerializedProperty obj, string key) : base(def, obj, key)
        {
        }
    }
}