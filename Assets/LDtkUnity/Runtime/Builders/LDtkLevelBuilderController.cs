using LDtkUnity.BuildEvents;
using LDtkUnity.Data;
using LDtkUnity.UnityAssets;
using UnityEngine;

namespace LDtkUnity.Builders
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    [HelpURL(LDtkHelpURL.COMPONENT_LEVEL_BUILD_CONTROLLER)]
    public class LDtkLevelBuilderController : MonoBehaviour
    {
        private const string COMPONENT_NAME = "Level Builder Controller";
        
        [SerializeField] private LDtkLevelIdentifier _levelToBuild = null;
        [SerializeField] private LDtkProject _projectAssets = null;
        [SerializeField] private bool _disposeDefinitionMemoryAfterBuilt = true;
        
        private void Start()
        {
            LDtkDataProject project = _projectAssets.GetDeserializedProject();
            LDtkLevelBuilder.BuildLevel(project, _levelToBuild, _projectAssets, _disposeDefinitionMemoryAfterBuilt);
        }
    }
}