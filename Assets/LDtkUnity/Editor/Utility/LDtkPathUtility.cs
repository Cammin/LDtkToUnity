
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkPathUtility
    {
        public static string CleanPath(string directory)
        {
            string doubleDotsCleaned = SimplifyPathWithDoubleDots(directory);
            return CleanPathSlashes(doubleDotsCleaned);
        }

        public static string CleanPathSlashes(string directory)
        {
            return directory.Replace('\\', '/');
        }
        private static string SimplifyPathWithDoubleDots(string inputPath)
        {
            string fullPath = CleanPathSlashes(Path.GetFullPath(inputPath));
            
            if (fullPath.StartsWith(Application.dataPath))
            {
                return "Assets" + fullPath.Substring(Application.dataPath.Length);
            }

            Debug.LogWarning($"LDtk: Cannot specify a folder outside of the Unity project\n{fullPath}");
            return fullPath;
        }

        public static void TryCreateDirectoryForFile(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            TryCreateDirectory(directory);
        }
        public static void TryCreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// //this directory is a folder that stands aside the project asset.
        /// Folder uses the same name as the original object
        /// </summary>
        public static string SiblingDirectoryOfAsset(Object obj)
        {
            string objAssetPath = AssetDatabase.GetAssetPath(obj);
            return SiblingDirectoryOfAssetPath(objAssetPath);
        }
        public static string SiblingDirectoryOfAssetPath(string objAssetPath)
        {
            string objAssetDirectory = Path.GetDirectoryName(objAssetPath);
            string fileName = Path.GetFileNameWithoutExtension(objAssetPath);
            string directory = $"{objAssetDirectory}/{fileName}";

            return CleanPath(directory);
        }
        public static string DirectoryOfAssetPath(string objAssetPath)
        {
            string objAssetDirectory = Path.GetDirectoryName(objAssetPath);
            return CleanPath(objAssetDirectory);
        }
        
        public static string AbsolutePathToAssetsPath(string absolutePath)
        {
            if (absolutePath.StartsWith(Application.dataPath)) 
            {
                return "Assets" + absolutePath.Substring(Application.dataPath.Length);
            }

            Debug.LogWarning("Did not convert absolute path to assets path");
            return absolutePath;
        }

        public static string AssetsPathToAbsolutePath(string assetsPath)
        {
            if (!assetsPath.Contains("Assets"))
            {
                Debug.LogError("Incorrect string format");
            }
            
            return Application.dataPath + assetsPath.Remove(0, "Assets".Length);
        }

        public static string GetDirectoryOfSelectedPath(string title)
        {
            string startFrom = Application.dataPath;
            if (AssetDatabase.Contains(Selection.activeObject))
            {
                string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                startFrom = AssetsPathToAbsolutePath(assetPath);
                startFrom = Path.GetDirectoryName(startFrom);
            }
            
            string directory = EditorUtility.OpenFolderPanel(title, startFrom, "");

            if (string.IsNullOrEmpty(directory))
            {
                Debug.LogError("LDtk: Did not solve a path correctly, empty path specified");
                return "";
            }

            //if the path involves a hidden unity folder (maybe symbolic link reasons), then it will break. Ensure crashes cannot happen
            directory += '/';
            if (directory.Contains("~/"))
            {
                Debug.LogError("LDtk: Chosen directory contains a '~' at the end of a folder name, which is considered a hidden folder to Unity. Consider renaming the folder.");
                return "";
            }

            if (directory.Contains("/."))
            {
                Debug.LogError("LDtk: Chosen directory contains a '.' at the start of a folder name, which is considered a hidden folder to Unity. Consider renaming the folder.");
                return "";
            }

            if (!directory.Contains(Application.dataPath))
            {
                Debug.LogError("LDtk: Chosen directory is outside the Unity project.");
                return "";
            }
            
            return directory;
        }


        
    }
}