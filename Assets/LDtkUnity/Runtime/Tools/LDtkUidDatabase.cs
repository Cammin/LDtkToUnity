using System.Collections.Generic;
using System.Linq;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkUidDatabase
    {
        private static IReadOnlyDictionary<int, LDtkDefinitionLayer> Layers { get; set; } //layer def
        private static IReadOnlyDictionary<int, LDtkDefinitionLayerAutoRuleGroup> AutoRuleGroups { get; set; }
        private static IReadOnlyDictionary<int, LDtkDefinitionAutoLayerRule> AutoRules { get; set; }
        
        private static IReadOnlyDictionary<int, LDtkDefinitionEntity> Entities { get; set; } //entity def
        private static IReadOnlyDictionary<int, LDtkDefinitionField> Fields { get; set; }
        
        private static IReadOnlyDictionary<int, LDtkDefinitionTileset> Tilesets { get; set; } //tilesets def
        
        private static IReadOnlyDictionary<int, LDtkDefinitionEnum> Enums { get; set; } //enums def
        
        private static IReadOnlyDictionary<int, LDtkDataLevel> Levels { get; set; } //levels
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Dispose()
        {
            Layers = null;
            AutoRuleGroups = null;
            AutoRules = null;
            Entities = null;
            Fields = null;
            Tilesets = null;
            Enums = null;
            Levels = null;
        }
        
        #region Caching
        public static void CacheUidData(LDtkDataProject project)
        {
            CacheDefinitions(project.defs);
            CacheLevels(project.levels);
        }

        private static void CacheDefinitions(LDtkDefinitions defs)
        {
            CacheLayerDefs(defs.layers);
            CacheEntityDefs(defs.entities);
            CacheTilesetDefs(defs.tilesets);
            CacheEnumDefs(defs.enums, defs.externalEnums);
        }

        private static void CacheLayerDefs(LDtkDefinitionLayer[] layerDefs)
        {
            Layers = DictionaryFromData(layerDefs, layer => layer.uid);
            
            LDtkDefinitionLayerAutoRuleGroup[] autoRuleGroupDefs = layerDefs.SelectMany(layer => layer.autoRuleGroups).ToArray();
            AutoRuleGroups = DictionaryFromData(autoRuleGroupDefs, ruleGroup => ruleGroup.uid);
            
            LDtkDefinitionAutoLayerRule[] autoRuleDefs = autoRuleGroupDefs.SelectMany(groupDef => groupDef.rules).ToArray();
            AutoRules = DictionaryFromData(autoRuleDefs, autoRuleDef => autoRuleDef.uid);
        }
        
        private static void CacheEntityDefs(LDtkDefinitionEntity[] entityDefs)
        {
            Entities = DictionaryFromData(entityDefs, entity => entity.uid);
            
            LDtkDefinitionField[] fieldDefs = entityDefs.SelectMany(entity => entity.fieldDefs).ToArray();
            Fields = DictionaryFromData(fieldDefs, field => field.uid);
        }
        
        private static void CacheTilesetDefs(LDtkDefinitionTileset[] tilesetDefs)
        {
            Tilesets = DictionaryFromData(tilesetDefs, tileset => tileset.uid);
        }
        
        private static void CacheEnumDefs(LDtkDefinitionEnum[] enumDefs, LDtkDefinitionEnum[] externalEnumDefs)
        {
            Enums = MergeDictionary
            (
                DictionaryFromData(enumDefs, enumDef => enumDef.uid),
                DictionaryFromData(externalEnumDefs, externalEnumDef => externalEnumDef.uid)
            ); 
        }
        
        private static void CacheLevels(LDtkDataLevel[] levels)
        {
            Levels = DictionaryFromData(levels, level => level.uid);
        }

        private delegate int UidGetter<in T>(T item);
        private static Dictionary<int, T> DictionaryFromData<T>(IEnumerable<T> items, UidGetter<T> getter) where T : struct
        {
            return items.ToDictionary(getter.Invoke);
        }
        private static Dictionary<int, T> MergeDictionary<T>(Dictionary<int, T> a, Dictionary<int, T> b)
        {
            return a.Concat(b).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        #endregion

        #region Gets
        public static LDtkDefinitionLayer GetLayerDefinition(int uid) => GetDefInternal(Layers, uid);
        public static LDtkDefinitionLayerAutoRuleGroup GetAutoRuleGroupDefinition(int uid) => GetDefInternal(AutoRuleGroups, uid);
        public static LDtkDefinitionAutoLayerRule GetAutoLayerRuleDefinition(int uid) => GetDefInternal(AutoRules, uid);
        public static LDtkDefinitionEntity GetEntityDefinition(int uid) => GetDefInternal(Entities, uid);
        public static LDtkDefinitionField GetFieldDefinition(int uid) => GetDefInternal(Fields, uid);
        public static LDtkDefinitionTileset GetTilesetDefinition(int uid) => GetDefInternal(Tilesets, uid);
        public static LDtkDefinitionEnum GetEnumDefinition(int uid) => GetDefInternal(Enums, uid);
        public static LDtkDataLevel GetLevelData(int uid) => GetDefInternal(Levels, uid);
        
        private static T GetDefInternal<T>(IReadOnlyDictionary<int, T> dict, int uid) where T : struct
        {
            if (dict == null)
            {
                Debug.LogError($"LDtk: DefinitionDatabase Dictionary<{typeof(T).Name}> is null; is the database not cached or already disposed?");
                return default;
            }

            if (dict.ContainsKey(uid))
            {
                return dict[uid];
            }
            
            Debug.LogError($"LDtk: DefinitionDatabase Dictionary<{typeof(T).Name}> does not contain a key for {uid}");
            return default;
        }
        #endregion
        

    }
}