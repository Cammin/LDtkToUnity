// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Tools;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#1-level
    public struct LDtkDataLevel : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// Background color of the level (same as bgColor, except the default value is automatically used here if its value is null)
        /// </summary>
        [JsonProperty] public string __bgColor { get; private set; }
        
        /// <summary>
        /// An array listing all other levels touching this one on the world map.
        /// The dir is a single lowercase character tipping on the level location (north, south, west, east).
        /// In "linear" world layouts, this array is populated with previous/next levels in array, and dir depends on the linear horizontal/vertical layout.
        /// </summary>
        [JsonProperty] public LDtkDataLevelNeighbour[] __neighbours { get; private set; }
        
        /// <summary>
        /// Background color of the level. If null, the project defaultLevelBgColor should be used.
        /// </summary>
        [JsonProperty] public string bgColor { get; private set; }
        
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
        
        /// <summary>
        /// World X coordinate in pixels
        /// </summary>
        [JsonProperty] public int worldX { get; private set; }
        
        /// <summary>
        /// World Y coordinate in pixels
        /// </summary>
        [JsonProperty] public int worldY { get; private set; }

        public Color BgColor => __bgColor.ToColor();
        public Vector2Int PxSize => new Vector2Int(pxWid, pxHei);
        public Vector2Int WorldCoord => new Vector2Int(worldX, worldY);
        public Bounds GetLevelBounds(int pixelsPerUnit) => new Bounds(new Vector3(worldX, worldY, 0), new Vector3(pxWid, pxHei, 0) * pixelsPerUnit); 
    }
}
