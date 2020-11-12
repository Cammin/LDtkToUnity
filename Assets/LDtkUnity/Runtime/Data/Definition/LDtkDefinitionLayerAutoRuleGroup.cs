// ReSharper disable InconsistentNaming

using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#21-layer-definition
    public struct LDtkDefinitionLayerAutoRuleGroup
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public bool active { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public bool collapsed { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public string name { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public LDtkDefinitionAutoLayerRule[] rules { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public int uid { get; private set; }
    }
}