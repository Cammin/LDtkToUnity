#pragma warning disable 0414
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// A component available in the the import result's root GameObject. Reference this to access the json data.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_PROJECT)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentProject : MonoBehaviour
    {
        internal const string PROPERTY_PROJECT = nameof(_file);
        internal const string PROPERTY_SEPARATE_LEVELS = nameof(_isSeparateLevels);
                
        [SerializeField] private LDtkProjectFile _file;
        
        [SerializeField] private bool _isSeparateLevels;
        internal void SetJson(LDtkProjectFile file)
        {
            _file = file;
        }
        
        internal void FlagAsSeparateLevels()
        {
            _isSeparateLevels = true;
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
                LDtkDebug.LogError("Json File is null");
                return default;
            }

            LdtkJson json = _file.FromJson;
            if (json != null)
            {
                return json;
            }
            
            LDtkDebug.LogError("Json File had a deserialization problem");
            return default;

        }
    }
}
