using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tests.Editor
{
    public class SchemaOrganiser : EditorWindow
    {
        [SerializeField] private string _schema;
        
        private Vector2 _scroll;
        private Dictionary<string, string[]> _pathedItems = new Dictionary<string, string[]>();

        [MenuItem("LDtkUnity/Schema Organiser")]
        private static void CreateWindow()
        {
            SchemaOrganiser window = GetWindow<SchemaOrganiser>();
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("1. Create the base file");
            if (GUILayout.Button("Paste Json"))
            {
                PasteNewText();
                AssetDatabase.Refresh();
            }

            EditorGUILayout.LabelField("2. Cache file paths for automated relocation");
            if (GUILayout.Button("Cache file paths"))
            {
                CachePrevPaths();
            }
            
            EditorGUILayout.LabelField("3. Delete old scripts");
            if (GUILayout.Button("Delete Scripts"))
            {
                DeleteScripts();
                AssetDatabase.Refresh();
            }

            EditorGUILayout.LabelField("4. Make stubs at each path so that rider doesn't auto delete the folders");
            if (GUILayout.Button("Create Stubs"))
            {
                CreateStubs();
                AssetDatabase.Refresh();
            }
            
            EditorGUILayout.LabelField("5. Manually Go into rider and split each class into a separate file");
            
            EditorGUILayout.LabelField("6. Move split files into their appropriate places where applicable");
            if (GUILayout.Button("Move new files"))
            {
                MoveNewFilesIntoOldLocations();
            }
            
            EditorGUILayout.LabelField("7. Delete stubs to clean up");
            
            if (GUILayout.Button("Delete Stubs"))
            {
                DeleteStubs();
                AssetDatabase.Refresh();
            }
            
            EditorGUILayout.Space(20);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            _schema = EditorGUILayout.TextArea(_schema, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndScrollView();
        }

        private void MoveNewFilesIntoOldLocations()
        {
            if (!ValidateRootPath())
            {
                return;
            }

            foreach (KeyValuePair<string,string[]> pathedItem in _pathedItems)
            {
                string category = pathedItem.Key;
                foreach (string fileName in pathedItem.Value)
                {
                    string rootPath = RootPath();
                    string startPath = Path.Combine(rootPath, fileName).Replace('\"', '/'); 
                    string destPath = Path.Combine(category, fileName).Replace('\"', '/'); 

                    /*if (!Directory.Exists(destPath))
                    {
                        Debug.LogError($"Dest directory doesn't exist for {destPath}");
                        continue;
                    }*/
                    Debug.Log($"Attempt move from {startPath} to {destPath}");
                    
                    
                    if (!File.Exists(startPath))
                    {
                        Debug.LogWarning($"New file doesn't exist for {fileName}, maybe it's a new file. It will not be moved so it's manually handled");
                        continue;
                    }
                    
                    File.Move(startPath, destPath);
                }
            }
        }

        private void CreateStubs()
        {
            if (!ValidateRootPath())
            {
                return;
            }

            foreach (string s in GetPaths())
            {
                CreateStub(s);
            }
        }
        
        private void CachePrevPaths()
        {
            if (!ValidateRootPath())
            {
                return;
            }
            
            _pathedItems.Clear();
            
            foreach (string path in GetPaths())
            {
                string[] files = Directory.GetFiles(path).Where(s => s.EndsWith(".cs")).ToArray();
                string[] fileNames = files.Select(Path.GetFileName).ToArray();
                
                _pathedItems.Add(path, fileNames);
            }

            string[] keyValues = _pathedItems.Select(p => $"{p.Key}:\n{string.Join(",\n", p.Value)}").ToArray();
            Debug.Log($"Cached files: {string.Join("\n\n", keyValues)}");
        }
        
        private void DeleteStubs()
        {
            if (!ValidateRootPath())
            {
                return;
            }

            foreach (string s in GetPaths())
            {
                DeleteStub(s);
            }
        }

        private bool ValidateRootPath()
        {
            string path = RootPath();

            if (Directory.Exists(path))
            {
                return true;
            }
            
            Debug.LogError($"path not valid: {path}");
            return false;
        }


        private string RootPath()
        {
            return Path.Combine(Application.dataPath, "LDtkUnity/Runtime/Data/Schema");
        }
        private string[] GetPaths()
        {
            string path = RootPath();
            return new[]
            {
                Path.Combine(path, "Converter"),
                Path.Combine(path, "Definition"),
                Path.Combine(path, "Enum"),
                Path.Combine(path, "Instance"),
                Path.Combine(path, "Level"),
            };
        }
        

        private void CreateStub(string path)
        {
            string dir = Path.Combine(path, "stub.txt");
            File.WriteAllText(dir, "");
        }
        private void DeleteStub(string path)
        {
            string dir = Path.Combine(path, "stub.txt");
            File.Delete(dir);
            dir += ".meta";
            File.Delete(dir);
        }

        private void PasteNewText()
        {
            string path = RootPath();

            if (!ValidateRootPath())
            {
                return;
            }

            string csPath = Path.Combine(path, "LdtkJson.cs");
            File.WriteAllText(csPath, _schema);
        }
        
        private void DeleteScripts()
        {
            if (!ValidateRootPath())
            {
                return;
            }

            foreach (string s in this.GetPaths())
            {
                DeleteContentsAtPath(s);
            }
        }

        private static void DeleteContentsAtPath(string dir)
        {
            string[] paths = Directory.GetFiles(dir);
            foreach (string s in paths)
            {
                File.Delete(s);
            }
        }
    }
}
