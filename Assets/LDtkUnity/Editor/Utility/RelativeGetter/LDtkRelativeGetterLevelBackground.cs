using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    internal class LDtkRelativeGetterLevelBackground : LDtkRelativeGetter<Level, Texture2D>
    {
        protected override string GetRelPath(Level definition)
        {
            return definition.BgRelPath;
        }
    }
}