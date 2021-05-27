using Newtonsoft.Json;

namespace LDtkUnity
{
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
        [JsonProperty("__bgPos")]
        public LevelBackgroundPosition BgPos { get; set; }

        /// <summary>
        /// An array listing all other levels touching this one on the world map. In "linear" world
        /// layouts, this array is populated with previous/next levels in array, and `dir` depends on
        /// the linear horizontal/vertical layout.
        /// </summary>
        [JsonProperty("__neighbours")]
        public NeighbourLevel[] Neighbours { get; set; }

        /// <summary>
        /// Background color of the level. If `null`, the project `defaultLevelBgColor` should be
        /// used.
        /// </summary>
        [JsonProperty("bgColor")]
        public string LevelBgColor { get; set; }

        /// <summary>
        /// Background image X pivot (0-1)
        /// </summary>
        [JsonProperty("bgPivotX")]
        public double BgPivotX { get; set; }

        /// <summary>
        /// Background image Y pivot (0-1)
        /// </summary>
        [JsonProperty("bgPivotY")]
        public double BgPivotY { get; set; }

        /// <summary>
        /// An enum defining the way the background image (if any) is positioned on the level. See
        /// `__bgPos` for resulting position info. Possible values: &lt;`null`&gt;, `Unscaled`,
        /// `Contain`, `Cover`, `CoverDirty`
        /// </summary>
        [JsonProperty("bgPos")]
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
        /// An array containing this level custom field values.
        /// </summary>
        [JsonProperty("fieldInstances")]
        public FieldInstance[] FieldInstances { get; set; }

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
        /// If TRUE, the level identifier will always automatically use the naming pattern as defined
        /// in `Project.levelNamePattern`. Becomes FALSE if the identifier is manually modified by
        /// user.
        /// </summary>
        [JsonProperty("useAutoIdentifier")]
        public bool UseAutoIdentifier { get; set; }

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