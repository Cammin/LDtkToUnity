using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// this is a hack to fix a field definition/instance ordering disparity
        /// https://github.com/deepnight/ldtk/issues/609
        /// todo promptly remove this once the issue is resolved
        /// </summary>
        protected void ReorderFieldInstances(LdtkJson json)
        {
            LDtkUidBank.CacheUidData(json);
            foreach (Level level in json.Levels)
            {
                level.FieldInstances = GetReordered(level.FieldInstances, json.Defs.LevelFields);

                foreach (EntityInstance entity in level.LayerInstances.Where(p => p.IsEntitiesLayer).SelectMany(p => p.EntityInstances))
                {
                    entity.FieldInstances = GetReordered(entity.FieldInstances, entity.Definition.FieldDefs);
                }
            }
            LDtkUidBank.ReleaseDefinitions();
        }

        private FieldInstance[] GetReordered(FieldInstance[] formerInstances, FieldDefinition[] defs)
        {
            Dictionary<string, FieldInstance> instances = new Dictionary<string, FieldInstance>();
            foreach (FieldInstance fieldInst in formerInstances)
            {
                instances.Add(fieldInst.Identifier, fieldInst);
            }
            
            FieldInstance[] newInstances = new FieldInstance[instances.Values.Count];

            for (int i = 0; i < defs.Length; i++)
            {
                FieldDefinition fieldDef = defs[i];
                if (!instances.ContainsKey(fieldDef.Identifier))
                {
                    Debug.LogError("LDtk: Could not reorder field instances to match definition order");
                    return formerInstances;
                }

                FieldInstance instance = instances[fieldDef.Identifier];
                newInstances[i] = instance;
            }

            return newInstances;
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