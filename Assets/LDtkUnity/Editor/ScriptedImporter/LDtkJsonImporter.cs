using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    internal abstract class LDtkJsonImporter : ScriptedImporter
    {
        public const string REIMPORT_ON_DEPENDENCY_CHANGE = nameof(_reimportOnDependencyChange);
        [SerializeField] private bool _reimportOnDependencyChange = true;

        public bool ReimportOnDependencyChange => _reimportOnDependencyChange;
        public AssetImportContext ImportContext { get; private set; }
        public string AssetName => Path.GetFileNameWithoutExtension(assetPath);

        protected abstract string[] GetGatheredDependencies();
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            ImportContext = ctx;

            MainImport();

            //serialize dependencies to display them in the inspector for easier dependency tracking.
            Profiler.BeginSample("SerializeStringDependencies");
            LDtkDependencyCache.Set(assetPath, GetGatheredDependencies()); 
            Profiler.EndSample();
        }

        private void MainImport()
        {
            string path = Path.GetFileName(assetPath);
            using (new LDtkProfiler.Scope(path))
            {
                Import();
            }
        }

        protected abstract void Import();
        
        public bool IsBackupFile() //both ldtk and ldtkl files can be backups. the level files are in a subdirectory from a backup folder
        {
            return LDtkPathUtility.IsFileBackupFile(assetPath);
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
    
    internal abstract class LDtkJsonImporter<T> : LDtkJsonImporter where T : ScriptableObject, ILDtkJsonFile
    {
        protected T ReadAssetText()
        {
            string jsonText = File.ReadAllText(assetPath);
            
            T file = ScriptableObject.CreateInstance<T>();
            file.name = Path.GetFileNameWithoutExtension(assetPath);
            file.SetJson(jsonText);
            return file;
        }
        
        public TJson FromJson<TJson>()
        {
            string path = assetPath;
            if (!File.Exists(path))
            {
                LDtkDebug.LogError($"Could not find the json file to deserialize at \"{path}\"");
                return default;
            }
            
            Profiler.BeginSample($"FromJson {typeof(TJson).Name}");
            
            Profiler.BeginSample($"ReadAllBytes");
            byte[] bytes = File.ReadAllBytes(path);
            Profiler.EndSample();
            
            Profiler.BeginSample($"FromJson");
            TJson json = Utf8Json.JsonSerializer.Deserialize<TJson>(bytes);
            Profiler.EndSample();
        
            Profiler.EndSample(); //this end sample for the caller up the stack
            return json;
        }
    }
}