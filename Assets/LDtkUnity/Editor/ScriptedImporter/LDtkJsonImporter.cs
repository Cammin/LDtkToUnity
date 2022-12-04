using System.IO;
using Newtonsoft.Json;
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
        
        public TJson FromJsonStream<TJson>()
        {
            string path = assetPath;
            if (!File.Exists(path))
            {
                LDtkDebug.LogError($"Could not find the json file to deserialize at \"{path}\"");
                return default;
            }
            
            Profiler.BeginSample($"ReadAllText");
            
            Profiler.BeginSample($"ReadAllText {typeof(TJson).Name}");
            string jsonText = File.ReadAllText(assetPath);
            Profiler.EndSample();
            
            Profiler.BeginSample($"Deserialize ReadAllText {typeof(TJson).Name}");
            Utf8Json.JsonSerializer.Deserialize<TJson>(jsonText);
            Profiler.EndSample();
            
            Profiler.EndSample();
            Profiler.EndSample();
            
            Profiler.BeginSample($"Stream");
            
            Profiler.BeginSample($"FromJsonStream {typeof(TJson).Name}");
            using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            Profiler.EndSample();
            
            Profiler.BeginSample($"FromJsonStream {typeof(TJson).Name}");
            TJson json = Utf8Json.JsonSerializer.Deserialize<TJson>(stream);
            Profiler.EndSample();
            
            Profiler.EndSample();
            return json;

            /*using (StreamReader streamReader = new StreamReader(stream))
            {
            }
                
                
                
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                TJson json = serializer.Deserialize<TJson>(jsonReader);
                Profiler.EndSample();
                return json;
            }*/
        }
    }
}