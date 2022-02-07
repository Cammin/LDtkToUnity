using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Responsible for holding onto LDtk definitions so that they are easily accessible by instance classes.
    /// </summary>
    public static class LDtkUidBank
    {
        private static LDtkDictionaryUId _uids = null;

        /// <summary>
        /// Call this when all definition data is no longer needed in memory.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void ReleaseDefinitions()
        {
            _uids = null;
        }

        /// <summary>
        /// Call this to statically load all definition data. This is automatic during the import process, but call this if accessing definitions is required in runtime or otherwise.<br/>
        /// Most LDtk json instances have a definition property, so call this before trying to access definitions.
        /// </summary>
        /// <param name="project">
        /// The json project to cache the definitions of.
        /// </param>
        public static void CacheUidData(LdtkJson project)
        {
            _uids = new LDtkDictionaryUId();
            _uids.CacheAllData(project);
        }

        internal static T GetUidData<T>(long uid) where T : ILDtkUid
        {
            if (_uids != null)
            {
                return (T)_uids.TryGet(uid);
            }
            
            Debug.LogError($"LDtk: {nameof(LDtkUidBank)} Dictionary<{typeof(T).Name}> is null; is the database not cached or already released?");
            return default;
        }
    }
}