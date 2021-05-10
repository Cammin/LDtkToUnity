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
        protected T ReadAssetText(AssetImportContext ctx)
        {
            string path = ctx.assetPath;
            string json = File.ReadAllText(path);

            T file = ScriptableObject.CreateInstance<T>();

            file.name = Path.GetFileNameWithoutExtension(ctx.assetPath);

            //Debug.Log($"Reimporting {file.name}");
            
            file.SetJson(json);

            return file;
        }
    }
}