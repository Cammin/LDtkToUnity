using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public const string PROP_DEPENDENCIES = nameof(_dependencies);
        [SerializeField] private string[] _dependencies = Array.Empty<string>();
        
        protected LDtkBuilderDependencies Dependencies;
        
        public string AssetName => Path.GetFileNameWithoutExtension(assetPath);

        protected abstract string[] GetGatheredDependencies();
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            ImportContext = ctx;
            Dependencies = new LDtkBuilderDependencies(ctx);

            MainImport();

            //serialize dependencies to display them in the inspector for easier dependency tracking.
            //it conveniently converges both sources of dependencies.
            Profiler.BeginSample("AssignSerializedDependencyStrings");
            _dependencies = Dependencies.GetDependencies().Concat(GetGatheredDependencies()).ToArray(); 
            Profiler.EndSample();
        }

        private void MainImport()
        {
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
            return assetPath.Contains("/backups/backup_");
        }

        protected void CacheDefs(LdtkJson json, Level separateLevel = null)
        {
            LDtkUidBank.CacheUidData(json);
            LDtkIidBank.CacheIidData(json, separateLevel);
        }

        protected void ReleaseDefs()
        {
            LDtkUidBank.ReleaseDefinitions();
            LDtkIidBank.Release();
        }
    }
}