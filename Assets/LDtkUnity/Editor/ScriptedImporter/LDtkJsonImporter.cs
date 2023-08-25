using System;
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

        public LDtkDebugInstance Logger;

        public bool ReimportOnDependencyChange => _reimportOnDependencyChange;
        public AssetImportContext ImportContext { get; private set; }
        public string AssetName => Path.GetFileNameWithoutExtension(assetPath);

        protected abstract string[] GetGatheredDependencies();
        
        public sealed override void OnImportAsset(AssetImportContext ctx)
        {
            ImportContext = ctx;
            Logger = new LDtkDebugInstance(ctx);
            
            if (LDtkPrefs.VerboseLogging)
            {
                LDtkDebug.Log($"OnImportAsset {GetType().Name} {assetPath}");
            }

            try
            {
                MainImport();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
            finally
            {
#if !UNITY_2022_2_OR_NEWER
                Logger._entries.WriteTheEntries();
#endif
            }

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
        
        /// <summary>
        /// both ldtk and ldtkl files can be backups. the level files are in a subdirectory from a backup folder
        /// ldtkt files, on the other hand, should be okay to not check
        /// </summary>
        public bool IsBackupFile()
        {
            if (this is LDtkProjectImporter)
            {
                return LDtkPathUtility.IsFileBackupFile(assetPath, assetPath);
            }
            
            string projectPath = new LDtkRelativeGetterProjectImporter().GetPath(assetPath, assetPath);
            return LDtkPathUtility.IsFileBackupFile(assetPath, projectPath);
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
            file.name = Path.GetFileNameWithoutExtension(assetPath) + "_Json";
            file.SetJson(jsonText);
            return file;
        }
        
        public TJson FromJson<TJson>()
        {
            return FromJson<TJson>(assetPath, Logger);
        }
        
        public static TJson FromJson<TJson>(string path, LDtkDebugInstance debug = null)
        {
            if (!File.Exists(path))
            {
                LDtkDebug.LogError($"Could not find the json file to deserialize at \"{path}\"", debug);
                return default;
            }
            
            Profiler.BeginSample($"FromJson {typeof(TJson).Name}");
            
            Profiler.BeginSample($"ReadAllBytes");
            byte[] bytes = File.ReadAllBytes(path);
            Profiler.EndSample();
            
            Profiler.BeginSample($"FromJson");
            TJson json = default;
            try
            {
                json = Utf8Json.JsonSerializer.Deserialize<TJson>(bytes);
            }
            catch (Exception e)
            {
                LDtkDebug.LogError($"Failure to deserialize json: {e}", debug);
            }
            Profiler.EndSample();
        
            Profiler.EndSample(); //this end sample for the caller up the stack
            return json;
        }
        
        public LDtkProjectImporter GetProjectImporter()
        {
            LDtkRelativeGetterProjectImporter getter = new LDtkRelativeGetterProjectImporter();
            LDtkProjectImporter projectImporter = getter.GetRelativeAsset(assetPath, assetPath, (path) => (LDtkProjectImporter)GetAtPath(path));
            return projectImporter;
        }
        public string GetProjectPath()
        {
            LDtkRelativeGetterProjectImporter getter = new LDtkRelativeGetterProjectImporter();
            return getter.GetPath(assetPath, assetPath);
        }
    }
}