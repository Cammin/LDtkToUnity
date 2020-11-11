using System.Collections.Generic;
using LDtkUnity.Runtime.Data.Definition;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkProviderDefinition
    {
        public static Dictionary<int, LDtkDefinitionLayer> Layers = null;
        public static Dictionary<int, LDtkDefinitionLayerAutoRuleGroup> AutoRuleGroups = null;
        public static Dictionary<int, LDtkDefinitionAutoLayerRule> AutoRules = null;
        
        public static Dictionary<int, LDtkDefinitionEntity> Entities = null;
        public static Dictionary<int, LDtkDefinitionField> Fields = null;
        
        public static Dictionary<int, LDtkDefinitionTileset> Tilesets = null;
        public static Dictionary<int, LDtkDefinitionEnum> Enums = null;
        


        public static void Init()
        {
            Layers = new Dictionary<int, LDtkDefinitionLayer>();
            AutoRuleGroups = new Dictionary<int, LDtkDefinitionLayerAutoRuleGroup>();
            AutoRules = new Dictionary<int, LDtkDefinitionAutoLayerRule>();
            Entities = new Dictionary<int, LDtkDefinitionEntity>();
            Fields = new Dictionary<int, LDtkDefinitionField>();
            Tilesets = new Dictionary<int, LDtkDefinitionTileset>();
            Enums = new Dictionary<int, LDtkDefinitionEnum>();
        }

        public static void Dispose()
        {
            Layers = null;
            AutoRuleGroups = null;
            AutoRules = null;
            Entities = null;
            Fields = null;
            Tilesets = null;
            Enums = null;
        }
    }
}