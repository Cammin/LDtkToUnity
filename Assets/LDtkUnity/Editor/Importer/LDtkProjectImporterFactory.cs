using System.Collections.Generic;
using LDtkUnity.Editor.Builders;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDtkUnity.Editor
{
    public class LDtkProjectImporterFactory
    {
        private readonly LDtkProjectImporter _importer;
        private readonly AssetImportContext _ctx;

        public LDtkProjectImporterFactory(LDtkProjectImporter importer, AssetImportContext ctx)
        {
            _importer = importer;
            _ctx = ctx;
        }

        public void Import()
        {
            LdtkJson jsonData = _importer.JsonFile.FromJson;
            if (jsonData == null)
            {
                _ctx.LogImportError("LDtk: Json import error");
                return;
            }

            Level[] levels = GetLevelsToBuild(jsonData);
            jsonData.Levels = levels;
            GameObject obj = BuildProject(jsonData);
            //GameObject obj = new GameObject(_file.name);
            
            LDtkComponentProjectFile lDtkComponentProjectFile = obj.AddComponent<LDtkComponentProjectFile>();
            lDtkComponentProjectFile.SetJson(jsonData);

            _ctx.AddObjectToAsset("rootGameObject", obj, LDtkIconLoader.LoadProjectFileIcon());
            _ctx.AddObjectToAsset("jsonFile", _importer, (Texture2D)LDtkIconLoader.GetUnityIcon("MetaFile"));
        }

        private GameObject BuildProject(LdtkJson project)
        {
            LDtkProjectBuilder builder = new LDtkProjectBuilder(_importer, project);
            builder.BuildProject();
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
            LDtkLevelFile[] levelFiles = finderLevels.GetRelativeAssets(projectLevels, _ctx.assetPath);

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
                _ctx.DependsOnArtifact(levelFilePath);
            }
            return levels.ToArray();
        }
    }
}