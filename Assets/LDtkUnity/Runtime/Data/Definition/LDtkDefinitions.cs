// ReSharper disable InconsistentNaming

using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#2-definitions
    public struct LDtkDefinitions
    {
        /// <summary>
        /// Array of Layer definition
        /// </summary>
        [JsonProperty] public LDtkDefinitionLayer[] layers { get; private set; }
        
        /// <summary>
        /// Array of Entity definition
        /// </summary>
        [JsonProperty] public LDtkDefinitionEntity[] entities { get; private set; }
        
        /// <summary>
        /// Array of Tileset definition
        /// </summary>
        [JsonProperty] public LDtkDefinitionTileset[] tilesets { get; private set; }
        
        /// <summary>
        /// Array of Enum definition
        /// </summary>
        [JsonProperty] public LDtkDefinitionEnum[] enums { get; private set; }
        
        /// <summary>
        /// Array of Enum definition
        /// Note: external enums are exactly the same as enums, except they have a relPath to point to an external source file.
        /// </summary>
        [JsonProperty] public LDtkDefinitionEnum[] externalEnums { get; private set; }
    }
}