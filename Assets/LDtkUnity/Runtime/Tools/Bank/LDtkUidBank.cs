﻿using System;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Responsible for holding onto LDtk definitions so that they are easily accessible by instance classes.
    /// </summary>
    public static class LDtkUidBank
    {
        private static LDtkDictionaryUid _uids = null;

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
            LDtkProfiler.BeginSample("CacheUidData");
            _uids = new LDtkDictionaryUid(100);
            _uids.CacheAllData(project);
            LDtkProfiler.EndSample();
        }

        //todo while awaiting this fix, we can safely be silent for these cases. Once the bug is fixed, remove the silent param https://github.com/deepnight/ldtk/issues/1107
        internal static T GetUidData<T>(long uid, bool silent = false) where T : ILDtkUid
        {
            Type requestedType = typeof(T);
            
            if (_uids != null)
            {
                ILDtkUid tryGet = _uids.TryGet(uid, silent);
                if (tryGet != null)
                {
                    Type type = tryGet.GetType();
                    if (type != requestedType)
                    {
                        LDtkDebug.LogError($"{nameof(LDtkUidBank)} Dictionary<{requestedType.Name}> tried getting a type for {requestedType.Name} but it was {type.Name} instead. Is the LDtk json file outdated?");
                    }
                    
                    return (T)tryGet;
                }

                if (!silent)
                {
                    LDtkDebug.LogError($"{nameof(LDtkUidBank)} Dictionary<{requestedType.Name}>'s dictionary entry was null");
                }
                
                return default;
            }
            
            LDtkDebug.LogError($"{nameof(LDtkUidBank)} Dictionary<{requestedType.Name}> is null; is the database not cached or already released?");
            return default;
        }
    }
}