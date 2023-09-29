using System.Collections.Generic;
using System.Linq;

namespace LDtkUnity
{
    internal class LDtkDictionaryUid : LDtkDictionary<long, ILDtkUid>
    {
        protected override long GetKeyFromValue(ILDtkUid value)
        {
            return value.Uid;
        }

        public void CacheAllData(LdtkJson json)
        {
            if (json == null)
            {
                //Debug.LogError("LDtk: json was null, not caching uids");
                return;
            }
            
            Definitions defs = json.Defs;
            
            CacheLayerDefs(defs.Layers);
            CacheEntityDefs(defs.Entities);
            
            TryAdd(defs.Tilesets);
            TryAdd(defs.Enums);
            TryAdd(defs.ExternalEnums);
            
            TryAdd(json.UnityWorlds.SelectMany(p => p.Levels).ToArray());
            TryAdd(defs.LevelFields);
        }
        
        private void CacheEntityDefs(EntityDefinition[] entityDefs)
        {
            TryAdd(entityDefs);
            
            FieldDefinition[] fieldDefs = entityDefs.SelectMany(entity => entity.FieldDefs).ToArray();
            TryAdd(fieldDefs);
        }
        
        private void CacheLayerDefs(LayerDefinition[] layerDefs)
        {
            TryAdd(layerDefs);
            
            //we probably dont need to cache this? It was causing problems
            //CacheIntGridValueGroupDefinitions(layerDefs);
        }

        private void CacheIntGridValueGroupDefinitions(LayerDefinition[] layerDefs)
        {
            List<IntGridValueGroupDefinition> defs = new List<IntGridValueGroupDefinition>();
            foreach (LayerDefinition layerDef in layerDefs)
            {
                //if it's an old json value
                if (layerDef.IntGridValuesGroups == null)
                {
                    continue;
                }

                foreach (IntGridValueGroupDefinition group in layerDef.IntGridValuesGroups)
                {
                    defs.Add(group);
                }
            }

            if (!defs.IsNullOrEmpty())
            {
                TryAdd(defs.ToArray());
            }
        }
    }
}