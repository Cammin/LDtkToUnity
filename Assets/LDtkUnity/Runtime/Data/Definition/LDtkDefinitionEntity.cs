// ReSharper disable InconsistentNaming

using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#22-entity-definition
    public struct LDtkDefinitionEntity
    {
        /// <summary>
        /// Base entity color
        /// </summary>
        [JsonProperty] public string color { get; private set; }
        
        /// <summary>
        /// Array of field definitions
        /// </summary>
        [JsonProperty] public LDtkDefinitionField[] fieldDefs { get; private set; }
        
        /// <summary>
        /// Pixel height
        /// </summary>
        [JsonProperty] public int height { get; private set; }
        
        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty] public string identifier { get; private set; }
        
        /// <summary>
        /// Max instances per level
        /// </summary>
        [JsonProperty] public int maxPerLevel { get; private set; }
        
        /// <summary>
        /// Pivot X coordinate (from 0 to 1.0)
        /// </summary>
        [JsonProperty] public float pivotX { get; private set; }
        
        /// <summary>
        /// Pivot Y coordinate (from 0 to 1.0)
        /// </summary>
        [JsonProperty] public float pivotY { get; private set; }
        
        /// <summary>
        /// Tile ID used for optional tile display
        /// </summary>
        [JsonProperty] public int tileId { get; private set; }
        
        /// <summary>
        /// Tileset ID used for optional tile display
        /// </summary>
        [JsonProperty] public int tilesetId { get; private set; }
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty] public int uid { get; private set; }
        
        /// <summary>
        /// Pixel width
        /// </summary>
        [JsonProperty] public int width { get; private set; }
    }
}