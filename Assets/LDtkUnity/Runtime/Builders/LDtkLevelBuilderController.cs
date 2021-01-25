using System;
using System.Diagnostics;
using System.Linq;
using LDtkUnity.BuildEvents;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Builders
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    [HelpURL(LDtkHelpURL.COMPONENT_LEVEL_BUILD_CONTROLLER)]
    public class LDtkLevelBuilderController : MonoBehaviour
    {
        private const string COMPONENT_NAME = "Level Builder Controller"; //todo this const alongside the others can belong in their own class instead, LDtkAddComponentMenu

        public const string PROP_PROJECT_ASSETS = nameof(_projectAssets);
        public const string PROP_BUILD_PREFERENCE = nameof(_buildPreference);
        public const string PROP_LEVELS_TO_BUILD = nameof(_levelsToBuild);

        [SerializeField] private LDtkProject _projectAssets = null;
        [SerializeField] private LDtkLevelBuilderControllerPreference _buildPreference = LDtkLevelBuilderControllerPreference.Single;
        [SerializeField] private LDtkLevelFile[] _levelsToBuild = null;
        
        //[SerializeField] private bool _disposeDefinitionMemoryAfterBuilt = true; //todo consider this for later
        
        private void Start()
        {
            LdtkJson project = _projectAssets.ProjectJson.FromJson;

            switch (_buildPreference)
            {
                case LDtkLevelBuilderControllerPreference.Single:
                case LDtkLevelBuilderControllerPreference.Partial:
                    
                    BuildLvls(project, _levelsToBuild.Select(p => p.FromJson).ToArray());
                    break;

                case LDtkLevelBuilderControllerPreference.All:
                    BuildLvls(project, _levelsToBuild.Select(p => p.FromJson).ToArray());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void BuildLvls(LdtkJson project, Level[] levels)
        {
            if (levels.NullOrEmpty())
            {
                Debug.LogError("LDtk: Level Builder Controller: No levels assigned.");
                return;
            }

            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            foreach (Level level in levels)
            {
                LDtkLevelBuilder.BuildLevel(_projectAssets, project, level);
            }
            
            levelBuildTimer.Stop();

            if (levels.Length > 1)
            {
                double ms = levelBuildTimer.ElapsedMilliseconds;
                Debug.Log($"LDtk: Built all levels in {ms}ms ({ms/1000}s)");
            }
        }
    }
}