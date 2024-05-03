using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;
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
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string USE_COMPOSITE_COLLIDER = nameof(_useCompositeCollider);
        public const string GEOMETRY_TYPE = nameof(_geometryType);
        public const string CREATE_BACKGROUND_COLOR = nameof(_createBackgroundColor);
        public const string CREATE_LEVEL_BOUNDS_TRIGGER = nameof(_createLevelBoundsTrigger);
        public const string USE_PARALLAX = nameof(_useParallax);
        
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
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private bool _useCompositeCollider = true;
        [SerializeField] private CompositeCollider2D.GeometryType _geometryType = CompositeCollider2D.GeometryType.Outlines;
        [SerializeField] private bool _createBackgroundColor = true;
        [SerializeField] private bool _createLevelBoundsTrigger = false;
        [SerializeField] private bool _useParallax = true;
        
        [SerializeField] private LDtkAssetIntGridValue[] _intGridValues = Array.Empty<LDtkAssetIntGridValue>();
        
        [SerializeField] private LDtkAssetEntity[] _entities = Array.Empty<LDtkAssetEntity>();
        
        [SerializeField] private bool _enumGenerate = false;
        [SerializeField] private string _enumPath = null;
        [SerializeField] private string _enumNamespace = string.Empty;

        
        public LDtkProjectFile JsonFile => _jsonFile;
        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public int PixelsPerUnit => _pixelsPerUnit;
        public GameObject CustomLevelPrefab => _customLevelPrefab;
        public bool UseCompositeCollider => _useCompositeCollider;
        public CompositeCollider2D.GeometryType GeometryType => _geometryType;
        public bool CreateBackgroundColor => _createBackgroundColor;
        public bool CreateLevelBoundsTrigger => _createLevelBoundsTrigger;
        public bool UseParallax => _useParallax;

        //all of these are wiped after the entire import is done
        private LDtkArtifactAssets _artifacts;
        private static string[] _previousDependencies;
        public LDtkTableOfContents Toc { get; private set; }

        //this will run upon standard reset, but also upon the meta file generation during the first import
        private void Reset()
        {
            LDtkPpuInitializer ppu = new LDtkPpuInitializer(_pixelsPerUnit, assetPath, assetPath);
            if (ppu.OnResetImporter())
            {
                _pixelsPerUnit = ppu.PixelsPerUnit;
                EditorUtility.SetDirty(this);
                SaveAndReimport();
            }
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
                FailImport();
                return;
            }
            
            if (IsVersionOutdated())
            {
                BufferEditorCache();
                FailImport();
                return;
            }
            
            Profiler.BeginSample("CreateJsonAsset");
            CreateJsonAsset();
            Profiler.EndSample();
            
            if (!TryGetJson(out LdtkJson json))
            {
                Logger.LogError("Json deserialization error. Not importing.");
                BufferEditorCache();
                FailImport();
                return;
            }
            
            Profiler.BeginSample("CacheSchemaDefs");
            CacheSchemaDefs(json);
            Profiler.EndSample();

            Profiler.BeginSample("CreateArtifactAsset");
            CreateArtifactAsset(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("MakeDefObjects");
            MakeDefObjects(json);
            Profiler.EndSample();
            
            //if for whatever reason (or backwards compatibility), if the ppu is -1 in any capacity
            Profiler.BeginSample("SetPixelsPerUnit");
            LDtkPpuInitializer ppu = new LDtkPpuInitializer(_pixelsPerUnit, assetPath, assetPath);
            if (ppu.TryInitializePixelsPerUnit(json.DefaultGridSize))
            {
                _pixelsPerUnit = ppu.PixelsPerUnit;
                EditorUtility.SetDirty(this);
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("CreateTableOfContents");
            TryCreateTableOfContents(json);
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

        private void MakeDefObjects(LdtkJson json)
        {
            Dictionary<int, LDtkArtifactAssetsTileset> artifacts = MakeTilesetDict(this, json);

            DefinitionObjects.InitializeFromProject(json.Defs, artifacts);
            _artifacts._definitions = DefinitionObjects.Defs;
            
            foreach (var obj in DefinitionObjects.Defs)
            {
                if (obj is ILDtkUid uid)
                {
                    ImportContext.AddObjectToAsset($"Uid_{uid.Uid}", obj);
                    continue;
                }
                Logger.LogError($"{obj.name} is not a uid! This should never happen", obj);
            }
        }

        private static void CheckDefaultEditorBehaviour()
        {
            if (EditorSettings.defaultBehaviorMode != EditorBehaviorMode.Mode2D)
            {
                LDtkDebug.LogWarning("It is encouraged to use 2D project mode while using LDtkToUnity. Change it in \"Project Settings > Editor > Default Behaviour Mode\"");
            }
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
            
            Toc = ScriptableObject.CreateInstance<LDtkTableOfContents>();
            Toc.name += Path.GetFileNameWithoutExtension(assetPath) + "_Toc";
            Toc.Initialize(json);
            ImportContext.AddObjectToAsset("toc", Toc, LDtkIconUtility.LoadListIcon());
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
            var preAction = new LDtkAssetProcessorActionCache();
            LDtkAssetProcessorInvoker.AddPreProcessProject(preAction, json, AssetName);
            preAction.Process();
            
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

        private void CreateArtifactAsset(LdtkJson json)
        {
            _artifacts = ScriptableObject.CreateInstance<LDtkArtifactAssets>();
            _artifacts.name = AssetName + "_Artifacts";
            
            Profiler.BeginSample("CreateAllBackgrounds");
            LDtkBackgroundSliceCreator bgMaker = new LDtkBackgroundSliceCreator(this);
            List<Sprite> allBackgrounds = bgMaker.CreateAllBackgrounds(json);
            Profiler.EndSample();
            
            _artifacts._backgrounds = new List<Sprite>(allBackgrounds);
            foreach (Sprite bg in allBackgrounds)
            {
                //it's possible that some could be null if there was an illegal slice
                if (bg == null)
                {
                    continue;
                }
                ImportContext.AddObjectToAsset($"bg_{bg.name}", bg, (Texture2D)LDtkIconUtility.GetUnityIcon("Sprite"));
            }
            ImportContext.AddObjectToAsset("artifacts", _artifacts, (Texture2D)LDtkIconUtility.GetUnityIcon("Image"));
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
        
        public void TryCacheArtifactsAsset(LDtkDebugInstance logger)
        {
            if (_artifacts != null)
            {
                return;
            }
            
            _artifacts = AssetDatabase.LoadAssetAtPath<LDtkArtifactAssets>(assetPath);
            if (_artifacts == null)
            {
                logger.LogError($"Artifacts was null during the import, likely due to a failed import for the project at \"{assetPath}\". Investigate other problems first.");
            }
        }
        public LDtkArtifactAssets GetArtifactAssets()
        {
            if (_artifacts == null)
            {
                Logger.LogError("Project importer's artifact assets was null, this needs to be cached");
                return null;
            }
            return _artifacts;
        }
        public Sprite GetBackgroundArtifact(Level level)
        {
            if (_artifacts == null)
            {
                Logger.LogError("Project importer's artifact assets was null, this needs to be cached");
                return null;
            }

            LDtkArtifactAssets ldtkArtifactAssets = GetArtifactAssets();
            string assetName = level.Identifier; 
            
            Sprite asset = ldtkArtifactAssets.GetBackgroundSlow(assetName);
            if (asset != null)
            { 
                return asset;
            }
            Logger.LogError($"Tried retrieving a background from the importer's artifacts, but was null: \"{assetName}\"");
            return asset;
        }
    }
}
