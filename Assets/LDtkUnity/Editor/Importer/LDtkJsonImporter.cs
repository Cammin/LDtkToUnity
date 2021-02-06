using System.IO;
using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

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

            Debug.Log("Detected change in file " + asset.name);
            //AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}