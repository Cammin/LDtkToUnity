using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// A component available in the the import result's root GameObject. Reference this to access the json data.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_PROJECT)]
    [AddComponentMenu("")]
    public class LDtkComponentProject : MonoBehaviour
    {
        internal const string PROPERTY_PROJECT = nameof(_file);
                
        [SerializeField] private LDtkProjectFile _file;

        internal void SetJson(LDtkProjectFile file)
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
