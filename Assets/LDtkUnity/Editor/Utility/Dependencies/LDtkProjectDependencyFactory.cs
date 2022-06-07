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
            
            HashSet<string> paths = new HashSet<string>();
            if (LDtkJsonDigger.GetTilesetRelPaths(projectPath, out IEnumerable<string> texturePaths))
            {
                foreach (string relPath in texturePaths)
                {
                    string path = new LDtkRelativeGetterTilesetTexture().GetPathRelativeToPath(projectPath, relPath);
                    if (!string.IsNullOrEmpty(path))
                    {
                        paths.Add(path);       
                    }
                }
            }
            
            //ONLY depend on other Assets when we are not separate level files.
            //If separate levels files, then the levels should instead depend on assets because the project won't depend on these assets anymore.
            if (isExternalLevels)
            {
                return Array.Empty<string>();
            }
            
            List<ParsedMetaData> datas = LDtkDependencyUtil.GetMetaDatasAtProjectPath(projectPath);
            foreach (ParsedMetaData data in datas)
            {
                string assetPath = data.GetAssetPath();
                if (!string.IsNullOrEmpty(assetPath))
                {
                    paths.Add(assetPath);
                }
            }
            
            foreach (string path in paths)
            {
                LDtkDependencyUtil.TestLogDependencySet("GatherProjectDependencies", projectPath, path);
            }

            return paths.ToArray();
        }
    }
}