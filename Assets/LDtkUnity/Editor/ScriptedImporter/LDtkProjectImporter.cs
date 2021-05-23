using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using Object = UnityEngine.Object;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

#pragma warning disable 0414
#pragma warning disable 0649

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.IMPORTER_LDTK_PROJECT)]
    [ScriptedImporter(LDtkImporterConsts.PROJECT_VERSION, LDtkImporterConsts.PROJECT_EXT, LDtkImporterConsts.PROJECT_ORDER)]
    public class LDtkProjectImporter : LDtkJsonImporter<LDtkProjectFile>
    {
        public const string JSON = nameof(_jsonFile);

        public const string PIXELS_PER_UNIT = nameof(_pixelsPerUnit);
        public const string LEVEL_FIELDS_PREFAB = nameof(_customLevelPrefab);
        public const string ATLAS = nameof(_atlas);
        
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string LOG_BUILD_TIMES = nameof(_logBuildTimes);
        public const string DEPARENT_IN_RUNTIME = nameof(_deparentInRuntime);

        public const string INTGRID = nameof(_intGridValues);
        public const string ENTITIES = nameof(_entities);
        public const string ENUM_GENERATE = nameof(_enumGenerate);
        public const string ENUM_NAMESPACE = nameof(_enumNamespace);
        public const string ENUM_PATH = nameof(_enumPath);
        
        
        [SerializeField] private LDtkProjectFile _jsonFile;
        
        [SerializeField] private int _pixelsPerUnit = 16;
        [SerializeField] private GameObject _customLevelPrefab = null;
        [SerializeField] private SpriteAtlas _atlas;
        
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private bool _deparentInRuntime = false;
        [SerializeField] private bool _logBuildTimes = false;
        
        [SerializeField] private LDtkAssetIntGridValue[] _intGridValues = new LDtkAssetIntGridValue[0];
        [SerializeField] private LDtkAssetEntity[] _entities = new LDtkAssetEntity[0];
        [SerializeField] private bool _enumGenerate = false;
        [SerializeField] private string _enumPath = null;
        [SerializeField] private string _enumNamespace = string.Empty;

        public AssetImportContext ImportContext { get; private set; }

        public LDtkProjectFile JsonFile => _jsonFile;
        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public int PixelsPerUnit => _pixelsPerUnit;
        public bool DeparentInRuntime => _deparentInRuntime;
        public GameObject CustomLevelPrefab => _customLevelPrefab;
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
            
            //trigger a reimport if any of these involved assets are saved or otherwise changed in source control
            SetupAssetDependencies(_intGridValues.Distinct().Cast<ILDtkAsset>().ToArray());
            SetupAssetDependencies(_entities.Distinct().Cast<ILDtkAsset>().ToArray());

            if (_customLevelPrefab != null)
            {
                SetupAssetDependency(_customLevelPrefab);
            }


            TryGenerateEnums(json);

            HideAssets();

            //allow the sprites to be gettable in the assetdatabase properly; only after the import process
            EditorApplication.delayCall += TrySetupSpriteAtlas;
            
        }

        private void SetupAssetDependency(Object asset)
        {
            if (asset == null)
            {
                Debug.LogError("asset null");
                return;
            }
            string customLevelPrefabPath = AssetDatabase.GetAssetPath(asset);
            ImportContext.DependsOnSourceAsset(customLevelPrefabPath);
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
                ImportContext.DependsOnSourceAsset(path);
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
                ImportContext.LogImportError("LDtk: Tried getting an asset from the build data but the array was null. Is the project asset properly saved?");
                return default;
            }

            foreach (LDtkAsset<T> asset in input)
            {
                if (ReferenceEquals(asset, null))
                {
                    ImportContext.LogImportError($"LDtk: A field in the build data is null.");
                    continue;
                }

                if (asset.Key != key)
                {
                    continue;
                }

                if (asset.Asset == null)
                {
                    continue;
                }
                
                return (T)asset.Asset;
            }
            
            return default;
        }

        public TileBase GetTile(Texture2D srcTex, Vector2Int srcPos, int pixelsPerUnit)
        {
            LDtkTileArtifactFactory creator = new LDtkTileArtifactFactory(this, _artifacts, srcTex, srcPos, pixelsPerUnit);
            TileBase tile = creator.TryGetOrCreateTile();
            if (tile == null)
            {
                ImportContext.LogImportError("Null tile, problem?");
            }

            return tile;
        }
    }
}
