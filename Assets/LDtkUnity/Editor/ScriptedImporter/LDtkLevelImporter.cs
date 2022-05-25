using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.IMPORTER_LDTK_LEVEL)]
    [ScriptedImporter(LDtkImporterConsts.LEVEL_VERSION, LDtkImporterConsts.LEVEL_EXT, LDtkImporterConsts.LEVEL_ORDER)]
    internal class LDtkLevelImporter : LDtkJsonImporter<LDtkLevelFile>
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
            _previousDependencies = LDtkLevelDependencyFactory.GatherLevelDependencies(path);
            return _previousDependencies;
        }

        protected override string[] GetGatheredDependencies() => _previousDependencies;

        protected override void Import()
        {
            if (IsBackupFile())
            {
                return;
            }
            
            if (_icon == null)
            {
                _icon = LDtkIconUtility.LoadLevelFileIcon();
            }
            
            //instead of grabbing the level from the project that built the level in that hierarchy, build the level from this json file directly to help individualize level building to only what's changed
            //that being said, the level importer still has important dependencies to the project importer like tile assets, entities, and any other artifacts.
            if (!DeserializeAndAssign())
            {
                return;
            }

            //Dependencies.AddDependency(_projectImporter.assetPath);

            using (new LDtkUidBankScope(_projectJson))
            {
                BuildLevel();
            }
        }

        private void BuildLevel()
        {
            LDtkPostProcessorCache postProcess = new LDtkPostProcessorCache();

            LDtkBuilderLevel levelBuilder = new LDtkBuilderLevel(_projectImporter, _projectJson, WorldLayout.Free, _levelJson, postProcess, Dependencies);
            GameObject levelRoot = levelBuilder.BuildLevel();
            postProcess.PostProcess();

            ImportContext.AddObjectToAsset("levelRoot", levelRoot, _icon);
            ImportContext.SetMainObject(levelRoot);
        }

        private bool DeserializeAndAssign()
        {
            _projectImporter = GetProjectImporter();
            if (_projectImporter == null)
            {
                string levelName = _levelJson != null ? _levelJson.Identifier : "<Null>";
                LDtkDebug.LogError($"Tried to build level {levelName}, but the project importer was not found");
                return false;
            }

            _projectJson = GetJsonData(_projectImporter);
            if (_projectJson == null)
            {
                LDtkDebug.LogError("Tried to build level, but the project json data was null");
                return false;
            }

            _levelFile = AddLevelSubAsset();
            if (_levelFile == null)
            {
                LDtkDebug.LogError("Tried to build level, but the level json ScriptableObject was null");
                return false;
            }

            try
            {
                _levelJson = _levelFile.FromJson;
            }
            catch (Exception e)
            {
                LDtkDebug.LogError(e.ToString());
                return false;
            }

            return true;
        }

        private static LdtkJson GetJsonData(LDtkProjectImporter importer)
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
            
            LdtkJson json = importer.JsonFile.FromJson;
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
            importer.CacheTempSubAsset();
            Jsons.Add(importer, json);
            
            return json;
        }

        public LDtkProjectImporter GetProjectImporter()
        {
            LDtkRelativeGetterProjectImporter getter = new LDtkRelativeGetterProjectImporter();
            LDtkProjectImporter projectImporter = getter.GetRelativeAsset(assetPath, assetPath, (path) => (LDtkProjectImporter)GetAtPath(path));
            return projectImporter;
        }

        private LDtkLevelFile AddLevelSubAsset()
        {
            LDtkLevelFile levelFile = ReadAssetText();
            ImportContext.AddObjectToAsset("levelFile", levelFile, LDtkIconUtility.LoadLevelIcon());
            return levelFile;
        }
    }
}