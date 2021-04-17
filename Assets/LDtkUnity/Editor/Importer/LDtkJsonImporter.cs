using System.IO;
using UnityEngine;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    public abstract class LDtkJsonImporter<T> : ScriptedImporter
    {
        protected T LoadJson(AssetImportContext ctx)
        {
            string path = ctx.assetPath;
            string json = File.ReadAllText(path);
            return LoadData(json);
        }

        protected abstract T LoadData(string json);
    }
}