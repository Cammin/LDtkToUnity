using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    public static class LDtkJsonParser
    {
        private delegate bool JsonDigAction<T>(JsonTextReader reader, out T result);
        
        public static IEnumerable<string> GetUsedEntitiesInJsonLevel(string levelJsonFilePath)
        {
            DigIntoJson(levelJsonFilePath, GetUsedEntitiesInJsonLevelReader, out IEnumerable<string> result);
            return result;
        }
        public static bool GetIsExternalLevels(string projectPath, out bool result) => DigIntoJson(projectPath, GetIsExternalLevelsInReader, out result);
        public static bool GetDefaultGridSize(string projectPath, out int result) => DigIntoJson(projectPath, GetDefaultGridSizeInReader, out result); //todo setup test framework function for this
        
        private static bool DigIntoJson<T>(string path, JsonDigAction<T> actionThing, out T result)
        {
            Profiler.BeginSample($"DigIntoJson {typeof(T).Name}");
            StreamReader sr = File.OpenText(path);
            bool success;
            result = default;
            try
            {
                JsonTextReader reader = new JsonTextReader(sr);
                success = actionThing.Invoke(reader, out result);
            }
            finally
            {
                sr.Close();
            }
            
            Profiler.EndSample();

            if (success)
            {
                //Debug.Log($"Dug json and got {result} for {actionThing.Method.Name} at {path}");
                return true;
            }
            
            LDtkDebug.LogError($"Issue digging into the json for {path}");
            return false;
        }

        private static bool GetUsedEntitiesInJsonLevelReader(JsonTextReader reader, out IEnumerable<string> result)
        {
            HashSet<string> entities = new HashSet<string>();
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

            result = entities;
            return true;
        }
        private static bool GetIsExternalLevelsInReader(JsonTextReader reader, out bool result)
        {
            result = false;
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "externalLevels")
                    continue;

                bool? value = reader.ReadAsBoolean();
                if (value == null)
                    break;
                
                result = value.Value;
                return true;
            }
            return false;
        }

        private static bool GetDefaultGridSizeInReader(JsonTextReader reader, out int result)
        {
            result = 0;
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "defaultGridSize")
                    continue;

                int? value = reader.ReadAsInt32();
                if (value == null)
                    break;
                
                result = value.Value;
                return true;
            }
            return false;
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