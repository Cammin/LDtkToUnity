using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity
{
    /// <summary>
    /// Holds onto all scriptable objects and attempts to link everything by their uids.
    /// 
    /// Rules for defining definition objects:
    /// - Have all schema fields exist except for deprecated ones.
    /// 
    /// - Do a #region for the internal values only to the LDtk editor.
    /// To have a inspector header, only make the first field encountered be private serialized field with a public getter property after.
    /// All others will do the [field: SerializeField] with a { get; private set; }
    /// 
    /// - All uid refs should translate into the appropriate scriptable object by relaying in a second pass over all def objects.
    /// - All strings/ints/floats that might represent a color or Vector2 can be turned into the appropriate structs.
    /// </summary>
    public class LDtkDefinitionObjectsCache
    {
        internal Dictionary<int, LDtkDefinitionObjectEntity> Entities = new Dictionary<int, LDtkDefinitionObjectEntity>();
        internal Dictionary<int, LDtkDefinitionObjectField> EntityFields = new Dictionary<int, LDtkDefinitionObjectField>();
        
        internal Dictionary<int, LDtkDefinitionObjectEnum> Enums = new Dictionary<int, LDtkDefinitionObjectEnum>();
        
        internal Dictionary<int, LDtkDefinitionObjectLayer> Layers = new Dictionary<int, LDtkDefinitionObjectLayer>();
        internal Dictionary<int, LDtkDefinitionObjectIntGridValueGroup> IntGridValueGroups = new Dictionary<int, LDtkDefinitionObjectIntGridValueGroup>();
        internal Dictionary<int, LDtkDefinitionObjectAutoLayerRuleGroup> RuleGroups = new Dictionary<int, LDtkDefinitionObjectAutoLayerRuleGroup>();
        internal Dictionary<int, LDtkDefinitionObjectAutoLayerRule> Rules = new Dictionary<int, LDtkDefinitionObjectAutoLayerRule>();
        
        internal Dictionary<int, LDtkDefinitionObjectField> LevelFields = new Dictionary<int, LDtkDefinitionObjectField>();
        
        internal Dictionary<int, LDtkDefinitionObjectTileset> Tilesets = new Dictionary<int, LDtkDefinitionObjectTileset>();
        internal List<LDtkDefinitionObjectTilesetRectangle> TilesetRects = new List<LDtkDefinitionObjectTilesetRectangle>();

        internal List<ScriptableObject> AllObjects = new List<ScriptableObject>();
        
        internal LDtkDebugInstance Logger;
        
        
        internal LDtkDefinitionObjectsCache(LDtkDebugInstance logger)
        {
            Logger = logger;
        }
        
        public void GenerateAndPopulate(Definitions defs)
        {
            Profiler.BeginSample("GenerateAndCacheObjects");
            GenerateAndCacheObjects(defs);
            Profiler.EndSample();
            
            Profiler.BeginSample("PopulateObjects");
            PopulateObjects(defs);
            Profiler.EndSample();
        }
        
        private void GenerateAndCacheObjects(Definitions defs)
        {
            foreach (EntityDefinition def in defs.Entities)
            {
                Entities.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectEntity>());

                foreach (FieldDefinition field in def.FieldDefs)
                {
                    EntityFields.Add(field.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectField>());
                }
            }
            
            foreach (EnumDefinition def in defs.Enums)
            {
                Enums.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectEnum>());
            }
            foreach (EnumDefinition def in defs.ExternalEnums)
            {
                Enums.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectEnum>());
            }
            
            foreach (LayerDefinition def in defs.Layers)
            {
                Layers.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectLayer>());

                foreach (IntGridValueGroupDefinition group in def.IntGridValuesGroups)
                {
                    IntGridValueGroups.Add(group.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectIntGridValueGroup>());
                }
                
                foreach (AutoLayerRuleGroup group in def.AutoRuleGroups)
                {
                    RuleGroups.Add(group.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectAutoLayerRuleGroup>());
                    foreach (AutoLayerRuleDefinition rule in group.Rules)
                    {
                        Rules.Add(rule.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectAutoLayerRule>());
                    }
                }
            }
            
            foreach (FieldDefinition def in defs.LevelFields)
            {
                LevelFields.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectField>());
            }
            
            foreach (TilesetDefinition def in defs.Tilesets)
            {
                Tilesets.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectTileset>());
            }
            
            //cache all
            AllObjects.AddRange(Entities.Values);
            AllObjects.AddRange(EntityFields.Values);
            AllObjects.AddRange(Enums.Values);
            AllObjects.AddRange(Layers.Values);
            AllObjects.AddRange(IntGridValueGroups.Values);
            AllObjects.AddRange(RuleGroups.Values);
            AllObjects.AddRange(Rules.Values);
            AllObjects.AddRange(LevelFields.Values);
            AllObjects.AddRange(Tilesets.Values);
        }

        

        private void PopulateObjects(Definitions defs)
        {
            foreach (EntityDefinition def in defs.Entities)
            {
                Entities[def.Uid].Populate(this, def);
            }
            
            foreach (EnumDefinition def in defs.Enums)
            {
                Enums[def.Uid].Populate(this, def);
            }
            foreach (EnumDefinition def in defs.ExternalEnums)
            {
                Enums[def.Uid].Populate(this, def);
            }
            
            foreach (LayerDefinition def in defs.Layers)
            {
                Layers[def.Uid].Populate(this, def);
            }
            
            foreach (FieldDefinition def in defs.LevelFields)
            {
                LevelFields[def.Uid].Populate(this, def);
            }
            
            foreach (TilesetDefinition def in defs.Tilesets)
            {
                Tilesets[def.Uid].Populate(this, def);
            }
        }
        
        public T GetObject<T>(Dictionary<int,T> dict, int? uid) where T : ScriptableObject
        {
            return uid == null ? null : GetObject(dict, uid.Value);
        }

        public T GetObject<T>(Dictionary<int, T> dict, int uid) where T : ScriptableObject
        {
            if (dict.TryGetValue(uid, out var obj))
            {
                return obj;
            }
            
            Logger.LogError($"Failed to get a \"{typeof(T).Name}\" of uid: {uid}. This is likely from a broken json structure");
            return null;
        }
    }
}