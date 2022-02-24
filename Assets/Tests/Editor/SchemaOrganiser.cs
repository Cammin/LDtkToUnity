using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tests.Editor
{
    public class SchemaOrganiser : EditorWindow
    {
        [SerializeField] private string _schema;
    
        [MenuItem("LDtkUnity/Schema Organiser")]
        private static void CreateWindow()
        {
            SchemaOrganiser window = GetWindow<SchemaOrganiser>();
            window.Show();
        }

        private void OnGUI()
        {
            _schema = EditorGUILayout.TextArea(_schema, GUILayout.Height(EditorGUIUtility.singleLineHeight * 10));
            
            if (GUILayout.Button("Paste Text"))
            {
                PasteNewText();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Reset Files"))
            {
                DeleteScripts();
                AssetDatabase.Refresh();
            }
        }

        private void PasteNewText()
        {
            string path = Path.Combine(Application.dataPath, "LDtkUnity/Runtime/Data/Schema");

            if (!Directory.Exists(path))
            {
                Debug.LogError($"path not valid: {path}");
                return;
            }

            string csPath = Path.Combine(path, "LdtkJson.cs");
            File.WriteAllText(csPath, _schema);
            //Debug.Log($"write at {csPath}");
            
            
            
        }
        
        private void DeleteScripts()
        {
            string path = Path.Combine(Application.dataPath, "LDtkUnity/Runtime/Data/Schema");

            if (!Directory.Exists(path))
            {
                Debug.LogError($"path not valid: {path}");
                return;
            }
            
            DeleteAtPath(path, "Converter");
            DeleteAtPath(path, "Definition");
            DeleteAtPath(path, "Enum");
            DeleteAtPath(path, "Instance");
            DeleteAtPath(path, "Level");
        }

        private static void DeleteAtPath(string path, string extension)
        {
            string dir = Path.Combine(path, extension);
            string[] paths = Directory.GetFiles(dir);
            foreach (string s in paths)
            {
                //Debug.Log($"delte at {s}");
                File.Delete(s);
            }
            File.WriteAllText(Path.Combine(dir, "stub.txt"), "stub");
        }
    }
}
