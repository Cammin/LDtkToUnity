﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Handles loading meta file lines and processing what should happen with them
    /// </summary>
    internal static class LDtkDependencyUtil
    {
        private const string NULL = "{instanceID: 0}";
        
        public static bool ShouldDependOnNothing(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.Contains(LDtkJsonImporter.REIMPORT_ON_DEPENDENCY_CHANGE))
                {
                    return line.Contains(": 0");
                }
            }

            //if we didnt find the serialized value, then assume that we should just depend on everything
            return false;
        }
        
        /// <summary>
        /// Example:
        /// _overrideTexture: {fileID: 2800000, guid: e00cd3b651e599f49933952f529b1a70, type: 3}
        /// _overrideTexture: {instanceID: 0}
        /// </summary>
        /// <returns></returns>
        public static string GetTilesetImporterOverrideTexturePath(string tilesetImporterMetaPath)
        {
            string line = FindLineContaining(tilesetImporterMetaPath, LDtkTilesetImporter.OVERRIDE_TEXTURE);

            if (line == null)
            {
                //then the line wasn't found or generated from new serialization, it's fine to ignore
                return null;
            }
            
            if (line.Contains(NULL))
            {
                return null;
            }

            int indexOf = line.IndexOf("guid:", StringComparison.InvariantCulture);
            string substring = line.Substring(indexOf);
            string guid = substring.Split(' ')[1];
            guid = guid.TrimEnd(',');

            return AssetDatabase.GUIDToAssetPath(guid);
        }

        private static void LogMetas(List<ParsedMetaData> metas)
        {
            foreach (ParsedMetaData meta in metas)
            {
                LDtkDebug.Log($"GotMeta: {meta}");
            }
        }

        public static string FindLineContaining(string filePath, string searchString)
        {
            string metaPath = filePath + ".meta";

            if (!File.Exists(metaPath))
            {
                LDtkDebug.LogError($"The tileset meta file cannot be found at \"{metaPath}\", Check that there are no broken paths.");
                return null;
            }
            
            using (StreamReader reader = new StreamReader(metaPath, Encoding.ASCII))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(searchString))
                    {
                        return line;
                    }
                }
            }
            return null;
        }
        
        public static string[] LoadMetaLinesAtPath(string projectPath)
        {
            string metaPath = projectPath + ".meta";

            if (!File.Exists(metaPath))
            {
                LDtkDebug.LogError($"The project/level meta file cannot be found at \"{metaPath}\", Check that there are no broken paths. Most likely the project was renamed but not re-saved in LDtk yet. save the project in LDtk to potentially fix this problem");
                return Array.Empty<string>();
            }

            string[] lines = File.ReadAllLines(metaPath, Encoding.ASCII);
            return lines;
        }

        public static List<ParsedMetaData> GetMetaDatasForDependencies(string[] lines)
        {
            List<ParsedMetaData> metaData = new List<ParsedMetaData>();
            

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (AddCustomPrefabLevel(line, metaData))
                {
                    continue;
                }

                if (!line.Contains(LDtkAsset<Object>.PROPERTY_KEY))
                {
                    continue;
                }
                string nextLine = lines[i+1];
                TryAddLDtkAsset(nextLine, line, metaData);
            }

            return metaData;
        }

        private static void TryAddLDtkAsset(string nextLine, string line, List<ParsedMetaData> metaData)
        {
            if (nextLine.Contains(NULL))
            {
                return;
            }
            
            ParsedMetaData meta = new ParsedMetaData();

            //name
            string[] strings = line.Split(' ');
            meta.Name = strings[4];

            //guid
            int indexOf = nextLine.IndexOf("guid:", StringComparison.InvariantCulture);
            string substring = nextLine.Substring(indexOf);
            string guid = substring.Split(' ')[1];
            meta.Guid = guid.TrimEnd(',');

            //Debug.Log($"parsed array item {meta}");
            metaData.Add(meta);
        }
        
        private static bool AddCustomPrefabLevel(string line, List<ParsedMetaData> metaData)
        {
            //RULE: we need to depend on the custom level if: We are a project file without separate levels, or if we are a level file, period. 
            //custom level prefabs can look like this
            //_customLevelPrefab: {fileID: 8588321673598725224, guid: fe05cfa93bba52540971cb633e22bfbe, type: 3}
            //_customLevelPrefab: {instanceID: 0}
            if (line.Contains(LDtkProjectImporter.CUSTOM_LEVEL_PREFAB) && !line.Contains(NULL))
            {
                ParsedMetaData meta = new ParsedMetaData();

                //name
                string[] tokens = line.Split(' ');
                meta.Name = tokens[2].TrimEnd(':');

                //guid
                int indexOf = line.IndexOf("guid:", StringComparison.InvariantCulture);
                string substring = line.Substring(indexOf);
                string guid = substring.Split(' ')[1];
                meta.Guid = guid.TrimEnd(',');

                metaData.Add(meta);

                //Debug.Log($"parsed custom level {meta}");
                return true;
            }

            return false;
        }

        public static void TestLogDependencySet(string functionName, string importerPath, string dependencyPath)
        {
            //used for testing
            //LDtkDebug.Log($"{functionName} <color=yellow>{Path.GetFileNameWithoutExtension(importerPath)}</color>:<color=navy>{Path.GetFileName(dependencyPath)}</color>");
        }
    }
}