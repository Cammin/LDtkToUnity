using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.U2D;
using Object = UnityEngine.Object;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

#pragma warning disable 0414

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.IMPORTER_LDTK_PROJECT)]
    [ScriptedImporter(3, EXTENSION)]
    public class LDtkProjectImporter : LDtkJsonImporter<LDtkProjectFile>
    {
        private const string EXTENSION = "ldtk";
        
        public const string JSON = nameof(_jsonFile);

        public const string PIXELS_PER_UNIT = nameof(_pixelsPerUnit);
        public const string LEVEL_FIELDS_PREFAB = nameof(_levelFieldsPrefab);
        public const string ATLAS = nameof(_atlas);
        
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string LOG_BUILD_TIMES = nameof(_logBuildTimes);
        public const string DEPARENT_IN_RUNTIME = nameof(_deparentInRuntime);

        public const string LEVELS_TO_BUILD = nameof(_levelsToBuild);
        public const string INTGRID = nameof(_intGridValues);
        public const string ENTITIES = nameof(_entities);
        public const string ENUM_NAMESPACE = nameof(_enumNamespace);
        public const string ENUM_ASSEMBLY = nameof(_enumAssembly);
        public const string TILEMAP_PREFABS = nameof(_gridPrefabs);
        
        
        [SerializeField] private LDtkProjectFile _jsonFile;
        
        [SerializeField] private int _pixelsPerUnit = 16;
        [SerializeField] private GameObject _levelFieldsPrefab = null;
        [SerializeField] private SpriteAtlas _atlas;
        
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private bool _deparentInRuntime = false;
        [SerializeField] private bool _logBuildTimes = false;
        
        [SerializeField] private bool[] _levelsToBuild = {true};
        [SerializeField] private LDtkAsset[] _intGridValues = null;
        [SerializeField] private LDtkAsset[] _entities = null;
        [SerializeField] private string _enumNamespace = string.Empty;
        [SerializeField] private Object _enumAssembly = null;
        [SerializeField] private LDtkAsset[] _gridPrefabs = null;

        public LDtkArtifactAssets AutomaticallyGeneratedArtifacts;
        

        public LDtkProjectFile JsonFile => _jsonFile;
        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public int PixelsPerUnit => _pixelsPerUnit;
        public bool DeparentInRuntime => _deparentInRuntime;
        public GameObject LevelFieldsPrefab => _levelFieldsPrefab;
        public bool LogBuildTimes => _logBuildTimes;

        
        public string AssetName => Path.GetFileNameWithoutExtension(assetPath);
        
        public AssetImportContext ImportContext { get; private set; }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            ImportContext = ctx;
            
            _jsonFile = ReadAssetText(ctx);
            _jsonFile.name += "_Json";
            
            //the tile bank for storing the creation process. also gets added to the context
            AutomaticallyGeneratedArtifacts = ScriptableObject.CreateInstance<LDtkArtifactAssets>();
            AutomaticallyGeneratedArtifacts.name = AssetName + "_Assets";
                
            
            LDtkProjectImporterFactory factory = new LDtkProjectImporterFactory(this, ctx);
            factory.Import();
            
            SetupAssetDependencies(ctx, _intGridValues);
            SetupAssetDependencies(ctx, _entities);
            SetupAssetDependencies(ctx, _gridPrefabs);
        }

        private void SetupAssetDependencies(AssetImportContext ctx, LDtkAsset[] assets)
        {
            if (assets.IsNullOrEmpty())
            {
                return;
            }
            
            foreach (LDtkAsset asset in assets)
            {
                if (asset.Asset == null)
                {
                    continue;
                }

                string path = AssetDatabase.GetAssetPath(asset.Asset);
                GUID guid = AssetDatabase.GUIDFromAssetPath(path);
                ctx.DependsOnArtifact(guid);
            }
        }
        
        
        public Sprite GetIntGridValueSprite(string key)
        {
            return GetAssetByIdentifier<Sprite>(_intGridValues, key, true);
        }
        public GameObject GetEntity(string key)
        {
            return GetAssetByIdentifier<GameObject>(_entities, key);
        }
        public GameObject GetTilemapPrefab(string identifier)
        {
            //prefer to get the custom prefab from the specified player first.
            GameObject customLayerGridPrefab = GetAssetByIdentifier<GameObject>(_gridPrefabs, identifier, true);

            //if override exists, use it. Otherwise use a default. Similar to how unity resolves empty fields like Physics Materials for example.
            return customLayerGridPrefab != null ? customLayerGridPrefab : LDtkResourcesLoader.LoadDefaultGridPrefab();
        }
        
        private T GetAssetByIdentifier<T>(IEnumerable<LDtkAsset> input, string key, bool ignoreNullProblem = false) where T : Object
        {
            
            
            if (input == null)
            {
                Debug.LogError("LDtk: Tried getting an asset from the build data but the array was null. Is the project asset properly saved?");
                return OnFail();
            }
            
            if (LDtkProviderErrorIdentifiers.Contains(key))
            {
                //this is to help prevent too much log spam. only one mistake from the same identifier get is necessary.
                return default;
            }
            
            foreach (LDtkAsset asset in input)
            {
                if (ReferenceEquals(asset, null))
                {
                    Debug.LogError($"LDtk: A field in the build data is null.");
                    continue;
                }

                if (asset.Key != key)
                {
                    continue;
                }

                if (asset.Asset != null)
                {
                    return (T)asset.Asset;
                }

                if (!ignoreNullProblem)
                {
                    Debug.LogError($"LDtk: The asset \"{asset.Key}\" was required to build, but wasn't assigned.", asset.Asset);
                }
                
                return OnFail();
            }

            Debug.LogError($"LDtk: Could not find any asset with identifier \"{key}\" in the build data. Unassigned in project assets?");
            return OnFail();
            
            T OnFail()
            {
                LDtkProviderErrorIdentifiers.Add(key);
                return default;
            }
        }
    }
}
