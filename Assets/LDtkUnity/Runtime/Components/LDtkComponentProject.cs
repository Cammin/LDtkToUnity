using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    /// <summary>
    /// A component available in the the import result's root GameObject. Reference this to access the json data.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_PROJECT)]
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + "Project Data")]
    public class LDtkComponentProject : MonoBehaviour
    {
        [ExcludeFromDocs] public const string PROP_PROJECT = nameof(_file);
                
        [SerializeField] private LDtkProjectFile _file;

        [ExcludeFromDocs] public void SetJson(LDtkProjectFile file)
        {
            _file = file;
        }

        /// <summary>
        /// Get a deserialized <see cref="LdtkJson"/> data class.
        /// </summary>
        /// <returns>
        /// A deserialized <see cref="LdtkJson"/> data class.
        /// </returns>
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
