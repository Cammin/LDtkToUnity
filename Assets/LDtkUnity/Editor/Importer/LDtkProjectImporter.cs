using System.Collections.Generic;
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
    public class LDtkProjectImporter : LDtkJsonImporter<LdtkJson>
    {
        private const string EXTENSION = "ldtk";
        
        public const string LOG_BUILD_TIMES = nameof(_logBuildTimes);
        
        [SerializeField] private bool _logBuildTimes = false;
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            LdtkJson data = LoadJson(ctx);
            
            
        }
        
        protected GameObject BuildProject()
        {
            if (_projectAssets == null)
            {
                Debug.LogError("LDtk: Project Assets is null, is the project assets assigned?");
                return null;
            }
            
            LdtkJson project = _projectAssets.ProjectJson.FromJson;

            Assert.IsTrue(project.Levels.Length == _levelsToBuild.Length);

            Level[] levels = GetLevelsToBuild(project);
            LDtkProjectBuilder builder = new LDtkProjectBuilder(_projectAssets, project, levels);

            builder.BuildProject(_logBuildTimes);
            
            if (_useCustomSpawnPosition)
            {
                foreach (GameObject levelObject in builder.LevelObjects)
                {
                    levelObject.transform.position = _customSpawnPosition;
                    LDtkEditorUtil.Dirty(levelObject.transform);
                }
            }
            
            return builder.RootObject;
        }
        
        private Level[] GetLevelsToBuild(LdtkJson project)
        {
            List<Level> levels = new List<Level>();
            
            for (int i = 0; i < project.Levels.Length; i++)
            {
                Level projectLevel = project.Levels[i];
                Assert.IsNotNull(projectLevel);

                LDtkLevelFile file = _projectAssets.GetLevel(projectLevel.Identifier);
                if (file == null)
                {
                    continue;
                }
                
                Level fileLevel = file.FromJson;
                Assert.IsNotNull(fileLevel);

                if (projectLevel.Identifier != fileLevel.Identifier)
                {
                    Debug.LogError(
                        $"LDtk: Level file \"{fileLevel.Identifier}\" doesn't match up with \"{projectLevel.Identifier}\" from the project asset level");
                    continue;
                }

                bool buildLevel = _levelsToBuild[i];
                if (!buildLevel)
                {
                    continue;
                }
                
                levels.Add(fileLevel);
            }

            return levels.ToArray();
        }
        

        protected override LdtkJson LoadData(string json)
        {
            return LdtkJson.FromJson(json);
        }
    }
}
