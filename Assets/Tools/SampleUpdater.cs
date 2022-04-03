using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDtkUnity.Editor
{
    internal static class SampleUpdater
    {
        [MenuItem("LDtkUnity/Update Samples")]
        private static void UpdateSamples()
        {
            string srcPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            srcPath = Path.Combine(srcPath, "Programs/ldtk/extraFiles/samples");
            Assert.IsTrue(Directory.Exists(srcPath), srcPath);
            Debug.Log(srcPath);

            string destPath = Application.dataPath;
            destPath = Path.Combine(destPath, "Samples/Samples");
            Assert.IsTrue(Directory.Exists(destPath), destPath);
            Debug.Log(destPath);
            
            if (!Directory.Exists(srcPath) || !Directory.Exists(destPath))
            {
                Debug.LogError("Didn't copy/overwrite files");
                return;
            }
            
            CopyFilesRecursively(srcPath, destPath);
            AssetDatabase.Refresh();
        }
        
        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                string ext = Path.GetExtension(newPath);
                if (ext.EndsWith("md"))
                {
                    //ignore the readme file
                    continue;
                }
                
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}
