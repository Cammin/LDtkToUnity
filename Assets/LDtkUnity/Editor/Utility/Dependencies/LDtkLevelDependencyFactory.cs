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
            if (LDtkPathUtility.IsFileBackupFile(levelPath))
            {
                return Array.Empty<string>();
            }

            string projectPath = new LDtkRelativeGetterProjectImporter().GetPath(levelPath, levelPath);
            if (!File.Exists(projectPath))
            {
                LDtkDebug.LogError($"The project cannot be found at \"{projectPath}\", Check that there are no broken paths. Most likely the project was renamed but not saved from LDtk yet. Save the project in LDtk to potentially fix this problem");
                return Array.Empty<string>();
            }
            
            HashSet<string> paths = new HashSet<string>();
            paths.Add(projectPath);
            
            string relLvlBackgroundPath = null;
            LDtkJsonDigger.GetUsedSeparateLevelBackgrounds(levelPath, ref relLvlBackgroundPath);
            if (!string.IsNullOrEmpty(relLvlBackgroundPath))
            {
                LDtkRelativeGetterLevelBackground levelGetter = new LDtkRelativeGetterLevelBackground();
                string levelBgPath = levelGetter.GetPathRelativeToPath(projectPath, relLvlBackgroundPath);
                paths.Add(levelBgPath);
            }

            //we get all possible assets that is possibly available as the serialized information.  
            List<ParsedMetaData> allSerializedAssets = LDtkDependencyUtil.GetMetaDatasAtProjectPath(projectPath);

            HashSet<string> entities = new HashSet<string>();
            LDtkJsonDigger.GetUsedEntities(levelPath, ref entities);
            
            HashSet<string> intGridValues = new HashSet<string>();
            LDtkJsonDigger.GetUsedIntGridValues(levelPath, ref intGridValues);
            
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