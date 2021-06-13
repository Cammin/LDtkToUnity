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