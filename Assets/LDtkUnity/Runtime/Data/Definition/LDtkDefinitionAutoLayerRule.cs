// ReSharper disable InconsistentNaming

using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#211-auto-layer-rule-definition
    public struct LDtkDefinitionAutoLayerRule
    {
        /// <summary>
        /// If FALSE, the rule effect isn't applied, and no tiles are generated.
        /// </summary>
        [JsonProperty] public bool active { get; private set; }
        
        /// <summary>
        /// When TRUE, the rule will prevent other rules to be applied in the same cell if it matches (TRUE by default).
        /// </summary>
        [JsonProperty] public bool breakOnMatch { get; private set; }
        
        /// <summary>
        /// Chances for this rule to be applied (0 to 1)
        /// </summary>
        [JsonProperty] public float chance { get; private set; }
        
        /// <summary>
        /// If TRUE, enable checker mode
        /// </summary>
        [JsonProperty] public string checker { get; private set; } //todo is listed as bool in docs, but is a string
        
        /// <summary>
        /// If TRUE, allow rule to be matched by flipping its pattern horizontally
        /// </summary>
        [JsonProperty] public bool flipX { get; private set; }
        
        /// <summary>
        /// If TRUE, allow rule to be matched by flipping its pattern vertically
        /// </summary>
        [JsonProperty] public bool flipY { get; private set; }
        
        /// <summary>
        /// Rule pattern (size x size)
        /// </summary>
        [JsonProperty] public int[] pattern { get; private set; }
        
        /// <summary>
        /// If TRUE, enable Perlin filtering to only apply rule on specific random area
        /// </summary>
        [JsonProperty] public bool perlinActive { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public float perlinOctaves { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public float perlinScale { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public float perlinSeed { get; private set; }
        
        /// <summary>
        /// Only 'Stamp' tile mode.
        /// X pivot of a tile stamp (0-1)
        /// </summary>
        [JsonProperty] public float pivotX { get; private set; }
        
        /// <summary>
        /// Only 'Stamp' tile mode
        /// Y pivot of a tile stamp (0-1)
        /// </summary>
        [JsonProperty] public float pivotY { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public int size { get; private set; }
        
        /// <summary>
        /// Array of all the tile IDs. They are used randomly or as stamps, based on tileMode value.
        /// </summary>
        [JsonProperty] public int[] tileIds { get; private set; }
        
        /// <summary>
        /// Defines how tileIds array is used
        /// </summary>
        //[JsonProperty] public Enum tileMode { get; private set; }//todo is an enum, not sure about definition yet
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty] public int uid { get; private set; }
        
        /// <summary>
        /// X cell coord modulo
        /// </summary>
        [JsonProperty] public int xModulo { get; private set; }
        
        /// <summary>
        /// Y cell coord modulo
        /// </summary>
        [JsonProperty] public int yModulo { get; private set; }
    }
}