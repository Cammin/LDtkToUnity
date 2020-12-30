using System;
using System.Linq;
using LDtkUnity.BuildEvents;
using LDtkUnity.Data;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;

namespace LDtkUnity.Builders
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    [HelpURL(LDtkHelpURL.COMPONENT_LEVEL_BUILD_CONTROLLER)]
    public class LDtkLevelBuilderController : MonoBehaviour
    {
        private const string COMPONENT_NAME = "Level Builder Controller";

        public const string PROP_PROJECT_ASSETS = nameof(_projectAssets);
        public const string PROP_BUILD_PREFERENCE = nameof(_buildPreference);
        public const string PROP_LEVELS_TO_BUILD = nameof(_levelsToBuild);

        [SerializeField] private LDtkProject _projectAssets = null;
        [SerializeField] private LDtkLevelBuilderControllerPreference _buildPreference = LDtkLevelBuilderControllerPreference.Single;
        [SerializeField] private LDtkLevelIdentifier[] _levelsToBuild = null;
        
        //[SerializeField] private bool _disposeDefinitionMemoryAfterBuilt = true; //todo consider this for later
        
        private void Start()
        {
            LDtkDataProject project = _projectAssets.GetDeserializedProject();

            switch (_buildPreference)
            {
                case LDtkLevelBuilderControllerPreference.Single:
                case LDtkLevelBuilderControllerPreference.Partial:
                    BuildLvls(_levelsToBuild.Select(p => p.name).ToArray());
                    break;

                case LDtkLevelBuilderControllerPreference.All:
                    BuildLvls(project.levels.Select(p => p.identifier).ToArray());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            void BuildLvls(string[] levels)
            {
                if (levels.NullOrEmpty())
                {
                    Debug.LogError("LDtk: Level Builder Controller: No levels assigned.");
                    return;
                }
                
                foreach (string levelToBuild in levels)
                {
                    LDtkLevelBuilder.BuildLevel(_projectAssets, project, levelToBuild);
                }
            }
        }
    }
}