using LDtkUnity.Data;
using LDtkUnity.UnityAssets;
using UnityEngine;

namespace LDtkUnity.Builders
{
    [HelpURL(LDtkHelpURL.LEVEL_BUILD_CONTROLLER)]
    public class LDtkLevelBuilderController : MonoBehaviour
    {
        [SerializeField] private LDtkLevelIdentifier _levelToBuild = null;
        [SerializeField] private LDtkProject _projectAssets = null;
        
        private void Start()
        {
            LDtkDataProject project = _projectAssets.GetDeserializedProject();
            LDtkLevelBuilder.BuildLevel(project, _levelToBuild, _projectAssets);
        }
    }
}