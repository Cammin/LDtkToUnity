using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
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
        public const string SCALE_ENTITIES = nameof(_scaleEntities);
        
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
        [SerializeField] private bool _scaleEntities = true;
        
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
        public bool ScaleEntities => _scaleEntities;

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

            LDtkProfiler.BeginWriting($"GatherDependenciesFromSourceFile/{Path.GetFileName(path)}");
            _previousDependencies = LDtkProjectDependencyFactory.GatherProjectDependencies(path);
            LDtkProfiler.EndWriting();

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
            
            LDtkProfiler.BeginSample("CreateJsonAsset");
            CreateJsonAsset();
            LDtkProfiler.EndSample();
            
            if (!TryGetJson(out LdtkJson json))
            {
                Logger.LogError("Json deserialization error. Not importing.");
                BufferEditorCache();
                FailImport();
                return;
            }
            
            LDtkProfiler.BeginSample("CacheSchemaDefs");
            CacheSchemaDefs(json);
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("CreateArtifactAsset");
            CreateArtifactAsset(json);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("MakeDefObjects");
            MakeDefObjects(json);
            LDtkProfiler.EndSample();
            
            //if for whatever reason (or backwards compatibility), if the ppu is -1 in any capacity
            LDtkProfiler.BeginSample("SetPixelsPerUnit");
            LDtkPpuInitializer ppu = new LDtkPpuInitializer(_pixelsPerUnit, assetPath, assetPath);
            if (ppu.TryInitializePixelsPerUnit(json.DefaultGridSize))
            {
                _pixelsPerUnit = ppu.PixelsPerUnit;
                EditorUtility.SetDirty(this);
            }
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("CreateTableOfContents");
            TryCreateTableOfContents(json);
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("MainBuild");
            MainBuild(json);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("TryGenerateEnums");
            TryGenerateEnums(json);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("CreateConfigurationFile");
            GenerateConfigurationFile(json);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("BufferEditorCache");
            BufferEditorCache();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("ReleaseDefs");
            ReleaseDefs();
            LDtkProfiler.EndSample();
        }

        private void MakeDefObjects(LdtkJson json)
        {
            Dictionary<int, LDtkArtifactAssetsTileset> artifacts = MakeTilesetDict(this, json);

            LDtkProfiler.BeginSample("InitializeFromProject");
            DefinitionObjects.InitializeFromProject(json.Defs, artifacts);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("Set _definitions");
            _artifacts._definitions = DefinitionObjects.Defs;
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("AddAllObjectsToAsset");
            foreach (var obj in DefinitionObjects.Defs)
            {
                if (obj is ILDtkUid uid)
                {
                    ImportContext.AddObjectToAsset($"Uid_{uid.Uid}", obj);
                    continue;
                }
                Logger.LogError($"{obj.name} is not a uid! This should never happen", obj);
            }
            LDtkProfiler.EndSample();
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
        /// <summary>
        /// Should process after all the definition scriptable objects are created so that they are accessible in this context
        /// </summary>
        /// <param name="json"></param>
        private void TryCreateTableOfContents(LdtkJson json)
        {
            if (json.Toc.IsNullOrEmpty())
            {
                return;
            }
            
            Toc = ScriptableObject.CreateInstance<LDtkTableOfContents>();
            Toc.name += AssetName + "_Toc";

            LDtkTocFieldFactory factory = new LDtkTocFieldFactory(json, this, this);
            
            LDtkProfiler.BeginSample("Toc_IndexEntitiesAndFieldsByIdentifiers");
            factory.IndexEntitiesAndFieldsByIdentifiers();
            LDtkProfiler.EndSample();

            Toc.InitializeList(json);
            
            LDtkFieldParser.CacheRecentBuilder(null);
            LDtkProfiler.BeginSample("Toc_GenerateAndAddEntries");
            foreach (LdtkTableOfContentEntry tocEntry in json.Toc)
            {
                var output = factory.GenerateFieldsFromTocEntry(tocEntry);
                Toc.AddEntry(tocEntry, output.Definition, output.Fields);
            }
            LDtkProfiler.EndSample();
            
            
            ImportContext.AddObjectToAsset("toc", Toc, LDtkIconUtility.LoadListIcon());
        }

        private void GenerateConfigurationFile(LdtkJson json)
        {
            //only generate the file if separate levels is used
            if (!json.ExternalLevels) return;

            LDtkConfigData config = new LDtkConfigData()
            {
                PixelsPerUnit = _pixelsPerUnit,
                CustomLevelPrefab = _customLevelPrefab,
                IntGridValueColorsVisible = _intGridValueColorsVisible,
                UseCompositeCollider = _useCompositeCollider,
                GeometryType = _geometryType,
                CreateBackgroundColor = _createBackgroundColor,
                CreateLevelBoundsTrigger = _createLevelBoundsTrigger,
                UseParallax = _useParallax,
                IntGridValues = _intGridValues,
                Entities = _entities,
            };
            string writePath = config.WriteJson(assetPath);
            
            //importing the asset if it doesn't exist due to the asset database not refreshing this automatically
            AssetDatabase.ImportAsset(writePath);
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
            LDtkProfiler.BeginSample("SetupPreprocessors");
            var preAction = new LDtkAssetProcessorActionCache();
            LDtkAssetProcessorInvoker.AddPreProcessProject(preAction, json, AssetName);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("RunPreprocessors");
            preAction.Process();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("ImportProject");
            LDtkBuilderProjectFactory factory = new LDtkBuilderProjectFactory(this);
            factory.Import(json);
            LDtkProfiler.EndSample();
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
            
            LDtkProfiler.BeginSample("CreateAllBackgrounds");
            LDtkBackgroundSliceCreator bgMaker = new LDtkBackgroundSliceCreator(this);
            List<Sprite> allBackgrounds = bgMaker.CreateAllBackgrounds(json);
            LDtkProfiler.EndSample();
            
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
