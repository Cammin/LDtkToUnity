using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Assertions;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.JSON_PROJECT)]
    [ScriptedImporter(1, EXTENSION)]
    public class LDtkProjectImporter : LDtkJsonImporter<LDtkProjectFile>
    {
        private const string EXTENSION = "ldtk";
        
        public const string LOG_BUILD_TIMES = nameof(_logBuildTimes);
        public const string LEVELS_TO_BUILD = nameof(_levelsToBuild);
        
        
        [SerializeField] private bool _logBuildTimes = false;
        [SerializeField] private bool[] _levelsToBuild = {true};

        private AssetImportContext _context;
        private LDtkProjectFile _file;
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            _context = ctx;
            _file = ReadAssetText(ctx);
            Import();
        }

        private void Import()
        {
            LdtkJson jsonData = _file.FromJson;
            if (jsonData == null)
            {
                _context.LogImportError("LDtk: Json import error");
                return;
            }

            Level[] levels = GetLevelsToBuild(jsonData);
            jsonData.Levels = levels;
            //GameObject obj = BuildProject(jsonData);
            GameObject obj = new GameObject(_file.name);
            
            LDtkComponentProjectFile lDtkComponentProjectFile = obj.AddComponent<LDtkComponentProjectFile>();
            lDtkComponentProjectFile.SetJson(jsonData);

            _context.AddObjectToAsset("rootGameObject", obj, LDtkIconLoader.LoadFavIcon());
            _context.AddObjectToAsset("jsonFile", _file, LDtkIconLoader.LoadProjectFileIcon());
        }

        private GameObject BuildProject(LdtkJson project)
        {
            LDtkProjectBuilder builder = new LDtkProjectBuilder(null, project);
            builder.BuildProject(_logBuildTimes);
            return builder.RootObject;
        }
        
        /// <summary>
        /// returns the jsons of the level, based on whether we specify external levels.
        /// </summary>
        private Level[] GetLevelsToBuild(LdtkJson project)
        {
            return project.ExternalLevels ? GetExternalLevels(project.Levels) : project.Levels;
        }

        private Level[] GetExternalLevels(Level[] projectLevels)
        {
            List<Level> levels = new List<Level>();
            
            LDtkRelativeAssetFinderLevels finderLevels = new LDtkRelativeAssetFinderLevels();
            LDtkLevelFile[] levelFiles = finderLevels.GetRelativeAssets(projectLevels, _context.assetPath);

            foreach (LDtkLevelFile file in levelFiles)
            {
                Level level = file.FromJson;
                Assert.IsNotNull(level);
                
                levels.Add(level);

                //add dependency so that we trigger a reimport if we reimport a level
                string levelFilePath = AssetDatabase.GetAssetPath(file);
                _context.DependsOnArtifact(levelFilePath);
            }
            return levels.ToArray();
        }
    }
}
