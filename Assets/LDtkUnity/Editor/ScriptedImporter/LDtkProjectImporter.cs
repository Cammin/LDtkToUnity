using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
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
    [ScriptedImporter(LDtkImporterConsts.PROJECT_VERSION, LDtkImporterConsts.PROJECT_EXT, LDtkImporterConsts.PROJECT_ORDER)]
    public class LDtkProjectImporter : LDtkJsonImporter<LDtkProjectFile>
    {
        public const string JSON = nameof(_jsonFile);

        public const string PIXELS_PER_UNIT = nameof(_pixelsPerUnit);
        public const string ATLAS = nameof(_atlas);
        
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string LOG_BUILD_TIMES = nameof(_logBuildTimes);
        public const string DEPARENT_IN_RUNTIME = nameof(_deparentInRuntime);

        //public const string LEVELS_TO_BUILD = nameof(_levelsToBuild);
        public const string INTGRID = nameof(_intGridValues);
        public const string ENTITIES = nameof(_entities);
        public const string ENUM_GENERATE = nameof(_enumGenerate);
        public const string ENUM_NAMESPACE = nameof(_enumNamespace);
        public const string ENUM_PATH = nameof(_enumPath);
        //public const string TILEMAP_PREFABS = nameof(_gridPrefabs);
        
        
        [SerializeField] private LDtkProjectFile _jsonFile;
        
        [SerializeField] private int _pixelsPerUnit = 16;
        //[SerializeField] private GameObject _levelFieldsPrefab = null;
        [SerializeField] private SpriteAtlas _atlas;
        
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private bool _deparentInRuntime = false;
        [SerializeField] private bool _logBuildTimes = false;
        
        //[SerializeField] private bool[] _levelsToBuild = {true};
        [SerializeField] private LDtkAsset<LDtkIntGridTile>[] _intGridValues = null;
        [SerializeField] private LDtkAsset<GameObject>[] _entities = null;
        [SerializeField] private bool _enumGenerate = false;
        [SerializeField] private string _enumPath = null;
        [SerializeField] private string _enumNamespace = string.Empty;
        //[SerializeField] private LDtkAsset[] _gridPrefabs = null;

        
        public AssetImportContext ImportContext { get; private set; }
        

        public LDtkProjectFile JsonFile => _jsonFile;
        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public int PixelsPerUnit => _pixelsPerUnit;
        public bool DeparentInRuntime => _deparentInRuntime;
        //public GameObject LevelFieldsPrefab => _levelFieldsPrefab;
        public bool LogBuildTimes => _logBuildTimes;
        public string AssetName => Path.GetFileNameWithoutExtension(assetPath);
        
        private LDtkArtifactAssets _artifacts;
        
        

        public override void OnImportAsset(AssetImportContext ctx)
        {
            ImportContext = ctx;
            Import();
        }

        private void Import()
        {
            CreateJsonAsset();

            if (!TryGetJson(out LdtkJson json))
            {
                return;
            }
            
            CreateArtifactAsset();

            MainBuild(json);
                        

            SetupAssetDependencies(_intGridValues.Distinct().Cast<ILDtkAsset>().ToArray());
            SetupAssetDependencies(_entities.Distinct().Cast<ILDtkAsset>().ToArray());
            //SetupAssetDependencies(_gridPrefabs);
            
            TryGenerateEnums(json);

            HideAssets();

            //TODO consider adding enum components fields to entities this way? Potentially make the project and levels only after loading the assets? (they may not be modifiable after the import process)
            //allow the sprites to be gettable in the assetdatabase properly; after the import process
            EditorApplication.delayCall += TrySetupSpriteAtlas;
            
        }

        private void HideAssets()
        {
            //need to keep the sprites visible in the project view if using sprite atlas
            if (_atlas == null)
            {
                _artifacts.HideSprites();
            }

            _artifacts.HideTiles();
            
            _artifacts.HideBackgrounds();
        }

        private bool TryGetJson(out LdtkJson json)
        {
            json = _jsonFile.FromJson;
            if (json != null)
            {
                return true;
            }
            
            ImportContext.LogImportError("LDtk: Json import error");
            return false;

        }

        private void CreateJsonAsset()
        {
            _jsonFile = ReadAssetText(ImportContext);
            _jsonFile.name += "_Json";
            ImportContext.AddObjectToAsset("jsonFile", JsonFile, (Texture2D) EditorGUIUtility.IconContent("ScriptableObject Icon").image);
        }

        private void MainBuild(LdtkJson json)
        {
            LDtkProjectImporterFactory factory = new LDtkProjectImporterFactory(this);
            factory.Import(json);
        }

        private void CreateArtifactAsset()
        {
            //the bank for storing the auto-generated items.
            _artifacts = ScriptableObject.CreateInstance<LDtkArtifactAssets>();
            _artifacts.name = AssetName + "_Assets";
            
            ImportContext.AddObjectToAsset("artifacts", _artifacts, (Texture2D)LDtkIconUtility.GetUnityIcon("Tilemap"));
        }

        private void TryGenerateEnums(LdtkJson json)
        {
            //generate enums
            if (!_enumGenerate || json.Defs.Enums.IsNullOrEmpty())
            {
                return;
            }
            
            LDtkProjectImporterEnumGenerator enumGenerator = new LDtkProjectImporterEnumGenerator(json.Defs.Enums, ImportContext, _enumPath, _enumNamespace);
            enumGenerator.Generate();
        }

        private void TrySetupSpriteAtlas()
        {
            if (_atlas == null)
            {
                return;
            }

            Object[] atPath = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            Sprite[] sprites = atPath.Where(p => p is Sprite).Cast<Sprite>().ToArray();
            
            //remove existing
            _atlas.Remove(_atlas.GetPackables());
            
            //add sorted sprites
            Object[] inputSprites = sprites.Distinct().OrderBy(p => p.name).Cast<Object>().ToArray();
            _atlas.Add(inputSprites);

        }

        public void AddArtifact(Object obj)
        {
            if (_artifacts.AddArtifact(obj))
            {
                ImportContext.AddObjectToAsset(obj.name, obj);
            }
        }

        public void AddBackgroundArtifact(Sprite obj)
        {
            AddArtifact(obj);
            _artifacts.AddBackground(obj);
        }

        private void SetupAssetDependencies(ILDtkAsset[] assets)
        {
            //dependencies. reimport if any of these assets change
            if (assets.IsNullOrEmpty())
            {
                return;
            }
            
            foreach (ILDtkAsset asset in assets)
            {
                if (asset.Asset == null)
                {
                    continue;
                }

                string path = AssetDatabase.GetAssetPath(asset.Asset);
                GUID guid = AssetDatabase.GUIDFromAssetPath(path);
                ImportContext.DependsOnSourceAsset(guid);
            }
        }
        
        
        public LDtkIntGridTile GetIntGridValueTile(string key)
        {
            return GetAssetByIdentifier(_intGridValues, key);
        }
        public GameObject GetEntity(string key)
        {
            return GetAssetByIdentifier(_entities, key);
        }

        private T GetAssetByIdentifier<T>(IEnumerable<LDtkAsset<T>> input, string key) where T : Object
        {
            if (input == null)
            {
                Debug.LogError("LDtk: Tried getting an asset from the build data but the array was null. Is the project asset properly saved?");
                return default;
            }

            foreach (LDtkAsset<T> asset in input)
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

                return default;
            }

            Debug.LogError($"LDtk: Could not find any asset with identifier \"{key}\" in the build data. Unassigned in project assets?");
            return default;
        }

        public TileBase GetTile(Texture2D srcTex, Vector2Int srcPos, int pixelsPerUnit)
        {
            LDtkTileArtifactFactory creator = new LDtkTileArtifactFactory(this, _artifacts, srcTex, srcPos, pixelsPerUnit);
            TileBase tile = creator.TryGetOrCreateTile();
            if (tile == null)
            {
                Debug.LogError("Null tile, problem?");
            }

            return tile;
        }
    }
}
