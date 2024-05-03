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
    /// All others will do the [field: SerializeField] with a { get; private set; }
    /// 
    /// - All uid refs should translate into the appropriate scriptable object by relaying in a second pass over all def objects.
    /// - All strings/ints/floats that might represent a color or Vector2 can be turned into the appropriate structs.
    /// </summary>
    internal class LDtkDefinitionObjectsCache
    {
        private readonly Dictionary<int, LDtkDefinitionObject> _defsDict = new Dictionary<int, LDtkDefinitionObject>();
        private readonly LDtkDebugInstance _logger;
        
        internal List<LDtkDefinitionObject> Defs = new List<LDtkDefinitionObject>();
        
        //key: tilesetuid, value: rectangle to sprite ref
        private Dictionary<int, Dictionary<Rect,Sprite>> _allSprites;
        
        internal LDtkDefinitionObjectsCache(LDtkDebugInstance logger)
        {
            _logger = logger;
        }
        
        public void InitializeFromProject(Definitions defs, Dictionary<int, LDtkArtifactAssetsTileset> tilesets)
        {
            InitializeTilesets(tilesets);

            Profiler.BeginSample("GenerateObjects");
            GenerateObjects(defs);
            Profiler.EndSample();
            
            Profiler.BeginSample("PopulateObjects");
            PopulateObjects(defs);
            Profiler.EndSample();
            
            Profiler.BeginSample("SetObjectNames");
            SetObjectNames();
            Profiler.EndSample();

            Profiler.BeginSample("CacheDictToList");
            CacheDictToList();
            Profiler.EndSample();
        }

        

        public void InitializeFromLevel(List<LDtkDefinitionObject> defs, Dictionary<int, LDtkArtifactAssetsTileset> tilesets)
        {
            InitializeTilesets(tilesets);
            
            Defs = defs;
            CacheListToDict();
        }
        
        private void InitializeTilesets(Dictionary<int, LDtkArtifactAssetsTileset> tilesets)
        {
            _allSprites = new Dictionary<int, Dictionary<Rect,Sprite>>(tilesets.Count);
            foreach (var pair in tilesets)
            {
                Dictionary<Rect,Sprite> dict = pair.Value != null ? pair.Value.AllSpritesToConvertedDict() : new Dictionary<Rect, Sprite>();
                _allSprites.Add(pair.Key, dict);
            }
        }

        private void GenerateObjects(Definitions defs)
        {
            foreach (EntityDefinition def in defs.Entities)
            {
                _defsDict.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectEntity>());

                foreach (FieldDefinition field in def.FieldDefs)
                {
                    _defsDict.Add(field.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectField>());
                }
            }
            
            AddEnums(defs.Enums);
            AddEnums(defs.ExternalEnums);
            void AddEnums(EnumDefinition[] enums)
            {
                foreach (EnumDefinition def in enums)
                {
                    _defsDict.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectEnum>());
                }
            }
            
            foreach (LayerDefinition def in defs.Layers)
            {
                _defsDict.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectLayer>());
                
                foreach (AutoLayerRuleGroup group in def.AutoRuleGroups)
                {
                    _defsDict.Add(group.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectAutoLayerRuleGroup>());
                    foreach (AutoLayerRuleDefinition rule in group.Rules)
                    {
                        _defsDict.Add(rule.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectAutoLayerRule>());
                    }
                }
            }
            
            foreach (FieldDefinition def in defs.LevelFields)
            {
                _defsDict.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectField>());
            }
            
            foreach (TilesetDefinition def in defs.Tilesets)
            {
                _defsDict.Add(def.Uid, ScriptableObject.CreateInstance<LDtkDefinitionObjectTileset>());
            }
        }
        
        private void PopulateObjects(Definitions defs)
        {
            foreach (EntityDefinition def in defs.Entities)
            {
                if (_defsDict[def.Uid] is LDtkDefinitionObjectEntity obj)
                {
                    obj.Populate(this, def);
                }
            }
            
            AddEnums(defs.Enums);
            AddEnums(defs.ExternalEnums);
            void AddEnums(EnumDefinition[] enumDefs)
            {
                foreach (EnumDefinition def in enumDefs)
                {
                    if (_defsDict[def.Uid] is LDtkDefinitionObjectEnum obj)
                    {
                        obj.Populate(this, def);
                    }
                }
            }
            
            foreach (LayerDefinition def in defs.Layers)
            {
                if (_defsDict[def.Uid] is LDtkDefinitionObjectLayer obj)
                {
                    obj.Populate(this, def);
                }
            }
            
            foreach (FieldDefinition def in defs.LevelFields)
            {
                if (_defsDict[def.Uid] is LDtkDefinitionObjectField obj)
                {
                    obj.Populate(this, def);
                }
            }
            
            foreach (TilesetDefinition def in defs.Tilesets)
            {
                if (_defsDict[def.Uid] is LDtkDefinitionObjectTileset obj)
                {
                    obj.Populate(this, def);
                }
            }
        }
        
        private void SetObjectNames()
        {
            foreach (var obj in _defsDict.Values)
            {
                obj.SetAssetName();
            }
        }
        
        public T GetObject<T>(int? uid) where T : ScriptableObject
        {
            return uid == null ? null : GetObject<T>(uid.Value);
        }

        public T GetObject<T>(int uid) where T : ScriptableObject
        {
            if (!_defsDict.TryGetValue(uid, out var obj))
            {
                _logger.LogError($"Failed to get a \"{typeof(T).Name}\" of uid: {uid}. This is likely from a broken json structure");
                return null;
            }

            if (obj is T t)
            {
                return t;
            }
            
            _logger.LogError($"Failed to get a \"{typeof(T).Name}\" of uid {uid} due to a type mismatch with {obj.GetType().Name}");
            return null;
        }

        public Sprite GetSpriteForTilesetRectangle(TilesetRectangle def)
        {
            if (def == null)
            {
                return null;
            }

            if (!_allSprites.TryGetValue(def.TilesetUid, out var sprites))
            {
                _logger.LogError($"Problem getting sprite for TilesetRectangle def uid {def.TilesetUid}: Couldn't get the dictionary for the tileset uid.");
                return null;
            }

            if (sprites.IsNullOrEmpty())
            {
                //this means there were no sprites; a previous issue is the actual problem to not have any.
                return null;
            }
            
            if (!sprites.TryGetValue(def.UnityRect, out var sprite))
            {
                _logger.LogError($"Problem getting sprite for TilesetRectangle def uid {def.TilesetUid}: Couldn't get the sprite from the dictionary for the Rect {def.UnityRect} out of {sprites.Count} possible rects");
                return null;
            }

            return sprite;
        }
        
        private void CacheDictToList()
        {
            foreach (LDtkDefinitionObject def in _defsDict.Values)
            {
                Defs.Add(def);
            }
        }
        
        private void CacheListToDict()
        {
            foreach (LDtkDefinitionObject def in Defs)
            {
                if (def is ILDtkUid uid)
                {
                    _defsDict.Add(uid.Uid, def);
                }
            }
        }
    }
}