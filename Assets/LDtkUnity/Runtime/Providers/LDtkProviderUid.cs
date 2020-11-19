using System.Collections.Generic;
using System.Linq;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Definition;
using UnityEngine;

namespace LDtkUnity.Runtime.Providers
{
    public static class LDtkProviderUid
    {
        private static Dictionary<int, ILDtkUid> Database { get; set; } = null;
        
#if UNITY_2019_2_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void Dispose()
        {
            Database = null;
        }

        public static void CacheUidData(LDtkDataProject project)
        {
            Database = new Dictionary<int, ILDtkUid>();
            
            CacheLayerDefs(project.defs.layers);
            CacheEntityDefs(project.defs.entities);
            
            CacheUidData(project.defs.tilesets);
            CacheUidData(project.defs.enums);
            CacheUidData(project.defs.externalEnums);
            
            CacheUidData(project.levels);
        }

        private static void CacheLayerDefs(LDtkDefinitionLayer[] layerDefs)
        {
            CacheUidData(layerDefs);
            
            LDtkDefinitionLayerAutoRuleGroup[] autoRuleGroupDefs = layerDefs.SelectMany(layer => layer.autoRuleGroups).ToArray();
            CacheUidData(autoRuleGroupDefs);
            
            LDtkDefinitionAutoLayerRule[] autoRuleDefs = autoRuleGroupDefs.SelectMany(groupDef => groupDef.rules).ToArray();
            CacheUidData(autoRuleDefs);
        }
        
        private static void CacheEntityDefs(LDtkDefinitionEntity[] entityDefs)
        {
            CacheUidData(entityDefs);
            
            LDtkDefinitionField[] fieldDefs = entityDefs.SelectMany(entity => entity.fieldDefs).ToArray();
            CacheUidData(fieldDefs);
        }

        private static void CacheUidData<T>(IEnumerable<T> items) where T : ILDtkUid
        {
            foreach (T item in items)
            {
                if (Database.ContainsKey(item.uid))
                {
                    Debug.LogError($"LDtk: UID database already has an int entry for {item.uid}");
                    continue;
                }
                
                Database.Add(item.uid, item);
            }
        }
        
        public static T GetUidData<T>(int uid) where T : ILDtkUid
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