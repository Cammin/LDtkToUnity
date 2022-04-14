using System.Linq;
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
    internal class LDtkLevelImporter : LDtkJsonImporter<LDtkLevelFile>
    {
        protected override void Import()
        {
            //instead of grabbing the level from the project, build the level from this json file directly to help individualize level building to only what's changed
            
            LDtkProjectImporter projectImporter = GetProjectAsset();
            ImportContext.DependsOnSourceAsset(projectImporter.assetPath);
            
            LDtkLevelFile levelFile = AddLevelFile();
            Level level = levelFile.FromJson;

            //my plan is to make this possible in a way where the level builder is solely meant to build levels. It should not have a dependency on the world.
            LdtkJson json = projectImporter.JsonFile.FromJson;
            
            LDtkUidBank.CacheUidData(json);
            LDtkPostProcessorCache.Initialize();
            
            World world = json.DeprecatedWorld; //todo this is a hack
            LDtkLinearLevelVector v = new LDtkLinearLevelVector();
            LDtkBuilderLevel levelBuilder = new LDtkBuilderLevel(projectImporter, json, json.DeprecatedWorld, level, v);
            GameObject newLevelObj = levelBuilder.BuildLevel();
            

            ImportContext.AddObjectToAsset("levelRoot", newLevelObj, LDtkIconUtility.LoadLevelFileIcon());
            ImportContext.SetMainObject(newLevelObj);
            
            LDtkUidBank.ReleaseDefinitions();
        }
        
        public LDtkProjectImporter GetProjectAsset()
        {
            LDtkRelativeGetterProjectImporter getter = new LDtkRelativeGetterProjectImporter();
            LDtkProjectImporter projectImporter = getter.GetRelativeAsset(this, assetPath, (path) => (LDtkProjectImporter)GetAtPath(path));
            return projectImporter;
        }

        private LDtkLevelFile AddLevelFile()
        {
            LDtkLevelFile levelFile = ReadAssetText();
            ImportContext.AddObjectToAsset("levelFile", levelFile, LDtkIconUtility.LoadLevelIcon());
            return levelFile;
        }
    }
}