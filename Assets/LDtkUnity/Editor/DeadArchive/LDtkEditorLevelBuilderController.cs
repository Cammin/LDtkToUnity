#pragma warning disable 0414

namespace LDtkUnity.Editor
{
    /*[AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    [HelpURL(LDtkHelpURL.COMPONENT_LEVEL_BUILDER)]
    public class LDtkEditorLevelBuilderController : LDtkLevelBuilderController
    {
        private const string COMPONENT_NAME = "Editor Level Builder";
        public const string PREV_BUILT = nameof(_prevBuilt);
        
        [SerializeField] private GameObject _prevBuilt = null;

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
    }*/
}