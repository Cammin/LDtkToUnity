namespace LDtkUnity
{
    internal class LDtkDictionaryIid : LDtkDictionary<string, ILDtkIid>
    {
        protected override string GetKeyFromValue(ILDtkIid value)
        {
            return value.Iid;
        }

        public void CacheAllData(LdtkJson json, Level separateLevel = null)
        {
            if (json.ExternalLevels)
            {
                CacheLevelContents(separateLevel);
            }
            
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
            if (level == null)
            {
                return;
            }
            
            if (level.LayerInstances == null)
            {
                return;
            }
            
            foreach (LayerInstance layer in level.LayerInstances)
            {
                TryAdd(layer.EntityInstances);
            }
        }
    }
}