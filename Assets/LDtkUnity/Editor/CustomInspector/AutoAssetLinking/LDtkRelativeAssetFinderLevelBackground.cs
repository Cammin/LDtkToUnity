using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkRelativeAssetFinderLevelBackground : LDtkRelativeAssetFinder<Level, Texture2D>
    {
        protected override string GetRelPath(Level definition)
        {
            return definition.BgRelPath;
        }
    }
}