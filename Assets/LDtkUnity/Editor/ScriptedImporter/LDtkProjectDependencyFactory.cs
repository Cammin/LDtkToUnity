using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    internal static class LDtkProjectDependencyFactory
    {
        private const string NULL = "{instanceID: 0}";
        
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

            string[] lines = LoadMetaLinesAtProjectPath(projectPath);
            List<ParsedMetaData> datas = GetMetaDatas(lines);
            
            string[] assetPaths = datas.Select(p => p.GetAssetPath).ToArray();
            foreach (string path in assetPaths)
            {
                LDtkBuilderDependencies.TestLogDependencySet("GatherProjectDependencies", projectPath, path);
            }

            return assetPaths;
        }

        public static string[] LoadMetaLinesAtProjectPath(string projectPath)
        {
            string metaPath = projectPath + ".meta";

            if (!File.Exists(metaPath))
            {
                LDtkDebug.LogError($"The project meta file cannot be found at \"{metaPath}\", Check that there are no broken paths. Most likely the project was renamed but not re-saved in LDtk yet. save the project in LDtk to potentially fix this problem");
                return Array.Empty<string>();
            }

            string[] lines = File.ReadAllLines(metaPath, Encoding.ASCII);
            return lines;
        }


        public static List<ParsedMetaData> GetMetaDatas(string[] lines)
        {
            List<ParsedMetaData> metaData = new List<ParsedMetaData>();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!line.Contains(LDtkAsset<Object>.PROPERTY_KEY))
                {
                    continue;
                }
                string nextLine = lines[i+1];
                if (nextLine.Contains(NULL))
                {
                    continue;
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

                //Debug.Log(meta);
                metaData.Add(meta);
            }

            return metaData;
        }

        internal struct ParsedMetaData
        {
            public string Name;
            public string Guid;
            
            public string GetAssetPath => AssetDatabase.GUIDToAssetPath(Guid);
            public override string ToString() => $"\"{Name}\" {Guid}";
        }
    }
}