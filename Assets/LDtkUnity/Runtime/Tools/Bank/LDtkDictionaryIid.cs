namespace LDtkUnity
{
    internal class LDtkDictionaryIid : LDtkDictionary<string, ILDtkIid>
    {
        protected override string GetKeyFromValue(ILDtkIid value)
        {
            return value.Iid;
        }

        public override void CacheAllData(LdtkJson json)
        {
            World[] worlds = json.UnityWorlds;
            TryAdd(worlds);
            
            foreach (World world in worlds)
            { 
                TryAdd(world.Levels);
                CacheWorldContents(world);
            }
        }

        private void CacheWorldContents(World world)
        {
            foreach (Level level in world.Levels)
            {
                TryAdd(level.LayerInstances);
                CacheLevelContents(level);
            }
        }
        
        private void CacheLevelContents(Level level)
        {
            if (level.LayerInstances == null)
            {
                LDtkDebug.LogError("layer instances was null, we tried working with the separate levels and it was having problems?");
                return;
            }
            
            foreach (LayerInstance layer in level.LayerInstances)
            {
                TryAdd(layer.EntityInstances);
            }
        }
    }
}