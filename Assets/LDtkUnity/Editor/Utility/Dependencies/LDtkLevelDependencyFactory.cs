using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkLevelDependencyFactory
    {
        public static string[] GatherLevelDependencies(string levelPath)
        {
            
            string projectPath = new LDtkRelativeGetterProjectImporter().GetPath(levelPath, levelPath);
            if (!File.Exists(projectPath))
            {
                LDtkDebug.LogError($"The project cannot be found at \"{projectPath}\", Check that there are no broken paths. Most likely the project was renamed but not saved from LDtk yet. Save the project in LDtk to potentially fix this problem");
                return Array.Empty<string>();
            }
            
            HashSet<string> paths = new HashSet<string>();
            paths.Add(projectPath);

            //we get all possible assets that is possibly available as the serialized information.  
            List<ParsedMetaData> allSerializedAssets = LDtkDependencyUtil.GetMetaDatasAtProjectPath(projectPath);
            
            if (LDtkJsonDigger.GetUsedEntities(levelPath, out IEnumerable<string> entities))
            {
                foreach (string entity in entities)
                {
                    foreach (ParsedMetaData data in allSerializedAssets)
                    {
                        if (entity == data.Name)
                        {
                            string assetPath = data.GetAssetPath();
                            if (!string.IsNullOrEmpty(assetPath))
                            {
                                paths.Add(assetPath);
                            }
                        }
                    }
                }
            }

            if (LDtkJsonDigger.GetUsedIntGridValues(levelPath, out IEnumerable<string> intGridValues))
            {
                foreach (string intGridValue in intGridValues)
                {
                    foreach (ParsedMetaData data in allSerializedAssets)
                    {
                        if (intGridValue == data.Name)
                        {
                            string assetPath = data.GetAssetPath();
                            if (!string.IsNullOrEmpty(assetPath))
                            {
                                paths.Add(assetPath);
                            }
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