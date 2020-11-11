using System.Linq;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets;
using UnityEngine;

namespace LDtkUnity.Runtime.Builders
{
    public class LDtkLevelBuilderController : MonoBehaviour
    {
        [SerializeField] private LDtkLevelIdentifier _levelToBuild = null;
        [SerializeField] private TextAsset _assetLDtkProject = null;

        private LDtkLevelBuilder _builder;

        private void Awake()
        {
            _builder = GetComponent<LDtkLevelBuilder>();
        }

        private void Start()
        {
            LDtkDataProject project = LDtkToolProjectLoader.LoadProject(_assetLDtkProject.text);
            LDtkDataLevel lvl = GetProjectLevelByID(project);
            
            _builder.BuildLevel(lvl);
        }

        private LDtkDataLevel GetProjectLevelByID(LDtkDataProject project)
        {
            LDtkDataLevel[] lvls = project.levels;

            if (_levelToBuild == null)
            {
                Debug.LogError($"LevelToBuild null");
                return default;
            }
            
            if (lvls.Any(p => p.identifier == _levelToBuild) == false)
            {
                Debug.LogError($"No level named \"{_levelToBuild}\" was available in the LDtk Project");
                return default;
            }

            LDtkDataLevel lvl = lvls.First(p => p.identifier == _levelToBuild);
            return lvl;
        }
    }
}