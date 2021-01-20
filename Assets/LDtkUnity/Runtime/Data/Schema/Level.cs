using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// This section contains all the level data. It can be found in 2 distinct forms, depending
    /// on Project current settings:  - If "*Separate level files*" is **disabled** (default):
    /// full level data is *embedded* inside the main Project JSON file, - If "*Separate level
    /// files*" is **enabled**: level data is stored in *separate* standalone `.ldtkl` files (one
    /// per level). In this case, the main Project JSON file will still contain most level data,
    /// except heavy sections, like the `layerInstances` array (which will be null). The
    /// `externalRelPath` string points to the `ldtkl` file.  A `ldtkl` file is just a JSON file
    /// containing exactly what is described below.
    /// </summary>
    public partial class Level
    {
        /// <summary>
        /// Background color of the level (same as `bgColor`, except the default value is
        /// automatically used here if its value is `null`)
        /// </summary>
        [JsonProperty("__bgColor")]
        public string BgColor { get; set; }

        /// <summary>
        /// Position informations of the background image, if there is one.
        /// </summary>
        [JsonProperty("__bgPos", NullValueHandling = NullValueHandling.Ignore)]
        public LdtkLevelBgPosInfos BgPos { get; set; }

        /// <summary>
        /// An array listing all other levels touching this one on the world map. In "linear" world
        /// layouts, this array is populated with previous/next levels in array, and `dir` depends on
        /// the linear horizontal/vertical layout.
        /// </summary>
        [JsonProperty("__neighbours")]
        public LdtkNeighbourLevel[] Neighbours { get; set; }

        /// <summary>
        /// Background color of the level. If `null`, the project `defaultLevelBgColor` should be
        /// used.
        /// </summary>
        [JsonProperty("bgColor")]
        public string LevelBgColor { get; set; }

        [JsonProperty("bgPivotX")]
        public double BgPivotX { get; set; }

        [JsonProperty("bgPivotY")]
        public double BgPivotY { get; set; }

        /// <summary>
        /// An enum defining the way the background image (if any) is positioned on the level. See
        /// `__bgPos` for resulting position info. Possible values: `Unscaled`, `Contain`, `Cover`,
        /// `CoverDirty`
        /// </summary>
        [JsonProperty("bgPos", NullValueHandling = NullValueHandling.Ignore)]
        public BgPos? LevelBgPos { get; set; }

        /// <summary>
        /// The *optional* relative path to the level background image.
        /// </summary>
        [JsonProperty("bgRelPath")]
        public string BgRelPath { get; set; }

        /// <summary>
        /// This value is not null if the project option "*Save levels separately*" is enabled. In
        /// this case, this **relative** path points to the level Json file.
        /// </summary>
        [JsonProperty("externalRelPath")]
        public string ExternalRelPath { get; set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// An array containing all Layer instances. **IMPORTANT**: if the project option "*Save
        /// levels separately*" is enabled, this field will be `null`.<br/>  This array is **sorted
        /// in display order**: the 1st layer is the top-most and the last is behind.
        /// </summary>
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