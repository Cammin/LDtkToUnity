using System.Collections.Generic;
using System.Diagnostics;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Builders
{
    public abstract class LDtkLevelBuilderController : MonoBehaviour
    {
        public const string PROJECT_ASSETS = nameof(_projectAssets);
        public const string LEVELS_TO_BUILD = nameof(_levelsToBuild);

        [SerializeField] private LDtkProject _projectAssets = null;
        [SerializeField] private bool[] _levelsToBuild = {true};

        protected GameObject BuildProject()
        {
            if (_projectAssets == null)
            {
                Debug.LogError("LDtk: Project Assets is null, is the project assets assigned?");
                return null;
            }
            
            LdtkJson project = _projectAssets.ProjectJson.FromJson;

            Assert.IsTrue(project.Levels.Length == _levelsToBuild.Length);

            LDtkProjectBuilder builder = new LDtkProjectBuilder(_projectAssets, project, GetLevelsToBuild(project));
            return builder.BuildProject();
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
                        $"Level file \"{fileLevel.Identifier}\" doesn't match up with \"{projectLevel.Identifier}\" from the project asset level");
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
    }
}