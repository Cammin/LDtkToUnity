using System.Linq;
using LDtkUnity.Runtime.Data.Definition;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkDefinitionUtility
    {
        
        
        
        public static LDtkDefinitionTileset GetTilesetByUid(this LDtkDefinitions definitions, int uid)
        {
            return definitions.tilesets.First(p => p.uid == uid);
        }
    }
}