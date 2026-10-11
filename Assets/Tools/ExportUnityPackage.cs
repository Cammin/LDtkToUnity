using System;
using System.IO;
using UnityEditor;
using UnityEditor.AssetPackage;

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

            Package.Export(new ExportPackageParameters("Assets/LDtkUnity", destinationExportPath, "", ExportPackageOptions.Recurse));
            //AssetDatabase.ExportPackage("Assets/LDtkUnity", destinationExportPath, ExportPackageOptions.Recurse);
            ShowExplorer(destinationExportPath);
        }
        
        public static void ShowExplorer(string itemPath)
        {
            itemPath = itemPath.Replace(@"/", @"\");
            System.Diagnostics.Process.Start("explorer.exe", "/select,"+itemPath);
        }
    }
}
