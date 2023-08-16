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
using Utf8Json;
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
        public const string CUSTOM_LEVEL_PREFAB = nameof(_customLevelPrefab);
        public const string DEPARENT_IN_RUNTIME = nameof(_deparentInRuntime);
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string USE_COMPOSITE_COLLIDER = nameof(_useCompositeCollider);
        public const string GEOMETRY_TYPE = nameof(_geometryType);
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
        [SerializeField] private GameObject _customLevelPrefab = null;
        [SerializeField] private bool _deparentInRuntime = false;
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private bool _useCompositeCollider = true;
        [SerializeField] private CompositeCollider2D.GeometryType _geometryType = CompositeCollider2D.GeometryType.Outlines;
        [SerializeField] private bool _createBackgroundColor = true;
        [SerializeField] private bool _createLevelBoundsTrigger = false;
        
        [SerializeField] private LDtkAssetIntGridValue[] _intGridValues = Array.Empty<LDtkAssetIntGridValue>();
        
        [SerializeField] private LDtkAssetEntity[] _entities = Array.Empty<LDtkAssetEntity>();
        
        [SerializeField] private bool _enumGenerate = false;
        [SerializeField] private string _enumPath = null;
        [SerializeField] private string _enumNamespace = string.Empty;

        
        public LDtkProjectFile JsonFile => _jsonFile;
        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public int PixelsPerUnit => _pixelsPerUnit;
        public bool DeparentInRuntime => _deparentInRuntime;
        public GameObject CustomLevelPrefab => _customLevelPrefab;
        public bool UseCompositeCollider => _useCompositeCollider;
        public CompositeCollider2D.GeometryType GeometryType => _geometryType;
        public bool CreateBackgroundColor => _createBackgroundColor;
        public bool CreateLevelBoundsTrigger => _createLevelBoundsTrigger;

        //all of these are wiped after the entire import is done
        private LDtkArtifactAssets _backgroundArtifacts;
        private static string[] _previousDependencies;
        private readonly Dictionary<TilesetDefinition, LDtkTilesetImporter> _importersForDefs = new Dictionary<TilesetDefinition, LDtkTilesetImporter>();
        
        //this will run upon standard reset, but also upon the meta file generation during the first import
        private void Reset()
        {
            OnResetPPU();
        }

        private static string[] GatherDependenciesFromSourceFile(string path)
        {
            if (LDtkPrefs.VerboseLogging)
            {
                LDtkDebug.Log($"GatherDependenciesFromSourceFile Project {path}");
            }

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

            Profiler.BeginSample("CheckOutdatedJsonVersion");
            string version = "";
            LDtkJsonDigger.GetJsonVersion(assetPath, ref version);
            if (!CheckOutdatedJsonVersion(version, AssetName, Logger))
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
                Logger.LogError("Json deserialization error. Not importing.");
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
            CreateBackgroundArtifacts(json);
            Profiler.EndSample();
            

            Profiler.BeginSample("MainBuild");
            MainBuild(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("TryGenerateEnums");
            TryGenerateEnums(json);
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

        public static bool CheckOutdatedJsonVersion(string jsonVersion, string assetName, LDtkDebugInstance projectCtx = null)
        {
            jsonVersion = Regex.Replace(jsonVersion, "[^0-9.]", "");
            if (!Version.TryParse(jsonVersion, out Version version))
            {
                LDtkDebug.LogError($"This json asset \"{assetName}\" couldn't parse it's version \"{jsonVersion}\", post an issue to the developer", projectCtx);
                return false;
            }

            Version minimumRecommendedVersion = new Version(LDtkImporterConsts.LDTK_JSON_VERSION);
            if (version < minimumRecommendedVersion)
            {
                LDtkDebug.LogError($"The version of the project \"{assetName}\" is outdated. It's a requirement to update your project to the latest supported version. ({version} < {minimumRecommendedVersion})", projectCtx);
                return false;
            }

            return true;
        }

        private bool TryGetJson(out LdtkJson json)
        {
            json = FromJson<LdtkJson>();
            if (json != null)
            {
                return true;
            }

            Logger.LogError("LDtk: Json import error");
            return false;
        }

        private void CreateJsonAsset()
        {
            _jsonFile = ReadAssetText();
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
            LDtkBuilderProjectFactory factory = new LDtkBuilderProjectFactory(this);
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

        private void CreateBackgroundArtifacts(LdtkJson json)
        {
            Profiler.BeginSample("CreateAllBackgrounds");
            LDtkBackgroundSliceCreator bgMaker = new LDtkBackgroundSliceCreator(this);
            List<Sprite> allBackgrounds = bgMaker.CreateAllBackgrounds(json);
            Profiler.EndSample();
            
            _backgroundArtifacts = ScriptableObject.CreateInstance<LDtkArtifactAssets>();
            _backgroundArtifacts.name = AssetName + "_Backgrounds";
            _backgroundArtifacts._backgrounds = new List<Sprite>(allBackgrounds);
            foreach (Sprite bg in allBackgrounds)
            {
                ImportContext.AddObjectToAsset($"bg_{bg.name}", bg, (Texture2D)LDtkIconUtility.GetUnityIcon("Sprite"));
            }
            ImportContext.AddObjectToAsset("artifacts", _backgroundArtifacts, (Texture2D)LDtkIconUtility.GetUnityIcon("Image"));
        }
        
        public TileBase GetIntGridValueTile(string key) => GetSerializedImporterAsset(_intGridValues, key);
        public GameObject GetEntity(string key) => GetSerializedImporterAsset(_entities, key);
        private T GetSerializedImporterAsset<T>(IEnumerable<LDtkAsset<T>> input, string key) where T : Object //todo these should be indexed too for performance.
        {
            if (input == null)
            {
                Logger.LogError("LDtk: Tried getting an asset from the build data but the array was null. Is the project asset properly saved?");
                return default;
            }

            foreach (LDtkAsset<T> asset in input)
            {
                if (ReferenceEquals(asset, null))
                {
                    Logger.LogError($"LDtk: A field in the build data is null.");
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
        
        //this is nicely optimized to grab a tile by index instead of searching by name
        public TileBase GetTileArtifact(TilesetDefinition def, int tileID)
        {
            //todo just pass the artifact assets straight into the tilemap builder instead of trying to access an asset from this class?
            LDtkArtifactAssetsTileset artifacts = LoadTilesetArtifacts(def);
            return artifacts == null ? null : artifacts._tiles[tileID];
        }
        
        //this is nicely optimized to grab a tile by index instead of searching by name
        public Sprite GetAdditionalSprite(TilesetDefinition def, Rect id)
        {
            LDtkArtifactAssetsTileset artifacts = LoadTilesetArtifacts(def);
            return artifacts == null ? null : artifacts.GetAdditionalSpriteForRectSlow(id, def.PxHei);
        }

        private LDtkArtifactAssetsTileset LoadTilesetArtifacts(TilesetDefinition def)
        {
            LDtkTilesetImporter importer = LoadAndCacheTilesetImporter(def);
            if (importer == null)
            {
                return null;
            }

            LDtkArtifactAssetsTileset artifacts = importer.LoadArtifacts(Logger);
            if (artifacts == null)
            {
                return null;
            }

            return artifacts;
        }

        public Sprite GetBackgroundArtifact(Level level)
        {
            if (_backgroundArtifacts == null)
            {
                Logger.LogError("Project importer's artifact assets was null, this needs to be cached");
                return null;
            }
        
            string assetName = level.Identifier; 
            Sprite asset = _backgroundArtifacts.GetBackgroundSlow(assetName);
            if (asset != null)
            { 
                return asset;
            }
        
            Logger.LogError($"Tried retrieving a background from the importer's artifacts, but was null: \"{assetName}\"");
            return asset;
        }

        public LDtkTilesetImporter LoadAndCacheTilesetImporter(TilesetDefinition def)
        {
            if (_importersForDefs.TryGetValue(def, out LDtkTilesetImporter importer))
            {
                return importer;
            }
            
            string path = TilesetImporterPath(assetPath, def.Identifier);

            if (!File.Exists(path))
            {
                Logger.LogError($"Failed to find the required tileset file at \"{path}\". Ensure that LDtk exported a tileset file through a custom command. If the command wasn't configured yet, check the project inspector for more info.");
                return null;
            }
                
            importer = (LDtkTilesetImporter)GetAtPath(path);
            if (importer == null)
            {
                Logger.LogError($"Failed to find the required tileset file importer at \"{path}\", but the file exists. The tileset file may have failed to import?");
                return null;
            }
                
            _importersForDefs.Add(def, importer);
            return importer;
        }
        
        public static string TilesetImporterPath(string projectPath, string tilesetDefIdentifier)
        {
            string directoryName = Path.GetDirectoryName(projectPath);
            string projectName = Path.GetFileNameWithoutExtension(projectPath);

            if (directoryName == null)
            {
                LDtkDebug.LogError($"Issue formulating a tileset definition path; Path was invalid for: \"{projectPath}\"");
                return null;
            }
            
            return Path.Combine(directoryName, projectName, tilesetDefIdentifier) + '.' + LDtkImporterConsts.TILESET_EXT;
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

        public void TryCacheArtifactsAsset()
        {
            if (_backgroundArtifacts != null)
            {
                return;
            }
            
            _backgroundArtifacts = AssetDatabase.LoadAssetAtPath<LDtkArtifactAssets>(assetPath);
            if (_backgroundArtifacts == null)
            {
                Logger.LogError($"Artifacts was null during the import, this should never happen. Does the sub asset not exist for \"{assetPath}\"?");
            }
        }
    }
}
