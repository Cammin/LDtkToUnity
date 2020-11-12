// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#ldtk-json-root
    public struct LDtkDataProject
    {
        /// <summary>
        /// File format version
        /// </summary>
        [JsonProperty] public string jsonVersion { get; private set; }

        /// <summary>
        /// Default X pivot (0 to 1) for new entities
        /// </summary>
        [JsonProperty] public float defaultPivotX { get; private set; }
        
        /// <summary>
        /// Default Y pivot (0 to 1) for new entities
        /// </summary>
        [JsonProperty] public float defaultPivotY { get; private set; }
        
        /// <summary>
        /// Default grid size for new layers
        /// </summary>
        [JsonProperty] public int defaultGridSize { get; private set; }

        /// <summary>
        /// Project background color. Hexadecimal string using "#rrggbb" format
        /// </summary>
        [JsonProperty] public string bgColor { get; private set; }
        
        /// <summary>
        /// If TRUE, the Json is partially minified (no indentation, nor line breaks)
        /// </summary>
        [JsonProperty] public bool minifyJson { get; private set; }        
        
        /// <summary>
        /// If TRUE, a Tiled compatible file will also be generated along with the LDtk JSON file.
        /// </summary>
        [JsonProperty] public bool exportTiled { get; private set; }
        
        /// <summary>
        /// A structure containing all the definitions of this project
        /// </summary>
        [JsonProperty] public LDtkDefinitions defs { get; private set; }
        
        /// <summary>
        /// Array of Level
        /// </summary>
        [JsonProperty] public LDtkDataLevel[] levels { get; private set; }
    }
}