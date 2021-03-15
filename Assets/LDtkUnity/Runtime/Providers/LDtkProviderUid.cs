using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Responsible for holding onto definitions during the build process. The data is disposed of after the build is done.
    /// </summary>
    public static class LDtkProviderUid
    {
        private static Dictionary<long, ILDtkUid> Database { get; set; } = null;
        
        /// <summary>
        /// Call this when Definition data is no longer needed.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Dispose()
        {
            Database = null;
        }

        /// <summary>
        /// Call this when definition data is in need of being accessed via the Json Data's extended properties.
        /// </summary>
        public static void CacheUidData(LdtkJson project)
        {
            Database = new Dictionary<long, ILDtkUid>();
            
            CacheLayerDefs(project.Defs.Layers);
            CacheEntityDefs(project.Defs.Entities);
            
            CacheUidData(project.Defs.Tilesets);
            CacheUidData(project.Defs.Enums);
            CacheUidData(project.Defs.ExternalEnums);
            
            CacheUidData(project.Levels);
        }

        private static void CacheLayerDefs(LayerDefinition[] layerDefs)
        {
            CacheUidData(layerDefs);
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
                Debug.LogError($"LDtk: DefinitionDatabase Dictionary<{typeof(T).Name}> is null; is the database not cached or already disposed?");
                return default;
            }

            if (Database.ContainsKey(uid))
            {
                return (T)Database[uid];
            }
            
            Debug.LogError($"LDtk: DefinitionDatabase Dictionary<{typeof(T).Name}> does not contain a key for {uid}");
            return default;
        }
    }
}