// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity.Runtime.Data
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#ldtk-json-root
    public struct LDtkDataProject
    {
        /// <summary>
        /// Project background color. Hex color "#rrggbb"
        /// </summary>
        [JsonProperty] public string bgColor { get; private set; }
        
        /// <summary>
        /// Default grid size for new layers
        /// </summary>
        [JsonProperty] public int defaultGridSize { get; private set; }
        
        /// <summary>
        /// Default background color of levels.
        /// Hex color "#rrggbb"
        /// </summary>
        [JsonProperty] public string defaultLevelBgColor { get; private set; }
        
        /// <summary>
        /// Default X pivot (0 to 1) for new entities
        /// </summary>
        [JsonProperty] public float defaultPivotX { get; private set; }
        
        /// <summary>
        /// Default Y pivot (0 to 1) for new entities
        /// </summary>
        [JsonProperty] public float defaultPivotY { get; private set; }
        
        /// <summary>
        /// A structure containing all the definitions of this project
        /// </summary>
        [JsonProperty] public LDtkDefinitions defs { get; private set; }

        /// <summary>
        /// If TRUE, a Tiled compatible file will also be generated along with the LDtk JSON file (default is FALSE)
        /// </summary>
        [JsonProperty] public bool exportTiled { get; private set; }
        
        /// <summary>
        /// File format version
        /// </summary>
        [JsonProperty] public string jsonVersion { get; private set; }
        
        /// <summary>
        /// All levels.
        /// The order of this array is only relevant in LinearHorizontal and linearVertical world layouts (see worldLayout value).
        /// Otherwise, you should refer to the worldX,worldY coordinates of each Level.
        /// </summary>
        [JsonProperty] public LDtkDataLevel[] levels { get; private set; }

        /// <summary>
        /// If TRUE, the Json is partially minified (no indentation, nor line breaks, default is FALSE)
        /// </summary>
        [JsonProperty] public bool minifyJson { get; private set; }
        
        /// <summary>
        /// Only 'GridVania' layouts.
        /// Height of the world grid in pixels.
        /// </summary>
        [JsonProperty] public int worldGridHeight { get; private set; }
        
        /// <summary>
        /// Only 'GridVania' layouts.
        /// Width of the world grid in pixels.
        /// </summary>
        [JsonProperty] public int worldGridWidth { get; private set; }
        
        /// <summary>
        /// An enum that describes how levels are organized in this project (ie. linearly or in a 2D space).
        /// Possible values are: Free, GridVania, LinearHorizontal and LinearVertical;
        /// </summary>
        [JsonProperty] public string worldLayout { get; private set; }

        public Color BgColor => bgColor.ToColor();
        public Color DefaultLevelBgColor => defaultLevelBgColor.ToColor();
        public Vector2 DefaultPivot => new Vector2(defaultPivotX, defaultPivotY);
        public Vector2Int WorldGridSize => new Vector2Int(worldGridWidth, worldGridHeight);
    }
}