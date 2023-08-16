using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDtkUnity.Editor
{
    internal static class SampleUpdater
    {
        [MenuItem("LDtkUnity/Update Samples", false, 10)]
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
                //ignore the thumbnails
                if (dirPath.Contains("thumbs"))
                {
                    //ignore the thumbnails
                    continue;
                }
                
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                string ext = Path.GetFileName(newPath);
                if (ext == "README.md")
                {
                    //ignore the readme file
                    continue;
                }
                
                //ignore the thumbnails
                if (newPath.Contains("thumbs"))
                {
                    //ignore the thumbnails
                    continue;
                }

                string dest = newPath.Replace(sourcePath, targetPath);
                File.Copy(newPath, dest, true);
                LDtkEditorCommandUpdater.ModifyProjectWithCommand(dest, @"../../../Library/LDtkTilesetExporter/ExportTilesetDefinition.exe");
            }
        }
    }
}
