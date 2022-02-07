using System.Linq;

namespace LDtkUnity
{
    internal class LDtkDictionaryUId : LDtkDictionary<long, ILDtkUid>
    {
        protected override long GetKeyFromValue(ILDtkUid value)
        {
            return value.Uid;
        }

        public override void CacheAllData(LdtkJson json)
        {
            Definitions defs = json.Defs;
            
            TryAdd(defs.Layers);
            CacheEntityDefs(defs.Entities);
            
            TryAdd(defs.Tilesets);
            TryAdd(defs.Enums);
            TryAdd(defs.ExternalEnums);
            
            TryAdd(json.Levels);
            TryAdd(defs.LevelFields);
        }
        
        private void CacheEntityDefs(EntityDefinition[] entityDefs)
        {
            TryAdd(entityDefs);
            
            FieldDefinition[] fieldDefs = entityDefs.SelectMany(entity => entity.FieldDefs).ToArray();
            TryAdd(fieldDefs);
        }
    }
}