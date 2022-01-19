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

        public override void OnImportAsset(AssetImportContext ctx)
        {
            ImportContext = ctx;
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
        
        /// <summary>
        /// Reimport if any assets change: IntGrid values, entity/level prefabs, levelFiles, and tileset textures
        /// </summary>
        public void SetupAssetDependency(Object asset)
        {
            if (asset == null)
            {
                Debug.LogError("LDtk: Asset null while adding dependency");
                return;
            }
            string dependencyPath = AssetDatabase.GetAssetPath(asset);
            ImportContext.DependsOnSourceAsset(dependencyPath);
        }
    }
}