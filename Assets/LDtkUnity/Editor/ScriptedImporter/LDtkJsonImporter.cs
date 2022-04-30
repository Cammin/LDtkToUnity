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
            
            Import();
            //ProfileImport(); //use when testing
        }

        private void ProfileImport()
        {
            string s = $"Profiler/{Path.GetFileName(assetPath)}";
            Directory.CreateDirectory("Profiler");
            Profiler.logFile = s;
            Profiler.enableBinaryLog = true;
            Profiler.enabled = true;
            Profiler.BeginSample($"Import {AssetName}");
            
            Import();
            
            Profiler.EndSample();
            Profiler.enabled = false;
            Profiler.logFile = "";
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
    }
}