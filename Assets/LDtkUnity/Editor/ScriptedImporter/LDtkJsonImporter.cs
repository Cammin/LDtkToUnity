using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    internal abstract class LDtkJsonImporter : ScriptedImporter
    {
        public const string REIMPORT_ON_DEPENDENCY_CHANGE = nameof(_reimportOnDependencyChange);
        [SerializeField] private bool _reimportOnDependencyChange = true;
        
        private readonly Dictionary<TilesetDefinition, LDtkTilesetImporter> _importersForDefs = new Dictionary<TilesetDefinition, LDtkTilesetImporter>();

        public LDtkDebugInstance Logger;

        public bool ReimportOnDependencyChange => _reimportOnDependencyChange;
        public AssetImportContext ImportContext { get; private set; }
        public string AssetName => Path.GetFileNameWithoutExtension(assetPath);
        public LDtkDefinitionObjectsCache DefinitionObjects { get; private set; }
        

        protected abstract string[] GetGatheredDependencies();
        
        public sealed override void OnImportAsset(AssetImportContext ctx)
        {
            ImportContext = ctx;
            Logger = new LDtkDebugInstance(ctx);
            DefinitionObjects = new LDtkDefinitionObjectsCache(Logger);
            
            if (LDtkPrefs.VerboseLogging)
            {
                LDtkDebug.Log($"OnImportAsset {GetType().Name} {assetPath}");
            }

            try
            {
                MainImport();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                FailImport();
            }
            finally
            {
#if !UNITY_2022_2_OR_NEWER
                Logger._entries.WriteTheEntries();
#endif
            }

            //serialize dependencies to display them in the inspector for easier dependency tracking.
            Profiler.BeginSample("SerializeStringDependencies");
            LDtkDependencyCache.Set(assetPath, GetGatheredDependencies()); 
            Profiler.EndSample();
        }

        private void MainImport()
        {
            string path = Path.GetFileName(assetPath);
            using (new LDtkProfiler.Scope(path))
            {
                Import();
            }
        }

        protected abstract void Import();
        
        public bool IsProjectExists(string projectPath = null)
        {
            if (this is LDtkProjectImporter)
            {
                return true;
            }

            if (projectPath == null)
            {
                projectPath = new LDtkRelativeGetterProjectImporter().GetPath(assetPath, assetPath);
            }
            return File.Exists(projectPath);
        }
        
        /// <summary>
        /// both ldtk and ldtkl files can be backups. the level files are in a subdirectory from a backup folder
        /// ldtkt files, on the other hand, should be okay to not check
        /// </summary>
        public bool IsBackupFile(string projectPath = null)
        {
            if (this is LDtkProjectImporter)
            {
                return LDtkPathUtility.IsFileBackupFile(assetPath, assetPath);
            }

            if (projectPath == null)
            {
                projectPath = new LDtkRelativeGetterProjectImporter().GetPath(assetPath, assetPath);
            }
            return LDtkPathUtility.IsFileBackupFile(assetPath, projectPath);
        }
        
        public bool IsVersionOutdated(string projectPath = null)
        {
            Profiler.BeginSample("CheckOutdatedJsonVersion");
            if (projectPath == null)
            {
                projectPath = this is LDtkProjectImporter ? assetPath : new LDtkRelativeGetterProjectImporter().GetPath(assetPath, assetPath);
            }
            
            string version = "";
            LDtkJsonDigger.GetJsonVersion(projectPath, ref version);
            bool valid = CheckOutdatedJsonVersion(version, AssetName, Logger);
            
            Profiler.EndSample();
            return !valid;
        }
        
        public static bool CheckOutdatedJsonVersion(string jsonVersion, string assetName, LDtkDebugInstance projectCtx = null)
        {
            jsonVersion = Regex.Replace(jsonVersion, "[^0-9.]", "");
            if (!Version.TryParse(jsonVersion, out Version version))
            {
                LDtkDebug.LogError($"This json asset \"{assetName}\" couldn't parse it's version \"{jsonVersion}\", post an issue to the developer", projectCtx);
                return false;
            }

            Version minimumRecommendedVersion = new Version(LDtkImporterConsts.MINIMUM_JSON_VERSION);
            if (version < minimumRecommendedVersion)
            {
                LDtkDebug.LogError($"The version of the project \"{assetName}\" is outdated. It's a requirement to update your project to the latest supported version. ({version} < {minimumRecommendedVersion})", projectCtx);
                return false;
            }

            return true;
        }

        protected void CacheSchemaDefs(LdtkJson json, Level separateLevel = null)
        {
            LDtkUidBank.CacheUidData(json);
            LDtkIidBank.CacheIidData(json, separateLevel);
            
        }

        protected void ReleaseDefs()
        {
            LDtkUidBank.ReleaseDefinitions();
            LDtkIidBank.Release();
        }
        
        public LDtkProjectImporter GetProjectImporter()
        {
            if (this is LDtkProjectImporter projectImporter)
            {
                return projectImporter;
            }
            
            LDtkRelativeGetterProjectImporter getter = new LDtkRelativeGetterProjectImporter();
            projectImporter = getter.GetRelativeAsset(assetPath, assetPath, (path) => (LDtkProjectImporter)GetAtPath(path));
            return projectImporter;
        }
        public string GetProjectPath()
        {
            LDtkRelativeGetterProjectImporter getter = new LDtkRelativeGetterProjectImporter();
            return getter.GetPath(assetPath, assetPath);
        }
        
        //this is nicely optimized to grab a tile by index instead of searching by name
        public TileBase GetTileArtifact(LDtkProjectImporter project, TilesetDefinition def, int tileID)
        {
            LDtkArtifactAssetsTileset artifacts = LoadTilesetArtifacts(project, def);
            if (artifacts == null)
            {
                return null;
            }

            LDtkTilesetTile element = artifacts._tiles.ElementAtOrDefault(tileID);
            if (element)
            {
                return element;
            }

            Logger.LogError($"Failed to load a tile artifact at id \"{tileID}\" from \"{def.Identifier}\"");
            return null;
        }
        
        public Sprite GetAdditionalSprite(LDtkProjectImporter project, TilesetDefinition def, Rect id)
        {
            LDtkArtifactAssetsTileset artifacts = LoadTilesetArtifacts(project, def);
            if (artifacts == null)
            {
                return null;
            }

            Sprite sprite = null;
            
            Profiler.BeginSample("GetAdditionalSpriteForRectByNameCheck");
            sprite = artifacts.GetAdditionalSpriteForRect(id, def.PxHei);
            Profiler.EndSample();
            if (sprite)
            {
                return sprite;
            }
            /*Profiler.BeginSample("GetAdditionalSpriteForRect");
            sprite = artifacts.GetAdditionalSpriteForRect(id, def);
            Profiler.EndSample();
            if (sprite)
            {
                return sprite;
            }*/
            
            Logger.LogError($"Failed to load an additional sprite at id \"{id}\" from \"{def.Identifier}\"");
            return null;
        }
        
        protected Dictionary<int, LDtkArtifactAssetsTileset> MakeTilesetDict(LDtkProjectImporter project, LdtkJson json)
        {
            //construct a dictionary to get artifacts by tileset uid
            Dictionary<int, LDtkArtifactAssetsTileset> artifacts = new Dictionary<int, LDtkArtifactAssetsTileset>();
            foreach (TilesetDefinition def in json.Defs.Tilesets)
            {
                artifacts.Add(def.Uid, LoadTilesetArtifacts(project, def));
            }

            return artifacts;
        }
        
        internal LDtkArtifactAssetsTileset LoadTilesetArtifacts(LDtkProjectImporter project, TilesetDefinition def)
        {
            //if no texture is set in LDtk
            if (!def.IsEmbedAtlas && def.RelPath == null)
            {
                return null;
            }
            
            LDtkTilesetImporter tilesetImporter = LoadAndCacheTilesetImporter(def);
            if (tilesetImporter == null)
            {
                return null;
            }
            
            if (tilesetImporter._pixelsPerUnit != project.PixelsPerUnit)
            {
                Logger.LogWarning($"The tileset file \"{tilesetImporter.AssetName}\" ({tilesetImporter._pixelsPerUnit}) doesn't have the same pixels per unit as it's project \"{AssetName}\" ({project.PixelsPerUnit}). Ensure they match.");
            }

            LDtkArtifactAssetsTileset artifacts = tilesetImporter.LoadArtifacts(Logger);
            if (artifacts == null)
            {
                return null;
            }

            return artifacts;
        }

        private LDtkTilesetImporter LoadAndCacheTilesetImporter(TilesetDefinition def)
        {
            if (_importersForDefs.TryGetValue(def, out LDtkTilesetImporter importer))
            {
                return importer;
            }
            
            string path = TilesetImporterPath(assetPath, def.Identifier);

            if (!File.Exists(path))
            {
                Logger.LogError($"Failed to find the required tileset file at \"{path}\". Ensure that LDtk exported a tileset file through a custom command. If the command wasn't configured yet, check the project inspector for more info.");
                _importersForDefs.Add(def, null);
                return null;
            }
                
            importer = (LDtkTilesetImporter)GetAtPath(path);
            if (importer == null)
            {
                Logger.LogError($"Failed to load the tileset importer at \"{path}\", but the file exists. The tileset file may have failed to import?");
                _importersForDefs.Add(def, null);
                return null;
            }

            _importersForDefs.Add(def, importer);
            return importer;
        }
        
        public static string TilesetImporterPath(string importerPath, string tilesetDefIdentifier)
        {
            string ext = Path.GetExtension(importerPath);
            if (ext != ".ldtk")
            {
                importerPath = new LDtkRelativeGetterProjectImporter().GetPath(importerPath, importerPath);
            }
            
            string directoryName = Path.GetDirectoryName(importerPath);
            string projectName = Path.GetFileNameWithoutExtension(importerPath);

            if (directoryName == null)
            {
                LDtkDebug.LogError($"Issue formulating a tileset definition path; Path was invalid for: \"{importerPath}\"");
                return null;
            }
            
            return Path.Combine(directoryName, projectName, tilesetDefIdentifier) + '.' + LDtkImporterConsts.TILESET_EXT;
        }
        
        protected void FailImport()
        {
            GameObject failedAsset = new GameObject("DefaultAsset");

            if (this is LDtkProjectImporter)
            {
                ImportContext.AddObjectToAsset("failedImport", failedAsset, LDtkIconUtility.LoadProjectFileIcon(true));
            }
            if (this is LDtkLevelImporter)
            {
                ImportContext.AddObjectToAsset("failedImport", failedAsset, LDtkIconUtility.LoadLevelFileIcon(true));
            }
            if (this is LDtkTilesetImporter)
            {
                ImportContext.AddObjectToAsset("failedImport", failedAsset, LDtkIconUtility.LoadTilesetFileIcon(true));
            }
            
            ImportContext.SetMainObject(failedAsset);
        }
    }
    
    internal abstract class LDtkJsonImporter<T> : LDtkJsonImporter where T : ScriptableObject, ILDtkJsonFile
    {
        protected T ReadAssetText()
        {
            string jsonText = File.ReadAllText(assetPath);
            
            T file = ScriptableObject.CreateInstance<T>();
            file.name = Path.GetFileNameWithoutExtension(assetPath) + "_Json";
            file.SetJson(jsonText);
            return file;
        }
        
        public TJson FromJson<TJson>()
        {
            return FromJson<TJson>(assetPath, Logger);
        }
        
        public static TJson FromJson<TJson>(string path, LDtkDebugInstance debug = null)
        {
            if (!File.Exists(path))
            {
                LDtkDebug.LogError($"Could not find the json file to deserialize at \"{path}\"", debug);
                return default;
            }
            
            Profiler.BeginSample($"FromJson {typeof(TJson).Name}");
            
            Profiler.BeginSample($"ReadAllBytes");
            byte[] bytes = File.ReadAllBytes(path);
            Profiler.EndSample();
            
            Profiler.BeginSample($"FromJson");
            TJson json = default;
            try
            {
                json = Utf8Json.JsonSerializer.Deserialize<TJson>(bytes);
            }
            catch (Exception e)
            {
                LDtkDebug.LogError($"Failure to deserialize json: {e}", debug);
            }
            Profiler.EndSample();
        
            Profiler.EndSample(); //this end sample for the caller up the stack
            return json;
        }
    }
}