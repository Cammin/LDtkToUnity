using UnityEditor;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerLevel : LDtkAssetDrawer<Level, LDtkLevelFile>
    {
        public LDtkDrawerLevel(Level data, SerializedProperty obj, string key) : base(data, obj, key)
        {
        }
        
        public override bool HasProblem()
        {
            if (base.HasProblem())
            {
                return true;
            }

            if (Asset.Identifier != _data.Identifier)
            {
                CacheError($"Invalid Level assignment: Assign the level as this field requires.\n \"{Asset.Identifier}\" is not \"{_data.Identifier}\"");
                return true;
            }

            return false;
        }
    }
}