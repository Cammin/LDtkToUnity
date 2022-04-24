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
    internal static class LDtkDependencyFactory
    {
        private const string NULL = "{instanceID: 0}";
        
        public static string[] GatherProjectDependencies(string projectPath)
        {
            string[] lines = LoadMetaLinesAtProjectPath(projectPath);
            List<ParsedMetaData> datas = GetMetaDatas(lines);
            string[] assetPaths = datas.Select(p => p.GetAssetPath).ToArray();
            
            foreach (string path in assetPaths)
            {
                LDtkBuilderDependencies.TestLogDependencySet("GatherProjectDependencies", projectPath, path);
            }

            return assetPaths;
        }
        
        public static string[] GatherLevelDependencies(string levelPath)
        {
            List<string> paths = new List<string>();
            
            string projectPath = new LDtkRelativeGetterProjectImporter().GetPath(levelPath, levelPath);
            paths.Add(projectPath);
            
            
            string[] lines = LoadMetaLinesAtProjectPath(projectPath);
            IEnumerable<string> entities = GetUsedEntitiesInJsonLevel(levelPath);
            List<ParsedMetaData> datas = GetMetaDatas(lines);
            
            foreach (string entity in entities)
            {
                foreach (ParsedMetaData data in datas)
                {
                    if (entity == data.Name)
                    {
                        paths.Add(data.GetAssetPath);        
                    }
                }
            }
            foreach (string path in paths)
            {
                LDtkBuilderDependencies.TestLogDependencySet("GatherLevelDependencies", levelPath, path);
            }
            return paths.ToArray();
        }

        private static string[] LoadMetaLinesAtProjectPath(string projectPath)
        {
            string metaPath = projectPath + ".meta";

            if (!File.Exists(metaPath))
            {
                LDtkDebug.LogError($"The project meta file cannot be found at \"{metaPath}\", Check that there are no broken paths");
            }

            string[] lines = File.ReadAllLines(metaPath, Encoding.ASCII);
            return lines;
        }


        private static List<ParsedMetaData> GetMetaDatas(string[] lines)
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

        private struct ParsedMetaData
        {
            public string Name;
            public string Guid;
            
            public string GetAssetPath => AssetDatabase.GUIDToAssetPath(Guid);
            public override string ToString() => $"\"{Name}\" {Guid}";
        }

        private static IEnumerable<string> GetUsedEntitiesInJsonLevel(string levelJsonFilePath)
        {
            HashSet<string> entities = new HashSet<string>();
            StreamReader sr = File.OpenText(levelJsonFilePath);
            try
            {
                JsonTextReader reader = new JsonTextReader(sr);
                while (reader.Read())
                {
                    if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "entityInstances")
                        continue;
                    
                    int entityInstancesDepth = reader.Depth;
                    while (reader.Depth >= entityInstancesDepth && reader.Read())
                    {
                        if (reader.Depth != entityInstancesDepth + 2 || reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "__identifier")
                            continue;

                        reader.Read();
                        entities.Add((string)reader.Value);
                    }
                }
            }
            finally
            {
                sr.Close();
            }
            return entities;
        }

        private static void LogRemainingReader(JsonTextReader reader)
        {
            while (reader.Read())
            {
                object value = reader.Value;
                if (value != null)
                {
                    string msg = "";
                    {
                        string tokenText = Colorize(reader.TokenType.ToString(), "blue");
                        string valueText = Colorize(value.ToString(), "navy");
                        msg += $"{GetDepthString(reader)} {tokenText} {valueText}";
                    }

                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        reader.Read();
                        value = reader.Value;
                        string tokenText = Colorize(reader.TokenType.ToString(), "blue");

                        string nulNull = value != null ? value.ToString() : "null";
                        string valueText = Colorize(nulNull, "navy");
                        
                        msg += $"{tokenText} {valueText}";
                    }
                    
                    //msg = $"{GetDepthString(reader)} : {}";

                    Debug.Log(msg);
                }
                else
                {
                    string tokenText = Colorize(reader.TokenType.ToString(), "orange");
                    string msg = $"{GetDepthString(reader)} {tokenText}";
                    Debug.Log(msg);
                }
            }
        }

        private static string GetDepthString(JsonTextReader reader)
        {
            string depth = Colorize(reader.Depth.ToString(), "teal");

            for (int i = 0; i < reader.Depth; i++)
            {
                depth += "|       ";
            }

            return depth;
        }

        private static string Colorize(string text, string color) => $"<color={color}>{text}</color>";
    }
}