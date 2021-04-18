namespace LDtkUnity.Editor
{
    public class LDtkRelativeAssetFinderLevels : LDtkRelativeAssetFinder<Level, LDtkLevelFile>
    {
        protected override string GetRelPath(Level definition)
        {
            return definition.ExternalRelPath;
        }
    }
}