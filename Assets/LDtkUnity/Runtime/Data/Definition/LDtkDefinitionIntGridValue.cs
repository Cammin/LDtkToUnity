// ReSharper disable InconsistentNaming

using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#21-layer-definition
    public struct LDtkDefinitionIntGridValue
    {
        /// <summary>
        /// Identifier
        /// </summary>
        [JsonProperty] public string identifier { get; private set; }
        
        /// <summary>
        /// Hex color "#rrggbb"
        /// </summary>
        [JsonProperty] public string color { get; private set; }
    }
}