
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkPathUtil
    {
        

        public static string CleanPathSlashes(string directory)
        {
            return directory.Replace('\\', '/');
        }
        public static string SimplifyPathWithDoubleDots(string inputPath)
        {
            string fullPath = Path.GetFullPath(inputPath);
            return "Assets" + fullPath.Substring(Application.dataPath.Length);
        }

        public static void CreateDirectoryIfNotValidFolder(string directory)
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
            string objAssetDirectory = Path.GetDirectoryName(objAssetPath);
            
            string directory = $"{objAssetDirectory}/{obj.name}";

            return CleanPathSlashes(directory);
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


        public static T GetAssetRelativeToAsset<T>(Object asset, string relPath) where T : Object
        {
            if (!AssetDatabase.Contains(asset))
            {
                Debug.LogError("LDtk: input object is not in the AssetDatabase");
                return null;
            }
            
            string assetPath = AssetDatabase.GetAssetPath(asset);
            return GetAssetRelativeToAssetPath<T>(assetPath, relPath);
        }
        public static T GetAssetRelativeToAssetPath<T>(string assetPath, string relPath) where T : Object
        {
            string directory = Path.GetDirectoryName(assetPath);
            
            string assetsPath = $"{directory}/{relPath}";
            
            //simplify double dots
            assetsPath = SimplifyPathWithDoubleDots(assetsPath);
            
            //replace backslash with forwards
            assetsPath = CleanPathSlashes(assetsPath);

            Debug.Log($"Trying to load at {assetsPath}");
            
            T assetAtPath = AssetDatabase.LoadAssetAtPath<T>(assetsPath);
            if (assetAtPath != null)
            {
                return assetAtPath;
            }
            
            //try a reimport as it may fix it
            if (File.Exists(assetsPath))
            {
                AssetDatabase.ImportAsset(assetsPath);
                assetAtPath = AssetDatabase.LoadAssetAtPath<T>(assetsPath);
                if (assetAtPath != null)
                {
                    return assetAtPath;
                }
            }
            
            //if we couldn't load it but the file indeed exists, spit a different error
            if (File.Exists(assetsPath))
            {
                AssetDatabase.ImportAsset(assetsPath);
                    
                Debug.LogError($"LDtk: File exists but could not load an asset at \"{assetPath}\". " +
                               $"Is the asset imported correctly?");
            }
            else
            {
                Debug.LogError($"LDtk: Could not find an asset in the path relative to \"{assetPath}\": \"{relPath}\". " +
                               $"Is the asset also locatable by LDtk, and is the asset located in the Unity Project?");
            }

            return assetAtPath;
        }
    }
}