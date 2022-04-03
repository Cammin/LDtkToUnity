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
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            appDataFolder = Path.Combine(appDataFolder, "Programs/ldtk/extraFiles/samples");
            Assert.IsTrue(Directory.Exists(appDataFolder), appDataFolder);
            Debug.Log(appDataFolder);

            string destPath = Application.dataPath;
            destPath = Path.Combine(destPath, "Samples/Samples");
            Assert.IsTrue(Directory.Exists(destPath), destPath);
            Debug.Log(destPath);
            
            //CopyFilesRecursively(appDataFolder, destPath);
        }
        
        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}
