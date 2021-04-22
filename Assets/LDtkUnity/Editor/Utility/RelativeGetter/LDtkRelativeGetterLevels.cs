namespace LDtkUnity.Editor
{
    public class LDtkRelativeGetterLevels : LDtkRelativeGetter<Level, LDtkLevelFile>
    {
        protected override string GetRelPath(Level definition)
        {
            return definition.ExternalRelPath;
        }
    }
}