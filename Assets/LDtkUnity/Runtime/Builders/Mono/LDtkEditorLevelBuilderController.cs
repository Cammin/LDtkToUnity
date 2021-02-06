using LDtkUnity.BuildEvents;
using LDtkUnity.UnityAssets;
using UnityEngine;

namespace LDtkUnity.Builders
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    public class LDtkEditorLevelBuilderController : LDtkLevelBuilderController
    {
        private const string COMPONENT_NAME = "Editor Level Builder";
        public const string PREV_BUILT = nameof(_prevBuilt);
        
        [SerializeField] private GameObject _prevBuilt;

        public bool PrevExists => _prevBuilt != null;
        
        protected override bool DisposeDefinitionMemoryAfterBuilt => true;
        
        public void BuildLevels()
        {
            if (_prevBuilt != null)
            {
                DestroyImmediate(_prevBuilt);
            }
            
            _prevBuilt = BuildProject();
        }
    }
}