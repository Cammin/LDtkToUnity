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

            bool success = GetProjectLevelByID(project.levels, out LDtkDataLevel lvl);
            if (!success) return;
            
            LDtkUidDatabase.CacheUidData(project);
            _builder.BuildLevel(lvl);
            LDtkUidDatabase.Dispose();
        }

        private bool GetProjectLevelByID(LDtkDataLevel[] lvls, out LDtkDataLevel lvl)
        {
            lvl = default;

            if (_levelToBuild == null)
            {
                Debug.LogError($"LevelToBuild null, not assigned?");
                return false;
            }

            bool IdentifierMatchesLevelToBuild(LDtkDataLevel l) => string.Equals(l.identifier, _levelToBuild);
            
            if (!lvls.Any(IdentifierMatchesLevelToBuild))
            {
                Debug.LogError($"No level named \"{_levelToBuild}\" exists in the LDtk Project");
                return false;
            }

            lvl = lvls.First(IdentifierMatchesLevelToBuild);
            return true;
        }
    }
}