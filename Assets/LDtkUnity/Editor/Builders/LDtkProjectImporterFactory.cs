using System.Collections.Generic;
using System.Linq;
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
            LdtkJson json = _importer.JsonFile.FromJson;
            if (json == null)
            {
                _ctx.LogImportError("LDtk: Json import error");
                return;
            }

            //set the data class's levels correctly, regardless if they are external levels or not
            json.Levels = GetLevelData(json);
            
            LDtkProjectBuilder builder = new LDtkProjectBuilder(_importer, json);
            builder.BuildProject();
            GameObject projectGameObject = builder.RootObject;
            
            _ctx.AddObjectToAsset("rootGameObject", projectGameObject, LDtkIconUtility.LoadProjectFileIcon());
            _ctx.AddObjectToAsset("jsonFile", _importer.JsonFile, (Texture2D)EditorGUIUtility.IconContent("ScriptableObject Icon").image);
            _ctx.AddObjectToAsset("artifacts", _importer.AutomaticallyGeneratedArtifacts, (Texture2D)LDtkIconUtility.GetUnityIcon("Tilemap"));
            
            _ctx.SetMainObject(projectGameObject);
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
            LDtkLevelFile[] levelFiles = projectLevels.Select(lvl => finderLevels.GetRelativeAsset(lvl, _ctx.assetPath)).ToArray();

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