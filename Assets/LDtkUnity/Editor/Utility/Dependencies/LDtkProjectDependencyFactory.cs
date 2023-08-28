using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Utf8Json;

namespace LDtkUnity.Editor
{
    internal static class LDtkProjectDependencyFactory
    {
        public static string[] GatherProjectDependencies(string projectPath)
        {
            if (LDtkPathUtility.IsFileBackupFile(projectPath, projectPath))
            {
                return Array.Empty<string>();
            }

            string[] projectLines = LDtkDependencyUtil.LoadMetaLinesAtPath(projectPath);
            if (LDtkDependencyUtil.ShouldDependOnNothing(projectLines))
            {
                return Array.Empty<string>();
            }
            
            bool isExternalLevels = false;
            if (!LDtkJsonDigger.GetIsExternalLevels(projectPath, ref isExternalLevels))
            {
                LDtkDebug.LogError("Issue getting external levels");
                return Array.Empty<string>();
            }
            
            HashSet<string> paths = new HashSet<string>();
            
            DependOnTilesetFiles(projectPath, paths);
            //DependOnPathsToTexture(projectPath, paths);
            
            //Separate levels depend on the further assets instead
            if (isExternalLevels)
            {
                return paths.ToArray();
            }
            
            DependOnUsedBackgrounds(projectPath, paths);
            DependOnProjectAssets(projectLines, paths);

            /*foreach (string path in paths)
            {
                LDtkDependencyUtil.TestLogDependencySet("GatherProjectDependencies", projectPath, path);
            }*/

            return paths.ToArray();
        }

        private static void DependOnProjectAssets(string[] projectLines, HashSet<string> paths)
        {
            List<ParsedMetaData> datas = LDtkDependencyUtil.GetMetaDatas(projectLines);
            foreach (ParsedMetaData data in datas)
            {
                string assetPath = data.GetAssetPath();
                if (!string.IsNullOrEmpty(assetPath))
                {
                    paths.Add(assetPath);
                }
            }
        }

        private static void DependOnTilesetFiles(string projectPath, HashSet<string> paths)
        {
            HashSet<string> tilesetDefNames = new HashSet<string>();
            LDtkJsonDigger.GetTilesetDefNames(projectPath, ref tilesetDefNames);

            foreach (string defName in tilesetDefNames)
            {
                string tilesetPath = LDtkProjectImporter.TilesetImporterPath(projectPath, defName);

                if (File.Exists(tilesetPath))
                {
                    paths.Add(tilesetPath);
                }
            }
        }

        private static void DependOnUsedBackgrounds(string projectPath, HashSet<string> paths)
        {
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
        }

        private static void DependOnPathsToTexture(string projectPath, HashSet<string> paths)
        {
            HashSet<string> texturePaths = new HashSet<string>();
            if (LDtkJsonDigger.GetTilesetTextureRelPaths(projectPath, ref texturePaths))
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
        }
    }
}