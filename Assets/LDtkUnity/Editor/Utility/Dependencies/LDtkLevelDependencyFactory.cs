using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LDtkUnity.Editor
{
    internal sealed class DugDependencyDataLevel
    {
        //public string Background;
        public HashSet<string> Entities = new HashSet<string>();
        public HashSet<string> IntGridValues = new HashSet<string>();

        public override string ToString()
        {
            return //$"Background: {Background}\n" +
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
            
            //Don't depend on the base project, nor it's sub assets.
            //paths.Add(projectPath);
            
            //base project dependency is not required because many of the artifacts are reference-based anyway, so the levels only need to scoop up what the project makes.
            //doesn't need to depend on tileset files either; the artifacts get loaded anyway due to the project importing before levels. Again, scooping up.
            //We only need to depend on prefabs because those absolutely need to be updated in the level's result.
            //in this case, it's:
            //- LDtkIntGridValue assets (so tilemaps tile references are updated properly),
            //- Entity prefabs, and level prefab (so that prefabs in the import result are updated, because normally they aren't after editing a prefab)
            
            //levels are not updated with the new/removed dependencies when changed in the project importer inspector. therefore, we need to at least specifically depend on the project meta data.
            //paths.Add(projectPath + ".meta");
            //NEW DEVELOPMENT: we should only depend on the meta file of the project, but not the source asset.
            //We ended up choosing to generate a new file. Here's how its referenced.
            string pathToProjectConfig = LDtkConfigData.GetPath(projectPath);
            if (File.Exists(pathToProjectConfig))
            {
                paths.Add(pathToProjectConfig);
            }
            else
            {
                LDtkDebug.LogWarning($"Could not find the level's project configuration file from {levelPath}. This will make levels not update properly when the project is changed in the importer inspector.");
            }

            //Within the above types of assets we want to depend on, we only want to depend on assets that are used in a particular level as to further prevent unnecessary reimports. 
            DugDependencyDataLevel depends = new DugDependencyDataLevel();
            LDtkJsonDigger.GetSeparateLevelDependencies(levelPath, ref depends);
            
            HashSet<string> entities = depends.Entities;
            HashSet<string> intGridValues = depends.IntGridValues;
            
            //As things stand, dependency on level background is not required, because the reference to the background is a sprite, and even if it was missing, there'd be bigger errors to solve anyways.
            //DependOnLevelBackground(depends, projectPath, paths);

            //we get all possible assets that is possibly available as the serialized information.  
            string[] projectLines = LDtkDependencyUtil.LoadMetaLinesAtPath(projectPath);
            List<ParsedMetaData> allSerializedAssets = LDtkDependencyUtil.GetMetaDatasForDependencies(projectLines);
            
            foreach (ParsedMetaData data in allSerializedAssets)
            {
                //DEPEND ON CUSTOM LEVEL PREFAB
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
        
        private static void DependOnLevelBackground(DugDependencyDataLevel depends, string projectPath, HashSet<string> paths)
        {
            string relLvlBackgroundPath = "";//depends.Background;
            if (relLvlBackgroundPath != null)
            {
                if (!string.IsNullOrEmpty(relLvlBackgroundPath))
                {
                    LDtkRelativeGetterLevelBackground levelGetter = new LDtkRelativeGetterLevelBackground();
                    string levelBgPath = levelGetter.GetPathRelativeToPath(projectPath, relLvlBackgroundPath);
                    
                    //DEPEND ON LEVEL BACKGROUND
                    paths.Add(levelBgPath);
                }
            }
        }
    }
}