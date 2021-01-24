using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// The `Tileset` definition is the most important part among project definitions. It
    /// contains some extra informations about each integrated tileset. If you only had to parse
    /// one definition section, that would be the one.
    /// </summary>
    public partial class TilesetDefinition
    {
        /// <summary>
        /// The following data is used internally for various optimizations. It's always synced with
        /// source image changes.
        /// </summary>
        [JsonProperty("cachedPixelData")]
        public Dictionary<string, object> CachedPixelData { get; set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Distance in pixels from image borders
        /// </summary>
        [JsonProperty("padding")]
        public long Padding { get; set; }

        /// <summary>
        /// Image height in pixels
        /// </summary>
        [JsonProperty("pxHei")]
        public long PxHei { get; set; }

        /// <summary>
        /// Image width in pixels
        /// </summary>
        [JsonProperty("pxWid")]
        public long PxWid { get; set; }

        /// <summary>
        /// Path to the source file, relative to the current project JSON file
        /// </summary>
        [JsonProperty("relPath")]
        public string RelPath { get; set; }

        /// <summary>
        /// Array of group of tiles selections, only meant to be used in the editor
        /// </summary>
        [JsonProperty("savedSelections")]
        public Dictionary<string, object>[] SavedSelections { get; set; }

        /// <summary>
        /// Space in pixels between all tiles
        /// </summary>
        [JsonProperty("spacing")]
        public long Spacing { get; set; }

        [JsonProperty("tileGridSize")]
        public long TileGridSize { get; set; }

        /// <summary>
        /// Unique Intidentifier
        /// </summary>
        [JsonProperty("uid")]
        public long Uid { get; set; }
    }
}