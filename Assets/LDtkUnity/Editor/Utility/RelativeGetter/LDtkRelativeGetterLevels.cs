namespace LDtkUnity.Editor
{
    internal class LDtkRelativeGetterLevels : LDtkRelativeGetter<Level, LDtkLevelFile>
    {
        protected override string GetRelPath(Level definition)
        {
            return definition.ExternalRelPath;
        }
    }
}