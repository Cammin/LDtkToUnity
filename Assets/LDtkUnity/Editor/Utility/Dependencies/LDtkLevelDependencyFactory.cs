using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LDtkUnity.Editor
{
    internal sealed class DugDependencyDataLevel
    {
        public string Background;
        public HashSet<string> Entities = new HashSet<string>();
        public HashSet<string> IntGridValues = new HashSet<string>();

        public override string ToString()
        {
            return $"Background: {Background}\n" +
                   $"Entities: {string.Join(", ", Entities)},\n" +
                   $"IntGridValues: {string.Join(", ", IntGridValues)},";
        }
    }
    internal static class LDtkLevelDependencyFactory
    {
        public static string[] GatherLevelDependencies(string levelPath)
        {
            string projectPath = new LDtkRelativeGetterProjectImporter().GetPath(levelPath, levelPath);
            
            if (!File.Exists(projectPath))
            {
                return Array.Empty<string>();
            }

            if (LDtkPathUtility.IsFileBackupFile(levelPath, projectPath))
            {
                return Array.Empty<string>();
            }
            
            string version = "";
            LDtkJsonDigger.GetJsonVersion(projectPath, ref version);
            if (!LDtkJsonImporter.CheckOutdatedJsonVersion(version, projectPath))
            {
                return Array.Empty<string>();
            }

            string[] levelLines = LDtkDependencyUtil.LoadMetaLinesAtPath(levelPath);
            if (LDtkDependencyUtil.ShouldDependOnNothing(levelLines))
            {
                return Array.Empty<string>();
            }
            
            HashSet<string> paths = new HashSet<string>();
            paths.Add(projectPath);

            DugDependencyDataLevel depends = new DugDependencyDataLevel();
            LDtkJsonDigger.GetSeparateLevelDependencies(levelPath, ref depends);
            
            string relLvlBackgroundPath = depends.Background;
            HashSet<string> entities = depends.Entities;
            HashSet<string> intGridValues = depends.IntGridValues;
            
            if (relLvlBackgroundPath != null)
            {
                if (!string.IsNullOrEmpty(relLvlBackgroundPath))
                {
                    LDtkRelativeGetterLevelBackground levelGetter = new LDtkRelativeGetterLevelBackground();
                    string levelBgPath = levelGetter.GetPathRelativeToPath(projectPath, relLvlBackgroundPath);
                    paths.Add(levelBgPath);
                }
            }

            //we get all possible assets that is possibly available as the serialized information.  
            string[] projectLines = LDtkDependencyUtil.LoadMetaLinesAtPath(projectPath);
            List<ParsedMetaData> allSerializedAssets = LDtkDependencyUtil.GetMetaDatas(projectLines);
            foreach (ParsedMetaData data in allSerializedAssets)
            {
                if (data.Name == "_customLevelPrefab")
                {
                    AddThisData();
                    continue;
                }
                
                if (TryAddFromMatchingArrayElement(entities))
                {
                    continue;
                }

                if (TryAddFromMatchingArrayElement(intGridValues))
                {
                    continue;
                }

                bool TryAddFromMatchingArrayElement(HashSet<string> assetKeys)
                {
                    foreach (string assetKey in assetKeys)
                    {
                        if (data.Name == assetKey)
                        {
                            AddThisData();
                            return true;
                        }
                    }
                    return false;
                }
                
                void AddThisData()
                {
                    string assetPath = data.GetAssetPath();
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        paths.Add(assetPath);
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