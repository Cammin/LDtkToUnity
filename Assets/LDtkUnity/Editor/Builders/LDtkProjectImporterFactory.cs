using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDtkUnity.Editor
{
    internal class LDtkProjectImporterFactory
    {
        private readonly LDtkProjectImporter _importer;

        public LDtkProjectImporterFactory(LDtkProjectImporter importer)
        {
            _importer = importer;
        }

        public void Import(LdtkJson json)
        {
            //set the data class's levels correctly, regardless if they are external levels or not
            json.Levels = GetLevelData(json);
            
            
            LDtkProjectBuilder builder = new LDtkProjectBuilder(_importer, json);
            builder.BuildProject();
            GameObject projectGameObject = builder.RootObject;

            if (projectGameObject == null)
            {
                _importer.ImportContext.LogImportError("LDtk: Project GameObject null, not building correctly");
                return;
            }
            
            _importer.ImportContext.AddObjectToAsset("rootGameObject", projectGameObject, LDtkIconUtility.LoadProjectFileIcon());
            _importer.ImportContext.SetMainObject(projectGameObject);
        }
        
        
        /// <summary>
        /// returns the jsons of the level, based on whether we specify external levels.
        /// </summary>
        private Level[] GetLevelData(LdtkJson project)
        {
            if (!project.ExternalLevels)
            {
                return project.Levels;
            }
            
            //if we are external levels, we wanna modify the json and serialize it back to that it's usable later with it's completeness regardless of external levels
            Level[] externalLevels = GetExternalLevels(project.Levels);
            project.Levels = externalLevels;

            string newJson = "";
            try
            {
                newJson = project.ToJson();
            }
            finally
            {
                _importer.JsonFile.SetJson(newJson);
            }
            
            return project.Levels;
        }

        private Level[] GetExternalLevels(Level[] projectLevels)
        {
            List<Level> levels = new List<Level>();
            LDtkRelativeGetterLevels finderLevels = new LDtkRelativeGetterLevels();
            
            string[] levelFiles = projectLevels.Select(lvl => finderLevels.ReadRelativeText(lvl, _importer.ImportContext.assetPath)).ToArray();
            //todo test what might happen when we try broken file paths

            foreach (string json in levelFiles)
            {
                if (json == null)
                {
                    Debug.LogError("LDtk: Level file was null, ignored. May cause problems?", _importer);
                    continue;
                }
                
                Level level = Level.FromJson(json);
                Assert.IsNotNull(level);
                
                levels.Add(level);

                //add dependency so that we trigger a reimport if we reimport a level due to it being saved
                //_importer.SetupAssetDependency(json); //todo verify if we need to disable a dependency here if they are external levels.
            }
            return levels.ToArray();
        }
    }
}