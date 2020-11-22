using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets;
using LDtkUnity.Runtime.UnityAssets.Settings;
using UnityEngine;

namespace LDtkUnity.Runtime.Builders
{
    [HelpURL(LDtkHelpURL.LEVEL_BUILD_CONTROLLER)]
    public class LDtkLevelBuilderController : MonoBehaviour
    {
        [SerializeField] private LDtkLevelIdentifier _levelToBuild = null;
        [SerializeField] private TextAsset _assetLDtkProject = null;
        [SerializeField] private LDtkProjectAssets _projectAssets = null;
        
        private void Start()
        {
            LDtkDataProject project = LDtkToolProjectLoader.LoadProject(_assetLDtkProject.text);

            LDtkLevelBuilder.BuildLevel(project, _levelToBuild, _projectAssets);
        }
    }
}