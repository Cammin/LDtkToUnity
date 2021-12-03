using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkRelativeGetterLevels : LDtkRelativeGetter<Level, LDtkLevelFile>
    {
        protected override string GetRelPath(Level definition)
        {
            return definition.ExternalRelPath;
        }
    }
}