using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
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

        public override void OnImportAsset(AssetImportContext ctx)
        {
            ImportContext = ctx;
            Dependencies = new LDtkBuilderDependencies(ctx);
            Import();
        }

        protected abstract void Import();

        protected T ReadAssetText()
        {
            string json = File.ReadAllText(assetPath);

            T file = ScriptableObject.CreateInstance<T>();

            file.name = Path.GetFileNameWithoutExtension(assetPath);

            //Debug.Log($"Reimporting {file.name}");
            
            file.SetJson(json);

            return file;
        }

        protected LdtkJson ReadJson()
        {
            string json = File.ReadAllText(assetPath);
            return LdtkJson.FromJson(json);
        }
    }
}