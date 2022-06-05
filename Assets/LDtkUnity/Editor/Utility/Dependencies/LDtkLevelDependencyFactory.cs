using System;
using System.Collections.Generic;
using System.IO;

namespace LDtkUnity.Editor
{
    internal static class LDtkLevelDependencyFactory
    {
        public static string[] GatherLevelDependencies(string levelPath)
        {
            List<string> paths = new List<string>();
            
            string projectPath = new LDtkRelativeGetterProjectImporter().GetPath(levelPath, levelPath);
            if (!File.Exists(projectPath))
            {
                LDtkDebug.LogError($"The project cannot be found at \"{projectPath}\", Check that there are no broken paths. Most likely the project was renamed but not saved from LDtk yet. Save the project in LDtk to potentially fix this problem");
                return Array.Empty<string>();
            }
            paths.Add(projectPath);

            //we get all possible assets that is possibly available as the serialized information.  
            string[] lines = LDtkDependencyUtil.LoadMetaLinesAtProjectPath(projectPath);
            List<ParsedMetaData> allSerializedAssets = LDtkDependencyUtil.GetMetaDatas(lines);
            
            foreach (ParsedMetaData data in allSerializedAssets)
            {
                if (LDtkJsonDigger.GetUsedEntitiesInJsonLevel(levelPath, out IEnumerable<string> entities))
                {
                    foreach (string entity in entities)
                    {
                        if (entity == data.Name)
                        {
                            paths.Add(data.GetAssetPath());
                        }
                    }
                }

                if (LDtkJsonDigger.GetUsedIntGridValues(levelPath, out IEnumerable<string> intGridValues))
                {
                    foreach (string intGridValue in intGridValues)
                    {
                        if (intGridValue == data.Name)
                        {
                            paths.Add(data.GetAssetPath());
                        }
                    }
                }
            }

            foreach (string path in paths)
            {
                LDtkDependencyUtil.TestLogDependencySet("GatherLevelDependencies", levelPath, path);
            }
            
            return paths.ToArray();
        }
    }
}