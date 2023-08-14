using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    internal static class LDtkTilesetExporterUtil
    {
        private const string EXPORT_ZIP = "LDtkTilesetExporter.zip";
        private const string EXPORT_APP = "ExportTilesetDefinition.exe";

        //[MenuItem("UnzipToProject/Unzip")]
        public static void UnzipToLibrary()
        {
            string pathToZip = PathToZip();
            EditorUtility.DisplayProgressBar("Install", pathToZip, 0);
                
            string destDir = PathToLibraryDir();
            
            
            LDtkPathUtility.TryCreateDirectory(destDir);
            if (Directory.Exists(destDir))
            {
                DirectoryInfo di = new DirectoryInfo(destDir);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete(); 
                }
            }

            Debug.Assert(File.Exists(pathToZip), "File.Exists(pathToZip)");
            Debug.Assert(Directory.Exists(destDir), "Directory.Exists(destDir)");
            
            ZipUtil.Extract(pathToZip, destDir);
            Debug.Log($"Extracted the tileset export app to \"{destDir}\"");
        }

        /*//[MenuItem("UnzipToProject/AppVersion")]
        private static void CheckAppVersion()
        {
            Debug.Log($"app version up to date? {GetAppUpToDate()}");
        }
        
        //[MenuItem("UnzipToProject/LogPathToExe")]
        public static void LogPathToExe()
        {
            Debug.Log(PathToExe());
        }*/
        
        public static string PathToLibraryDir()
        {
            string destDir = Application.dataPath;
            destDir = Path.Combine(destDir, "..", "Library", "LDtkTilesetExporter");
            destDir = Path.GetFullPath(destDir);
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
            return exePath;
        }
        
        public static bool GetAppUpToDate()
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(PathToExe());
            Version version = new Version(info.FileVersion);
            Version requiredVersion = new Version(LDtkImporterConsts.EXPORT_APP_VERSION_REQUIRED);
            //Debug.Log($"app version {version}, required {requiredVersion}");
            return version == requiredVersion;
        }
    }
}