using System;
using System.Collections.Generic;
using System.Linq;

namespace LDtkUnity.Editor
{
    internal static class LDtkProjectDependencyFactory
    {
        public static string[] GatherProjectDependencies(string projectPath)
        {
            if (LDtkPathUtility.IsFileBackupFile(projectPath))
            {
                return Array.Empty<string>();
            }

            bool isExternalLevels = false;
            if (!LDtkJsonDigger.GetIsExternalLevels(projectPath, ref isExternalLevels))
            {
                LDtkDebug.LogError("Issue getting external levels");
                return Array.Empty<string>();
            }
            
            string[] projectLines = LDtkDependencyUtil.LoadMetaLinesAtPath(projectPath);
            if (LDtkDependencyUtil.ShouldDependOnNothing(projectLines))
            {
                return Array.Empty<string>();
            }
            
            HashSet<string> paths = new HashSet<string>();
            HashSet<string> texturePaths = new HashSet<string>();
            if (LDtkJsonDigger.GetTilesetRelPaths(projectPath, ref texturePaths))
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
                return paths.ToArray();
            }
            
            HashSet<string> relLvlBackgroundPaths = new HashSet<string>();
            if (LDtkJsonDigger.GetUsedBackgrounds(projectPath, ref relLvlBackgroundPaths))
            {
                foreach (string lvlBackgroundPath in relLvlBackgroundPaths)
                {
                    LDtkRelativeGetterLevelBackground levelGetter = new LDtkRelativeGetterLevelBackground();
                    string levelBgPath = levelGetter.GetPathRelativeToPath(projectPath, lvlBackgroundPath);
                    if (!string.IsNullOrEmpty(levelBgPath))
                    {
                        paths.Add(levelBgPath);
                    }
                }
            }

            List<ParsedMetaData> datas = LDtkDependencyUtil.GetMetaDatas(projectLines);
            foreach (ParsedMetaData data in datas)
            {
                string assetPath = data.GetAssetPath();
                if (!string.IsNullOrEmpty(assetPath))
                {
                    paths.Add(assetPath);
                }
            }

            //LDtkJsonDigger.GetUsedTilesetSprites(projectPath, out Dictionary<string, HashSet<int>> dict);
            //Debug.Log("we got em");

            /*foreach (KeyValuePair<string,HashSet<int>> pair in dict)
            {
                string usedTiles = string.Join(",", pair.Value.Select(p => p.ToString()));
                Debug.Log($"Used tiles: {pair.Key} : {usedTiles}"); 
            }*/
            
            foreach (string path in paths)
            {
                LDtkDependencyUtil.TestLogDependencySet("GatherProjectDependencies", projectPath, path);
            }

            return paths.ToArray();
        }
    }
}