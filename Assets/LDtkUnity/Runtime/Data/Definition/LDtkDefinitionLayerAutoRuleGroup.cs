// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#21-layer-definition
    public struct LDtkDefinitionLayerAutoRuleGroup
    {
        /// <summary>
        /// 
        /// </summary>
        public bool active;
        
        /// <summary>
        /// 
        /// </summary>
        public bool collapsed;
        
        /// <summary>
        /// 
        /// </summary>
        public string name;
        
        /// <summary>
        /// 
        /// </summary>
        public LDtkDefinitionAutoLayerRule[] rules;
        
        /// <summary>
        /// 
        /// </summary>
        public bool uid;
    }
}