using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class ExportUnityPackage
    {
        [Serializable]
        public class PackageJson
        {
            public string name;
            public string version;
        }
        
        [MenuItem("LDtkUnity/Export .unitypackage")]
        private static void ExportPackage()
        {
            string text = File.ReadAllText("Assets/LDtkUnity/package.json");
            
            text = text.Replace("\n", string.Empty); 
            text = text.Replace("\r", string.Empty); 
            text = text.Replace("\t", string.Empty);

            PackageJson info = JsonUtility.FromJson<PackageJson>(text);
            
            LDtkPathUtility.TryCreateDirectory("ExportUnityPackage");
            AssetDatabase.ExportPackage("Assets/LDtkUnity", $"ExportUnityPackage/{info.name}-{info.version}.unitypackage", ExportPackageOptions.Recurse);
        }
    }
}
