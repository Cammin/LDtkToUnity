namespace LDtkUnity.Editor
{
    public class AutoAssetLinkerLevels : AutoAssetLinker<Level>
    {
        protected override string ButtonText => "Auto-Assign Levels";
        
        protected override string GetRelPath(Level definition) => definition.ExternalRelPath;
    }
}