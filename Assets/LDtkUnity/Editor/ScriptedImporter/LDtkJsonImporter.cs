using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    /// <summary>
    /// The LDtk json files import on a different basis, where they truly import on the EditorApplication.delayCall, and the first import is only a single imported GameObject.
    /// The reason for this is on this post: https://forum.unity.com/threads/create-additional-prefab-assets-in-scriptedimporter-and-track-them.1158734/#post-7792380
    /// </summary>
    [ExcludeFromDocs]
    public abstract class LDtkJsonImporter<T> : ScriptedImporter where T : ScriptableObject, ILDtkJsonFile
    {
        private static readonly Dictionary<string, bool> ImportRepeatCheck = new Dictionary<string, bool>();
        //[SerializeField] private T
            

//we do want to remember the json on the 2nd import so that we don't have to deserialize twice
        private class ImportInfo
        {
            public T json;
            public bool isSecondImport = false;
        }
        
        public AssetImportContext ImportContext { get; private set; }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            ImportContext = ctx;
            ImportDesision();
        }
        
        private void ImportDesision()
        {
            //this is in fact better to cache the json
            if (!ImportRepeatCheck.ContainsKey(assetPath))
            {
                ImportRepeatCheck.Add(assetPath, false);
            }
            
            //check if imported twice already, and in that case, then reset to prepare for the next one
            if (!ImportRepeatCheck[assetPath])
            {
                Debug.Log("first import");
                Import();
                
                EditorApplication.delayCall += () =>
                {
                    ImportRepeatCheck[assetPath] = true;
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);
                };
                return;
            }
            
            Debug.Log("second import");
            ImportRepeatCheck[assetPath] = false;
            Import();
            OnSecondImport();
        }
        protected abstract void Import();
        protected virtual void OnSecondImport()
        {
        }

        protected T ReadAssetText()
        {
            string json = File.ReadAllText(assetPath);

            T file = ScriptableObject.CreateInstance<T>();
            file.name = Path.GetFileNameWithoutExtension(assetPath);
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