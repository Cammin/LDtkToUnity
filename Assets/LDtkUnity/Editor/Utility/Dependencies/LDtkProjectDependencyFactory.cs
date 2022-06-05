using System;
using System.Collections.Generic;
using System.Linq;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    internal static class LDtkProjectDependencyFactory
    {
        
        public static string[] GatherProjectDependencies(string projectPath)
        {
            if (!LDtkJsonDigger.GetIsExternalLevels(projectPath, out bool isExternalLevels))
            {
                Debug.LogError("Issue getting external levels");
                return Array.Empty<string>();
            }
            
            //ONLY depend on Assets when we are not separate level files.
            //If separate levels files, then the levels should instead depend on assets because the project won't depend on these assets anymore.
            if (isExternalLevels)
            {
                return Array.Empty<string>();
            }

            string[] lines = LDtkDependencyUtil.LoadMetaLinesAtProjectPath(projectPath);
            List<ParsedMetaData> datas = LDtkDependencyUtil.GetMetaDatas(lines);
            
            string[] assetPaths = datas.Select(p => p.GetAssetPath()).ToArray();
            foreach (string path in assetPaths)
            {
                LDtkDependencyUtil.TestLogDependencySet("GatherProjectDependencies", projectPath, path);
            }

            return assetPaths;
        }
    }
}