using System.IO;
using UnityEngine;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    public abstract class LDtkJsonImporter<T> : ScriptedImporter where T : ScriptableObject, ILDtkJsonFile
    {
        protected abstract string Extension { get; }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            string path = ctx.assetPath;
            string json = File.ReadAllText(path);
            
            T asset = ScriptableObject.CreateInstance<T>();
            asset.SetJson(json);
            
            ctx.AddObjectToAsset(Extension, asset);
            ctx.SetMainObject(asset);
        }
    }
}