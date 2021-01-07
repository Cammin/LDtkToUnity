using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDtkUnity.Data
{
    public class Level : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// Background color of the level (same as `bgColor`, except the default value is
        /// automatically used here if its value is `null`)
        /// </summary>
        [JsonProperty("__bgColor")]
        public string BgColor { get; set; }

        /// <summary>
        /// An array listing all other levels touching this one on the world map. The `dir` is a
        /// single lowercase character tipping on the level location (`n`orth, `s`outh, `w`est,
        /// `e`ast). In "linear" world layouts, this array is populated with previous/next levels in
        /// array, and `dir` depends on the linear horizontal/vertical layout.
        /// </summary>
        [JsonProperty("__neighbours")]
        public Dictionary<string, dynamic>[] Neighbours { get; set; }

        /// <summary>
        /// Background color of the level. If `null`, the project `defaultLevelBgColor` should be
        /// used.
        /// </summary>
        [JsonProperty("bgColor")]
        public string LevelBgColor { get; set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("layerInstances")]
        public LayerInstance[] LayerInstances { get; set; }

        /// <summary>
        /// Height of the level in pixels
        /// </summary>
        [JsonProperty("pxHei")]
        public long PxHei { get; set; }

        /// <summary>
        /// Width of the level in pixels
        /// </summary>
        [JsonProperty("pxWid")]
        public long PxWid { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// World X coordinate in pixels
        /// </summary>
        [JsonProperty("worldX")]
        public long WorldX { get; set; }

        /// <summary>
        /// World Y coordinate in pixels
        /// </summary>
        [JsonProperty("worldY")]
        public long WorldY { get; set; }
    }
}