using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerEntity : LDtkAssetDrawer<EntityDefinition, GameObject>
    {
        public LDtkDrawerEntity(EntityDefinition def, SerializedProperty obj, string key) : base(def, obj, key)
        {
        }

        protected override string AssetUnassignedText => "No prefab assigned; Entity instance won't show up in the import result";
    }
}