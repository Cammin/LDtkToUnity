// ReSharper disable InconsistentNaming

using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#23-tileset-definition
    public struct LDtkDefinitionTileset
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty] public string identifier { get; private set; }

        /// <summary>
        /// An array of all tiles that are fully opaque (ie. no transparent pixel). Used internally for optimizations.
        /// </summary>
        [JsonProperty] public int[] opaqueTiles { get; private set; }
        
        /// <summary>
        /// Distance in pixels from image borders
        /// </summary>
        [JsonProperty] public int padding { get; private set; }
        
        /// <summary>
        /// Image width in pixels
        /// </summary>
        [JsonProperty] public int pxHei { get; private set; }
        
        /// <summary>
        /// Image width in pixels
        /// </summary>
        [JsonProperty] public int pxWid { get; private set; }
        
        /// <summary>
        /// Path to the source file, relative to the current project JSON file
        /// </summary>
        [JsonProperty] public string relPath { get; private set; }
        
        /// <summary>
        /// Space in pixels between all tiles
        /// </summary>
        [JsonProperty] public int spacing { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public int tileGridSize { get; private set; }
        
        /// <summary>
        /// Unique identifier
        /// </summary>
        [JsonProperty] public int uid { get; private set; }
    }
}