using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkRelativeGetterLevelPrefab : LDtkRelativeGetter<Level, GameObject>
    {
        protected override string GetRelPath(Level definition)
        {
            return definition.ExternalRelPath;
        }
    }
}