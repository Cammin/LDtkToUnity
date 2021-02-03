using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LDtkUnity.BuildEvents;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Builders
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    [HelpURL(LDtkHelpURL.COMPONENT_LEVEL_BUILD_CONTROLLER)]
    public class LDtkLevelBuilderController : MonoBehaviour
    {
        private const string COMPONENT_NAME = "Level Builder Controller"; //todo this const alongside the others can belong in their own class instead, LDtkAddComponentMenu

        public const string PROP_PROJECT_ASSETS = nameof(_projectAssets);
        public const string PROP_LEVELS_TO_BUILD = nameof(_levelsToBuild);

        [SerializeField] private LDtkProject _projectAssets = null;
        [SerializeField] private bool[] _levelsToBuild = {true};
        
        //[SerializeField] private bool _disposeDefinitionMemoryAfterBuilt = true; //todo consider this for later
        
        private void Start()
        {
            BuildProject();
        }

        public void BuildProject()
        {
            if (_projectAssets == null)
            {
                Debug.LogError("LDtk: Project Assets is null! Is the project assets assigned?");
                return;
            }
            
            LdtkJson project = _projectAssets.ProjectJson.FromJson;

            Assert.IsTrue(project.Levels.Length == _levelsToBuild.Length);
            
            BuildLvls(project, GetLevelsToBuild(project));
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
        
        private void BuildLvls(LdtkJson project, Level[] fileLevels)
        {
            if (fileLevels.NullOrEmpty())
            {
                Debug.LogError("LDtk: Level Builder Controller: No levels specified.");
                return;
            }

            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            foreach (Level fileLevel in fileLevels)
            {
                LDtkLevelBuilder.BuildLevel(_projectAssets, project, fileLevel);
            }

            levelBuildTimer.Stop();

            if (fileLevels.Length > 1)
            {
                double ms = levelBuildTimer.ElapsedMilliseconds;
                Debug.Log($"LDtk: Built all levels in {ms}ms ({ms/1000}s)");
            }
        }
    }
}