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
            
            TryAdd(defs.Layers);
            // Int grid value groups do not have globally unique Uids, so this wil never go used in this context
            
            TryAdd(defs.Entities);
            TryAdd(defs.Entities.SelectMany(entity => entity.FieldDefs).ToArray());
            
            TryAdd(defs.Tilesets);
            TryAdd(defs.Enums);
            TryAdd(defs.ExternalEnums);
            
            TryAdd(json.UnityWorlds.SelectMany(p => p.Levels).ToArray());
            TryAdd(defs.LevelFields);
        }
    }
}