using System.IO;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Usable for any asset that's in a folder relative to a ldtk file.
    /// </summary>
    internal sealed class LDtkRelativeGetterProjectImporter : LDtkRelativeGetter<string, LDtkProjectImporter>
    {
        protected override bool LOG => false;

        //find the path above us, our level is always a child in a directory with the same name as the project name
        protected override string GetRelPath(string assetPath)
        {
            string directory = Path.GetDirectoryName(assetPath);
            string directoryName = Path.GetFileName(directory);
            string relPath = $"/../{directoryName}.ldtk";

            return relPath;
        }
    }
}