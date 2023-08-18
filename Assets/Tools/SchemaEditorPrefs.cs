using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utf8Json;

namespace LDtkUnity.Editor
{
    [InitializeOnLoad]
    internal class SchemaEditorPrefs
    {
        private const string KEY = "LDtkSchemaPaths";
        public static readonly Dictionary<string, string[]> PathedItems;
            
        static SchemaEditorPrefs()
        {
            string json = EditorPrefs.GetString(KEY, null);

            if (json != null)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(json);
                PathedItems = JsonSerializer.Deserialize<Dictionary<string, string[]>>(bytes);
            }
            
            if (PathedItems == null)
            {
                PathedItems = new Dictionary<string, string[]>();
            }
        }

        public static void CachePaths(string[] paths)
        {
            PathedItems.Clear();

            foreach (string path in paths)
            {
                string[] files = Directory.GetFiles(path).Where(s => s.EndsWith(".cs")).ToArray();
                string[] fileNames = files.Select(Path.GetFileName).ToArray();

                PathedItems.Add(path, fileNames);
            }

            var json = JsonSerializer.Serialize(PathedItems);
            var str = System.Text.Encoding.UTF8.GetString(json);
            EditorPrefs.SetString(KEY, str);
                
            string[] keyValues = PathedItems.Select(p => $"{p.Key}:\n{string.Join(",\n", p.Value)}").ToArray();
            Debug.Log($"Cached files: {string.Join("\n\n", keyValues)}");
        }
    }
}