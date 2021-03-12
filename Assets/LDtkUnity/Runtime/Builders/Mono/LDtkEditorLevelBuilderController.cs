using UnityEngine;

namespace LDtkUnity
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    public class LDtkEditorLevelBuilderController : LDtkLevelBuilderController
    {
        private const string COMPONENT_NAME = "Editor Level Builder";
        public const string PREV_BUILT = nameof(_prevBuilt);
        
        [SerializeField] private GameObject _prevBuilt;

        public bool PrevExists => _prevBuilt != null;

        public GameObject BuildLevels()
        {
            if (_prevBuilt != null)
            {
                DestroyImmediate(_prevBuilt);
            }
            
            _prevBuilt = BuildProject();

            return _prevBuilt;
        }
    }
}