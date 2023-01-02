using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class ExportUnityPackage
    {
        [MenuItem("LDtkUnity/Export UnityPackage", false, 10)]
        private static void Make()
        {
            string date = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            
            string fileName = $"Assets/../LDtkUnity-{date}.unitypackage";
            string destinationExportPath = $"Assets/../Export/{fileName}";
            
            if (!Directory.Exists(destinationExportPath))
            {
                Directory.CreateDirectory(destinationExportPath);
            }

            AssetDatabase.ExportPackage("Assets/LDtkUnity", destinationExportPath, ExportPackageOptions.Recurse);
            ShowExplorer(destinationExportPath);
        }
        
        public static void ShowExplorer(string itemPath)
        {
            itemPath = itemPath.Replace(@"/", @"\");
            System.Diagnostics.Process.Start("explorer.exe", "/select,"+itemPath);
        }
    }
}
