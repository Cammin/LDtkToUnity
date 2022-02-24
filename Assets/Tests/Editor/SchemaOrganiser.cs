using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tests.Editor
{
    public class SchemaOrganiser : EditorWindow
    {
        private Vector2 scroll;
        
        [SerializeField] private string _schema;
    
        [MenuItem("LDtkUnity/Schema Organiser")]
        private static void CreateWindow()
        {
            SchemaOrganiser window = GetWindow<SchemaOrganiser>();
            window.Show();
        }

        private void OnGUI()
        {
            
            if (GUILayout.Button("Paste Json"))
            {
                PasteNewText();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Delete Scripts"))
            {
                DeleteScripts();
                AssetDatabase.Refresh();
            }
            
            if (GUILayout.Button("Create Stubs"))
            {
                CreateStubs();
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("Delete Stubs"))
            {
                DeleteStubs();
                AssetDatabase.Refresh();
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);
            _schema = EditorGUILayout.TextArea(_schema, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndScrollView();
        }

        
        private void CreateStubs()
        {
            string path = RootPath();

            if (!Directory.Exists(path))
            {
                Debug.LogError($"path not valid: {path}");
                return;
            }

            foreach (string s in GetPaths())
            {
                CreateStub(s);
            }
        }
        private void DeleteStubs()
        {
            string path = RootPath();

            if (!Directory.Exists(path))
            {
                Debug.LogError($"path not valid: {path}");
                return;
            }

            foreach (string s in GetPaths())
            {
                DeleteStub(s);
            }
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

            if (!Directory.Exists(path))
            {
                Debug.LogError($"path not valid: {path}");
                return;
            }

            string csPath = Path.Combine(path, "LdtkJson.cs");
            File.WriteAllText(csPath, _schema);
        }
        
        private void DeleteScripts()
        {
            string path = RootPath();

            if (!Directory.Exists(path))
            {
                Debug.LogError($"path not valid: {path}");
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
