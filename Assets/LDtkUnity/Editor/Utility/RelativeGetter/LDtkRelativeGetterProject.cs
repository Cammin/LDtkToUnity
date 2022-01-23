using System.IO;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkRelativeGetterProject : LDtkRelativeGetter<LDtkLevelImporter, GameObject>
    {
        protected override bool LOG => false;

        //find the path above us, our level is always a child in a directory with the same name as the project name
        protected override string GetRelPath(LDtkLevelImporter importer)
        {
            string directory = Path.GetDirectoryName(importer.assetPath);
            string directoryName = Path.GetFileName(directory);
            string relPath = $"/../{directoryName}.ldtk";

            return relPath;
        }
    }
}