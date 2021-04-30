using UnityEngine;

namespace LDtkUnity
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    public class LDtkComponentProject : MonoBehaviour
    {
        private const string COMPONENT_NAME = "Project Data";
        public const string PROP_PROJECT = nameof(_file);
                
        [SerializeField] private LDtkProjectFile _file;

        public void SetJson(LDtkProjectFile file)
        {
            _file = file;
        }

        public LdtkJson FromJson()
        {
            if (_file == null)
            {
                Debug.LogError("LDtk: Json File is null");
                return default;
            }

            LdtkJson json = _file.FromJson;
            if (json != null)
            {
                return json;
            }
            
            Debug.LogError("LDtk: Json File had a deserialization problem");
            return default;

        }
    }
}
