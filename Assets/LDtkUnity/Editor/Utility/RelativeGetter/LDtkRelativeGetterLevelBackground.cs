using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkRelativeGetterLevelBackground : LDtkRelativeGetter<Level, Texture2D>
    {
        protected override string GetRelPath(Level definition)
        {
            return definition.BgRelPath;
        }
    }
}