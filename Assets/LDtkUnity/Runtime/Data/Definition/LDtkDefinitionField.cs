// ReSharper disable InconsistentNaming

using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#221-field-definition
    public struct LDtkDefinitionField : ILDtkUid
    {
        /// <summary>
        /// Human readable value type (eg. Int, Float, Point, etc.).
        /// </summary>
        [JsonProperty] public string __type { get; private set; }
        
        /// <summary>
        /// Only FilePath.
        /// Optional list of accepted file extensions for FilePath value type. Includes the dot: .ext
        /// </summary>
        [JsonProperty] public string[] acceptFileTypes { get; private set; }
        
        /// <summary>
        /// Only Array.
        /// Array max length
        /// </summary>
        [JsonProperty] public int arrayMaxLength { get; private set; }
        
        /// <summary>
        /// Only Array.
        /// Array min length
        /// </summary>
        [JsonProperty] public int arrayMinLength { get; private set; }
        
        /// <summary>
        /// TRUE if the value can be null. For arrays, TRUE means it can contain null values (exception: array of Points can't have null values).
        /// </summary>
        [JsonProperty] public bool canBeNull { get; private set; }
        
        /// <summary>
        /// Default value if selected value is null or invalid.
        /// </summary>
        //[JsonProperty] public Enum defaultOverride { get; private set; } //TODO is unidentified enum, come back to later
        
        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty] public string identifier { get; private set; }
        
        /// <summary>
        /// TRUE if the value is an array of multiple values
        /// </summary>
        [JsonProperty] public bool isArray { get; private set; }
        
        /// <summary>
        /// Only Int, Float.
        /// Max limit for value, if applicable
        /// </summary>
        [JsonProperty] public float max { get; private set; }
        
        /// <summary>
        /// Only Int, Float.
        /// Min limit for value, if applicable
        /// </summary>
        [JsonProperty] public float min { get; private set; }
        
        /// <summary>
        /// Internal type enum
        /// </summary>
        //[JsonProperty] public string type { get; private set; } //TODO Listed as string in docs, but can also be object sometimes
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty] public int uid { get; private set; }

    }
}