using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    internal abstract class LDtkJsonImporter<T> : ScriptedImporter where T : ScriptableObject, ILDtkJsonFile
    {
        public AssetImportContext ImportContext { get; private set; }
        protected LDtkBuilderDependencies Dependencies;
        
        public string AssetName => Path.GetFileNameWithoutExtension(assetPath);

        public override void OnImportAsset(AssetImportContext ctx)
        {
            ImportContext = ctx;
            Dependencies = new LDtkBuilderDependencies(ctx);

            if (LDtkPrefs.WriteProfiledImports)
            {
                ProfileImport();
                return;
            }
            Import();
        }

        private void ProfileImport()
        {
            string path = Path.GetFileName(assetPath);
            using (new LDtkProfiler.Scope(path))
            {
                Import();
            }
        }

        protected abstract void Import();

        protected T ReadAssetText()
        {
            string jsonText = File.ReadAllText(assetPath);
            
            T file = ScriptableObject.CreateInstance<T>();
            file.name = Path.GetFileNameWithoutExtension(assetPath);
            file.SetJson(jsonText);
            return file;
        }
        
        public bool IsBackupFile() //both ldtk and ldtkl files can be backups. the level files are in a subdirectory from a backup folder
        {
            return assetPath.Contains("/backups/backup_", StringComparison.InvariantCulture);
        }
    }
}