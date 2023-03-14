using UnityEngine;
using UnityEngine.Profiling;

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
        /// Call this to statically load all iid json data. This is automatic during the import process, but call this if accessing iid data is required in runtime or otherwise.<br/>
        /// The <see cref="LDtkReferenceToAnEntityInstance"/> class has iid properties, so call this before trying to access them.
        /// </summary>
        /// <param name="project">
        /// The json project to cache the iid data of.
        /// </param>
        /// <param name="separateLevel">
        /// The optional level. If the project is using separate level files, then it's not possible to access iids from any other levels.
        /// </param>
        public static void CacheIidData(LdtkJson project, Level separateLevel = null)
        {
            Profiler.BeginSample("CacheIidData");
            _iids = new LDtkDictionaryIid();
            _iids.CacheAllData(project, separateLevel);
            Profiler.EndSample();
        }

        internal static T GetIidData<T>(string iid) where T : ILDtkIid
        {
            if (_iids != null)
            {
                return (T)_iids.TryGet(iid);
            }
            
            LDtkDebug.LogError($"{nameof(LDtkIidBank)} Dictionary<{typeof(T).Name}> is null; is the database not cached or already released?");
            return default;
        }
    }
}