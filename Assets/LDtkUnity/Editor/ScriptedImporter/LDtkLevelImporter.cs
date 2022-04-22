using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

        protected override void Import()
        {
            if (_icon == null)
            {
                _icon = LDtkIconUtility.LoadLevelFileIcon();
            }
            
            //instead of grabbing the level from the project that built the level in that hierarchy, build the level from this json file directly to help individualize level building to only what's changed
            //that being said, the level importer still has important dependencies to the project importer like tile assets, entities, and any other artifacts.
            if (!InitFields())
            {
                return;
            }
            
            ImportContext.DependsOnSourceAsset(_projectImporter.assetPath);

            using (new LDtkUidBankScope(_projectJson))
            {
                BuildLevel();
            }
        }

        private void BuildLevel()
        {
            //my plan is to make this possible in a way where the level builder is solely meant to build levels. It should not have a dependency on the world.
            //also make the importer inspector have a toggle to turn off level building for that importer, so that only separate levels need to build when they really need to

            //project importer options for separate levels:
            //Build levels in project -> this is only available while separate levels are enabled. keep off to decrease import time. off by default
            //Levels WILL always build at their position. this is because the user can choose to post process that themself. Unless it's common enough...
            
            LDtkBuilderLevel levelBuilder = new LDtkBuilderLevel(_projectImporter, _projectJson, WorldLayout.Free, _levelJson);
            GameObject levelRoot = levelBuilder.BuildLevel();

            ImportContext.AddObjectToAsset("levelRoot", levelRoot, _icon);
            ImportContext.SetMainObject(levelRoot);
        }

        private bool InitFields()
        {
            _projectImporter = GetProjectImporter();
            if (_projectImporter == null)
            {
                LDtkDebug.LogError("The project importer was null");
                return false;
            }

            _projectJson = GetJsonData(_projectImporter);
            if (_projectJson == null)
            {
                LDtkDebug.LogError("The project json data was null");
                return false;
            }

            _levelFile = AddLevelSubAsset();
            if (_levelFile == null)
            {
                LDtkDebug.LogError("The level json ScriptableObject was null");
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
            LDtkProjectImporter projectImporter = getter.GetRelativeAsset(this, assetPath, (path) => (LDtkProjectImporter)GetAtPath(path));
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