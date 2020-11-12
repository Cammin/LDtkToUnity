// ReSharper disable InconsistentNaming

using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#1-level
    public struct LDtkDataLevel
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty] public string identifier { get; private set; }
        
        /// <summary>
        /// Array of Layer instance
        /// </summary>
        [JsonProperty] public LDtkDataLayer[] layerInstances { get; private set; }
        
        /// <summary>
        /// Height of the level in pixels
        /// </summary>
        [JsonProperty] public int pxHei { get; private set; }
        
        /// <summary>
        /// Width of the level in pixels
        /// </summary>
        [JsonProperty] public int pxWid { get; private set; }
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty] public int uid { get; private set; }
        
        
    }
}
