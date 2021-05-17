using UnityEngine;

namespace LDtkUnity
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + "Project Data")]
    public class LDtkComponentProject : MonoBehaviour
    {
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
