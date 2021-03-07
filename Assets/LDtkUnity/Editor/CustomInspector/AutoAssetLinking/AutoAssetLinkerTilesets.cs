namespace LDtkUnity.Editor
{
    public class AutoAssetLinkerTilesets : AutoAssetLinker<TilesetDefinition>
    {
        protected override string ButtonText => "Auto-Assign Tilesets";
        protected override string GetRelPath(TilesetDefinition definition) => definition.RelPath;
    }
}