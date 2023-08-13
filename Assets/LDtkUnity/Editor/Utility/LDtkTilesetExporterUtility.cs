using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkTilesetExporterUtil
    {
        private const string EXPORT_ZIP = "LDtkTilesetExporter.zip";
        private const string EXPORT_APP = "ExportTilesetDefinition.exe";

        [MenuItem("UnzipToProject/Unzip")]
        public static void UnzipToProject()
        {
            string pathToZip = PathToZip();
            string destDir = PathToLibraryDir();
            
            if (Directory.Exists(destDir))
            {
                //delete so it can overwrite
                if (EditorUtility.DisplayDialog("delete", $"delete {destDir}", "ok", "cancel"))
                {
                    Directory.Delete(destDir);
                }
            }

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            
            Debug.Log($"pathToZip {pathToZip}");
            Debug.Assert(File.Exists(pathToZip), "File.Exists(pathToZip)");
            
            Debug.Log($"destDir {destDir}");
            Debug.Assert(Directory.Exists(destDir), "Directory.Exists(destDir)");
            
            ZipUtil.Extract(pathToZip, destDir);
        }

        [MenuItem("UnzipToProject/LogPathToExe")]
        public static void LogPathToExe()
        {
            Debug.Log(PathToExe());
        }
        
        public static string PathToLibraryDir()
        {
            string destDir = Application.dataPath;
            destDir = Path.Combine(destDir, "..", "Library", "LDtkTilesetExporter");
            destDir = Path.GetFullPath(destDir);
            //Debug.Assert(Directory.Exists(destDir), "path to project dir doesnt exist");
            return destDir;
        }
        
        public static string PathToZip()
        {
            string packagePath = Path.Combine(LDtkInternalUtility.ASSETS, EXPORT_ZIP);
            if (File.Exists(packagePath)) return packagePath;
            
            string assetsPath = Path.Combine(LDtkInternalUtility.PACKAGES, EXPORT_ZIP);
            if (File.Exists(assetsPath)) return assetsPath;

            return null;
        }
        public static string PathToExe()
        {
            string exePath = Path.Combine(PathToLibraryDir(), EXPORT_APP);
            Debug.Assert(File.Exists(exePath), "exe path doesnt exist");
            return exePath;
        }
    }
}