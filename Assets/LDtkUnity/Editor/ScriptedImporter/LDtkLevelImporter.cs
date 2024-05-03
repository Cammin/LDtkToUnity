using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.IMPORTER_LDTK_LEVEL)]
    [ScriptedImporter(LDtkImporterConsts.LEVEL_VERSION, LDtkImporterConsts.LEVEL_EXT, LDtkImporterConsts.LEVEL_ORDER)]
    internal sealed class LDtkLevelImporter : LDtkJsonImporter<LDtkLevelFile>
    {
        //statically reset this upon the editor application delay call so that we only need to deserialize json data once
        private static readonly Dictionary<LDtkProjectImporter, LdtkJson> Jsons = new Dictionary<LDtkProjectImporter, LdtkJson>();
        private static Texture2D _icon;
        
        private LDtkProjectImporter _projectImporter;
        private LdtkJson _projectJson;
        
        private LDtkLevelFile _levelFile;
        private Level _levelJson;
        private static string[] _previousDependencies;
        
        private static string[] GatherDependenciesFromSourceFile(string path)
        {
            if (LDtkPrefs.VerboseLogging)
            {
                LDtkDebug.Log($"GatherDependenciesFromSourceFile Level {path}");
            }
            
            LDtkProfiler.BeginSample($"GatherDependenciesFromSourceFile/{Path.GetFileName(path)}");
            _previousDependencies = LDtkLevelDependencyFactory.GatherLevelDependencies(path);
            LDtkProfiler.EndSample();
            
            return _previousDependencies;
        }

        protected override string[] GetGatheredDependencies() => _previousDependencies;

        protected override void Import()
        {
            string projectPath = new LDtkRelativeGetterProjectImporter().GetPath(assetPath, assetPath);
            if (!IsProjectExists(projectPath))
            {
                Logger.LogError($"This level can't find it's project: \"{assetPath}\"");
                return;
            }
            
            if (IsBackupFile(projectPath))
            {
                //we don't log anything for the backup check
                FailImport();
                return;
            }
            
            if (IsVersionOutdated(projectPath))
            {
                //it will log from the version check
                FailImport();
                return;
            }
            
            if (_icon == null)
            {
                _icon = LDtkIconUtility.LoadLevelFileIcon();
            }
            
            //instead of grabbing the level from the project that built the level in that hierarchy, build the level from this json file directly to help individualize level building to only what's changed
            //that being said, the level importer still has important dependencies to the project importer like tile assets, entities, and any other artifacts.
            
            Profiler.BeginSample("DeserializeAndAssign");
            if (!DeserializeAndAssign())
            {
                Profiler.EndSample();
                FailImport();
                return;
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("CacheArtifactsAsset");
            _projectImporter.TryCacheArtifactsAsset(Logger);
            Profiler.EndSample();

            Profiler.BeginSample("CacheDefs");
            CacheSchemaDefs(_projectJson, _levelJson);
            Profiler.EndSample();
            
            Profiler.BeginSample("InitializeDefinitionObjectsFromLevel");
            var tilesets = MakeTilesetDict(_projectImporter, _projectJson);
            DefinitionObjects.InitializeFromLevel(_projectImporter.GetArtifactAssets()._definitions, tilesets);
            Profiler.EndSample();
            
            Profiler.BeginSample("BuildLevel");
            BuildLevel();
            Profiler.EndSample();
            
            Profiler.BeginSample("ReleaseDefs");
            ReleaseDefs();
            Profiler.EndSample();
        }

        private void BuildLevel()
        {
            var preAction = new LDtkAssetProcessorActionCache();
            LDtkAssetProcessorInvoker.AddPreProcessLevel(preAction, _levelJson, _projectJson, AssetName);
            preAction.Process();
            
            LDtkAssetProcessorActionCache assetProcess = new LDtkAssetProcessorActionCache();

            
            
            LDtkBuilderLevel levelBuilder = new LDtkBuilderLevel(_projectImporter, _projectJson, WorldLayout.Free, _levelJson, _levelFile, assetProcess, this, null, null);
            GameObject levelRoot = levelBuilder.StubGameObject();
            
            Profiler.BeginSample($"BuildSeparateLevel {_levelJson.Identifier}");
            levelBuilder.BuildLevel();
            Profiler.EndSample();
            
            assetProcess.Process();

            ImportContext.AddObjectToAsset("levelRoot", levelRoot, _icon);
            ImportContext.SetMainObject(levelRoot);
        }

        private bool DeserializeAndAssign()
        {
            Profiler.BeginSample("DeserializeAndAssign");
            _projectImporter = GetProjectImporter();
            Profiler.EndSample();
            
            if (_projectImporter == null)
            {
                string levelName = _levelJson != null ? _levelJson.Identifier : "<Null>";
                Logger.LogError($"Tried to build level {levelName}, but the project importer was not found");
                return false;
            }

            Profiler.BeginSample("GetJsonData");
            _projectJson = GetProjectJsonData(_projectImporter);
            Profiler.EndSample();
            
            if (_projectJson == null)
            {
                Logger.LogError("Tried to build level, but the project json data was null");
                return false;
            }

            Profiler.BeginSample("AddLevelSubAsset");
            _levelFile = AddLevelSubAsset();
            Profiler.EndSample();
            
            if (_levelFile == null)
            {
                Logger.LogError("Tried to build level, but the level json ScriptableObject was null");
                return false;
            }

            try
            {
                _levelJson = FromJson<Level>();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return false;
            }

            return true;
        }

        private static LdtkJson GetProjectJsonData(LDtkProjectImporter importer)
        {
            if (importer == null)
            {
                return null;
            }
            
            if (Jsons.ContainsKey(importer))
            {
                //Debug.Log($"Project importer {importer.AssetName} deserialized already, skip");
                return Jsons[importer];
            }

            if (importer.JsonFile == null)
            {
                return null;
            }
            
            LdtkJson json = importer.FromJson<LdtkJson>();
            if (json == null)
            {
                return null;
            }
            
            if (Jsons.IsNullOrEmpty())
            {
                //Debug.Log("Added delayCall, this should only be called once per mass-reimport instance");
                EditorApplication.delayCall += Jsons.Clear;
            }
            
            //Debug.Log($"New project importer {importer.AssetName}, deserialize and cache");
            Jsons.Add(importer, json);
            
            return json;
        }

        private LDtkLevelFile AddLevelSubAsset()
        {
            LDtkLevelFile levelFile = ReadAssetText();
            ImportContext.AddObjectToAsset("levelFile", levelFile, LDtkIconUtility.LoadLevelIcon());
            return levelFile;
        }
    }
}