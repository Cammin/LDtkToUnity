using UnityEditor;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerLevel : LDtkAssetDrawer<Level, LDtkLevelFile>
    {
        public LDtkDrawerLevel(Level data, SerializedProperty obj, string key) : base(data, obj, key)
        {
        }

        protected override string AssetUnassignedText => "Level file is not assigned";
    }
}