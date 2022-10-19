namespace LDtkUnity.Editor
{
    internal sealed class LDtkRelativeGetterLevels : LDtkRelativeGetter<Level, LDtkLevelFile>
    {
        protected override string GetRelPath(Level definition)
        {
            return definition.ExternalRelPath;
        }
    }
}