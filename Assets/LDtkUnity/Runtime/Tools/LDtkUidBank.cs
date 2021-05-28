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
        private static Dictionary<long, ILDtkUid> Database { get; set; } = null;
        
        /// <summary>
        /// Call this when all definition data is no longer needed in memory.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void DisposeDefinitions()
        {
            Database = null;
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
            Database = new Dictionary<long, ILDtkUid>();

            Definitions defs = project.Defs;
            
            CacheUidData(defs.Layers);
            CacheEntityDefs(defs.Entities);
            
            CacheUidData(defs.Tilesets);
            CacheUidData(defs.Enums);
            CacheUidData(defs.ExternalEnums);
            
            CacheUidData(project.Levels);
            CacheUidData(defs.LevelFields);
        }

        private static void CacheEntityDefs(EntityDefinition[] entityDefs)
        {
            CacheUidData(entityDefs);
            
            FieldDefinition[] fieldDefs = entityDefs.SelectMany(entity => entity.FieldDefs).ToArray();
            CacheUidData(fieldDefs);
        }

        private static void CacheUidData<T>(IEnumerable<T> items) where T : ILDtkUid
        {
            foreach (T item in items)
            {
                if (Database.ContainsKey(item.Uid))
                {
                    Debug.LogError($"LDtk: UID database already has an int entry for {item.Uid}");
                    continue;
                }
                
                Database.Add(item.Uid, item);
            }
        }
        
        internal static T GetUidData<T>(long uid) where T : ILDtkUid
        {
            if (Database == null)
            {
                Debug.LogError($"LDtk: LDtkUidBank Dictionary<{typeof(T).Name}> is null; is the database not cached or already disposed?");
                return default;
            }

            if (Database.ContainsKey(uid))
            {
                return (T)Database[uid];
            }
            
            Debug.LogError($"LDtk: LDtkUidBank Dictionary<{typeof(T).Name}> does not contain a key UID for \"{uid}\"");
            return default;
        }
    }
}