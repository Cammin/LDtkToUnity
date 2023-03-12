using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
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
    internal sealed class LDtkProjectImporter : LDtkJsonImporter<LDtkProjectFile>
    {
        public const string JSON = nameof(_jsonFile);

        public const string PIXELS_PER_UNIT = nameof(_pixelsPerUnit);
        public const string ATLAS = nameof(_atlas);
        public const string CUSTOM_LEVEL_PREFAB = nameof(_customLevelPrefab);
        public const string DEPARENT_IN_RUNTIME = nameof(_deparentInRuntime);
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string USE_COMPOSITE_COLLIDER = nameof(_useCompositeCollider);
        public const string CREATE_BACKGROUND_COLOR = nameof(_createBackgroundColor);
        public const string CREATE_LEVEL_BOUNDS_TRIGGER = nameof(_createLevelBoundsTrigger);
        
        public const string INTGRID = nameof(_intGridValues);
        public const string ENTITIES = nameof(_entities);
        
        public const string ENUM_GENERATE = nameof(_enumGenerate);
        public const string ENUM_PATH = nameof(_enumPath);
        public const string ENUM_NAMESPACE = nameof(_enumNamespace);
        
        
        /// <summary>
        /// This is cached into the meta file upon an import. Could be null if the import was a failure. Invisible to the inspector.
        /// </summary>
        [SerializeField] private LDtkProjectFile _jsonFile;

        [SerializeField] private int _pixelsPerUnit = -1;
        [SerializeField] private SpriteAtlas _atlas;
        [SerializeField] private GameObject _customLevelPrefab = null;
        [SerializeField] private bool _deparentInRuntime = false;
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private bool _useCompositeCollider = true;
        [SerializeField] private bool _createBackgroundColor = true;
        [SerializeField] private bool _createLevelBoundsTrigger = false;
        
        [SerializeField] private LDtkAssetIntGridValue[] _intGridValues = Array.Empty<LDtkAssetIntGridValue>();
        
        [SerializeField] private LDtkAssetEntity[] _entities = Array.Empty<LDtkAssetEntity>();
        
        [SerializeField] private bool _enumGenerate = false;
        [SerializeField] private string _enumPath = null;
        [SerializeField] private string _enumNamespace = string.Empty;

        
        public LDtkProjectFile JsonFile => _jsonFile;
        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public SpriteAtlas Atlas => _atlas;
        public int PixelsPerUnit => _pixelsPerUnit;
        public bool DeparentInRuntime => _deparentInRuntime;
        public GameObject CustomLevelPrefab => _customLevelPrefab;
        public bool UseCompositeCollider => _useCompositeCollider;
        public bool CreateBackgroundColor => _createBackgroundColor;
        public bool CreateLevelBoundsTrigger => _createLevelBoundsTrigger;

        //all of these are wiped after the entire import is done
        private LDtkArtifactAssets _artifacts;
        private LDtkArtifactsFactory _artifactsFactory;
        private bool _hadArtifactLoadProblem;
        private static string[] _previousDependencies;
        
        //this will run upon standard reset, but also upon the meta file generation during the first import
        private void Reset()
        {
            OnResetPPU();
        }

        private static string[] GatherDependenciesFromSourceFile(string path)
        {
            LDtkProfiler.BeginSample($"GatherDependenciesFromSourceFile/{Path.GetFileName(path)}");
            _previousDependencies = LDtkProjectDependencyFactory.GatherProjectDependencies(path);
            LDtkProfiler.EndSample();
            
            return _previousDependencies;
        }

        protected override string[] GetGatheredDependencies() => _previousDependencies;

        protected override void Import()
        {
            if (IsBackupFile())
            {
                BufferEditorCache();
                return;
            }
            
            _hadArtifactLoadProblem = false;
            
            Profiler.BeginSample("CheckOutdatedJsonVersion");
            string version = "";
            LDtkJsonDigger.GetJsonVersion(assetPath, ref version);
            if (!CheckOutdatedJsonVersion(version, AssetName))
            {
                Profiler.EndSample();
                BufferEditorCache();
                return;
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("CreateJsonAsset");
            CreateJsonAsset();
            Profiler.EndSample();
            
            if (!TryGetJson(out LdtkJson json))
            {
                LDtkDebug.LogError("Json deserialization error. Not importing.");
                BufferEditorCache();
                return;
            }
            
            Profiler.BeginSample("CacheDefs");
            CacheDefs(json);
            Profiler.EndSample();

            Profiler.BeginSample("SetPixelsPerUnit");
            SetPixelsPerUnit(json.DefaultGridSize); //if for whatever reason (or backwards compatibility), if the ppu is -1 in any capacity
            Profiler.EndSample();
            
            Profiler.BeginSample("CreateTableOfContents");
            TryCreateTableOfContents(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("CacheRecentImporter");
            LDtkParsedTile.CacheRecentImporter(this);
            Profiler.EndSample();
            
            Profiler.BeginSample("CreateArtifactAsset");
            CreateArtifactsAsset(json);
            Profiler.EndSample();

            Profiler.BeginSample("MainBuild");
            MainBuild(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("TryGenerateEnums");
            TryGenerateEnums(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("HideArtifactAssets");
            HideArtifactAssets();
            Profiler.EndSample();
            
            Profiler.BeginSample("TryPrepareSpritePacking");
            TryPrepareSpritePacking(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("BufferEditorCache");
            BufferEditorCache();
            Profiler.EndSample();

            Profiler.BeginSample("CheckDefaultEditorBehaviour");
            CheckDefaultEditorBehaviour();
            Profiler.EndSample();
            
            Profiler.BeginSample("ReleaseDefs");
            ReleaseDefs();
            Profiler.EndSample();
        }

        private static void CheckDefaultEditorBehaviour()
        {
            if (EditorSettings.defaultBehaviorMode != EditorBehaviorMode.Mode2D)
            {
                LDtkDebug.LogWarning("It is encouraged to use 2D project mode while using LDtkToUnity. Change it in \"Project Settings > Editor > Default Behaviour Mode\"");
            }
        }

        public static bool CheckOutdatedJsonVersion(string jsonVersion, string assetName)
        {
            jsonVersion = Regex.Replace(jsonVersion, "[^0-9.]", "");
            if (!Version.TryParse(jsonVersion, out Version version))
            {
                LDtkDebug.LogError($"This json asset \"{assetName}\" couldn't parse it's version \"{jsonVersion}\", post an issue to the developer");
                return false;
            }

            Version minimumRecommendedVersion = new Version(LDtkImporterConsts.LDTK_JSON_VERSION);
            if (version < minimumRecommendedVersion)
            {
                LDtkDebug.LogError($"The version of the project \"{assetName}\" is outdated. It's a requirement to update your project to the latest supported version. ({version} < {minimumRecommendedVersion})");
                return false;
            }

            return true;
        }

        private void TryPrepareSpritePacking(LdtkJson json)
        {
            //allow the sprites to be gettable in the AssetDatabase properly; only after the import process
            if (_hadArtifactLoadProblem)
            {
                LDtkDebug.LogWarning($"Did not pack tile textures for {assetPath}, a previous tile error was encountered.");
                return;
            }
            
            if (_atlas == null || !LDtkProjectImporterAtlasPacker.UsesSpriteAtlas(json))
            {
                return;
            }
            
            LDtkProjectImporterAtlasPacker packer = new LDtkProjectImporterAtlasPacker(_atlas, assetPath);
            packer.TryPack();
        }

        private void HideArtifactAssets()
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
            json = FromJson<LdtkJson>();
            if (json != null)
            {
                return true;
            }

            ImportContext.LogImportError("LDtk: Json import error");
            return false;

        }

        private void CreateJsonAsset()
        {
            _jsonFile = ReadAssetText();
            _jsonFile.name += "_Json";
            ImportContext.AddObjectToAsset("jsonFile", _jsonFile, LDtkIconUtility.LoadListIcon());
        }
        private void TryCreateTableOfContents(LdtkJson json)
        {
            if (json.Toc.IsNullOrEmpty())
            {
                return;
            }
            LDtkTableOfContents toc = ScriptableObject.CreateInstance<LDtkTableOfContents>();
            toc.name += Path.GetFileNameWithoutExtension(assetPath) + "_Toc";
            toc.Initialize(json);
            ImportContext.AddObjectToAsset("toc", toc, LDtkIconUtility.LoadEntityIcon());
        }
        
        private void BufferEditorCache()
        {
            EditorApplication.delayCall += () =>
            {
                LDtkJsonEditorCache.ForceRefreshJson(assetPath);
            };
        }

        private void MainBuild(LdtkJson json)
        {
            LDtkProjectImporterFactory factory = new LDtkProjectImporterFactory(this);
            factory.Import(json);
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

        private void CreateArtifactsAsset(LdtkJson json) //the bank for storing the auto-generated items. //todo might belong in the artifacts factory?
        {
            _artifacts = ScriptableObject.CreateInstance<LDtkArtifactAssets>();
            _artifacts.name = AssetName + "_Assets";
            ImportContext.AddObjectToAsset("artifacts", _artifacts, (Texture2D)LDtkIconUtility.GetUnityIcon("Tilemap"));
            
            Profiler.BeginSample("CreateAllArtifacts");
            _artifactsFactory = new LDtkArtifactsFactory(ImportContext, _pixelsPerUnit, _artifacts);
            _artifactsFactory.CreateAllArtifacts(json);
            Profiler.EndSample();
        }
        
        public LDtkIntGridTile GetIntGridValueTile(string key) => GetSerializedImporterAsset(_intGridValues, key);
        public GameObject GetEntity(string key) => GetSerializedImporterAsset(_entities, key);
        private T GetSerializedImporterAsset<T>(IEnumerable<LDtkAsset<T>> input, string key) where T : Object //todo these should be indexed too for performance.
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
        
        public TileBase GetTileArtifact(TilesetDefinition tileset, Rect srcPos)
        {
            string relPath = FixRelPath(tileset);
            return GetArtifactAsset(_artifacts.GetIndexedTile, () => LDtkKeyFormatUtil.GetGetterSpriteOrTileAssetName(srcPos, relPath, tileset.PxHei));
        }
        public Sprite GetSpriteArtifact(TilesetDefinition tileset, Rect srcPos)
        {
            string relPath = FixRelPath(tileset);
            return GetArtifactAsset(_artifacts.GetIndexedSprite, () => LDtkKeyFormatUtil.GetGetterSpriteOrTileAssetName(srcPos, relPath, tileset.PxHei));
        }
        public Sprite GetBackgroundArtifact(Level level, int textureHeight)
        {
            return GetArtifactAsset(_artifacts.GetIndexedBackground, () => LDtkKeyFormatUtil.GetGetterSpriteOrTileAssetName(level.BgPos.UnityCropRect, level.BgRelPath, textureHeight));
        }
        private static string FixRelPath(TilesetDefinition tileset)
        {
            return tileset.IsEmbedAtlas ? LDtkProjectSettings.InternalIconsTexturePath : tileset.RelPath;
        }
        private delegate T AssetGetter<out T>(string assetName);
        private delegate string AssetNameSolver();
        private T GetArtifactAsset<T>(AssetGetter<T> getter, AssetNameSolver assetNameGetter) where T : Object //todo this may be able to belong in the artifacts factory
        {
            if (_artifacts == null)
            {
                LDtkDebug.LogError("Project importer's artifact assets was null, this needs to be cached");
                return null;
            }
            
            string assetName = assetNameGetter.Invoke(); 
            T asset = getter.Invoke(assetName);
            if (asset != null)
            {
                return asset;
            }

            
            //LDtkDebug.LogError($"Tried retrieving a {typeof(T).Name} from the importer's artifacts, but was null: \"{assetName}\"");
            _hadArtifactLoadProblem = true;
            return asset;
        }

        private void OnResetPPU()
        {
            if (_pixelsPerUnit > 0)
            {
                return;
            }

            int defaultGridSize = 16;
            if (!LDtkJsonDigger.GetDefaultGridSize(assetPath, ref defaultGridSize))
            {
                //if problem, then default to what LDtk also defaults to upon a new project
                _pixelsPerUnit = LDtkImporterConsts.DEFAULT_PPU;
                return;
            }
            SetPixelsPerUnit(defaultGridSize);
        }

        private void SetPixelsPerUnit(int ppu)
        {
            if (_pixelsPerUnit > 0)
            {
                return;
            }
            
            SerializedObject serializedObject = new SerializedObject(this);
            serializedObject.Update();

            SerializedProperty ppuProp = serializedObject.FindProperty(PIXELS_PER_UNIT);
            ppuProp.intValue = ppu;
            serializedObject.ApplyModifiedProperties();
        }
        
        public IEnumerable<TileBase> GetIntGridTiles()
        {
            return _intGridValues.Select(p => p.Asset).Cast<TileBase>().ToArray();
        }

        public void CacheArtifactsAsset()
        {
            if (_artifacts != null)
            {
                return;
            }
            
            _artifacts = AssetDatabase.LoadAssetAtPath<LDtkArtifactAssets>(assetPath);

            if (_artifacts == null)
            {
                LDtkDebug.LogError("Artifacts was null during the import, this should never happen");
            }
        }
    }
}
