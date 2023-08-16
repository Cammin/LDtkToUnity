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
            
            //todo Create & depend on the tileset definition files!


            HashSet<string> tilesetDefNames = new HashSet<string>();
            LDtkJsonDigger.GetTilesetDefNames(projectPath, ref tilesetDefNames);

            foreach (string defName in tilesetDefNames)
            {
                string tilesetPath = LDtkProjectImporter.TilesetImporterPath(projectPath, defName);
                paths.Add(tilesetPath);       
            }

            /*HashSet<string> texturePaths = new HashSet<string>();
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
            }*/
            
            
            //export and depend on a tileset file HERE.

            if (projectPath.Contains("AutoLayers_1_basic"))
            {
                /*Debug.Log("EXPORT AutoLayers_1_basic!");

                LDtkTilesetDefExporter exporter = new LDtkTilesetDefExporter(projectPath, 16);
                LdtkJson json = JsonSerializer.Deserialize<LdtkJson>(File.ReadAllText(projectPath));
                exporter.ExportTilesetDefinitions(json);

                TilesetDefinition[] defs = json.Defs.Tilesets;
                foreach (TilesetDefinition def in defs)
                {
                    //paths.Add(LDtkTilesetDefExporter.TilesetExportPath(projectPath, def));
                }*/
            }
            
            
            //Should we reiport projecct/levels if the tileset's importer changed file data?
            //Lots of tile data simply changes and should still work fine. even if we don't reimport the project.
            //Maybe in the future, we will need to setup a dependency.
            //But for now, it's just sprites and tiles! it gets changed anyways due to the project exporting a new tileset def when ti's changed anyways.
            /*if (path == "Assets/Samples/Samples/AutoLayers_1_basic.ldtk")
            {
                Debug.Log("DEPEND");
                _previousDependencies = _previousDependencies.Append("Assets/Samples/Samples/AutoLayers_1_basic/Cavernas_by_Adam_Saltsman.ldtkt").ToArray();
            }*/

            
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