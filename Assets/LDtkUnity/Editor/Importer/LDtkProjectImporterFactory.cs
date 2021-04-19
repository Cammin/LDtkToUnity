using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDtkUnity.Editor
{
    public class LDtkProjectImporterFactory
    {
        private readonly AssetImportContext _context;
        private readonly LDtkProjectFile _file;

        public LDtkProjectImporterFactory(AssetImportContext context, LDtkProjectFile file)
        {
            _context = context;
            _file = file;
        }

        public void Import()
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

            _context.AddObjectToAsset("rootGameObject", obj, LDtkIconLoader.LoadProjectFileIcon());
            _context.AddObjectToAsset("jsonFile", _file, LDtkIconLoader.LoadFavIcon());
        }

        private GameObject BuildProject(LdtkJson project)
        {
            return null;
            /*LDtkProjectBuilder builder = new LDtkProjectBuilder(_customBuildData, project);
            builder.BuildProject();
            return builder.RootObject;*/
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
                if (file == null)
                {
                    Debug.LogError("LDtk: Level file was null, ignored. May cause problems?");
                    continue;
                }
                
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