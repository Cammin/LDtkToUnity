using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Responsible for holding onto LDtk iid references so that they are easily accessible by instance classes.
    /// </summary>
    public static class LDtkIidBank
    {
        private static LDtkDictionaryIid _iids = null;

        /// <summary>
        /// Call this when all iid data is no longer needed in memory.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Release()
        {
            _iids = null;
        }

        /// <summary>
        /// Call this to statically load all iid data. This is automatic during the import process, but call this if accessing iid data is required in runtime or otherwise.<br/>
        /// The <see cref="EntityReferenceInfos"/> class has iid properties, so call this before trying to access them.
        /// </summary>
        /// <param name="project">
        /// The json project to cache the iid data of.
        /// </param>
        public static void CacheIidData(LdtkJson project)
        {
            _iids = new LDtkDictionaryIid();
            _iids.CacheAllData(project);
        }

        internal static T GetIidData<T>(string iid) where T : ILDtkIid
        {
            if (_iids != null)
            {
                return (T)_iids.TryGet(iid);
            }
            
            Debug.LogError($"LDtk: {nameof(LDtkIidBank)} Dictionary<{typeof(T).Name}> is null; is the database not cached or already released?");
            return default;
        }
    }
}