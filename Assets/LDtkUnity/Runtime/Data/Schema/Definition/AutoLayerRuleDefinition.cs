using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class AutoLayerRuleDefinition
    {
        /// <summary>
        /// If FALSE, the rule effect isn't applied, and no tiles are generated.
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }

        /// <summary>
        /// When TRUE, the rule will prevent other rules to be applied in the same cell if it matches
        /// (TRUE by default).
        /// </summary>
        [JsonProperty("breakOnMatch")]
        public bool BreakOnMatch { get; set; }

        /// <summary>
        /// Chances for this rule to be applied (0 to 1)
        /// </summary>
        [JsonProperty("chance")]
        public double Chance { get; set; }

        /// <summary>
        /// Checker mode Possible values: `None`, `Horizontal`, `Vertical`
        /// </summary>
        [JsonProperty("checker")]
        public Checker Checker { get; set; }

        /// <summary>
        /// If TRUE, allow rule to be matched by flipping its pattern horizontally
        /// </summary>
        [JsonProperty("flipX")]
        public bool FlipX { get; set; }

        /// <summary>
        /// If TRUE, allow rule to be matched by flipping its pattern vertically
        /// </summary>
        [JsonProperty("flipY")]
        public bool FlipY { get; set; }

        /// <summary>
        /// Default IntGrid value when checking cells outside of level bounds
        /// </summary>
        [JsonProperty("outOfBoundsValue")]
        public long? OutOfBoundsValue { get; set; }

        /// <summary>
        /// Rule pattern (size x size)
        /// </summary>
        [JsonProperty("pattern")]
        public long[] Pattern { get; set; }

        /// <summary>
        /// If TRUE, enable Perlin filtering to only apply rule on specific random area
        /// </summary>
        [JsonProperty("perlinActive")]
        public bool PerlinActive { get; set; }

        [JsonProperty("perlinOctaves")]
        public double PerlinOctaves { get; set; }

        [JsonProperty("perlinScale")]
        public double PerlinScale { get; set; }

        [JsonProperty("perlinSeed")]
        public double PerlinSeed { get; set; }

        /// <summary>
        /// X pivot of a tile stamp (0-1)
        /// </summary>
        [JsonProperty("pivotX")]
        public double PivotX { get; set; }

        /// <summary>
        /// Y pivot of a tile stamp (0-1)
        /// </summary>
        [JsonProperty("pivotY")]
        public double PivotY { get; set; }

        /// <summary>
        /// Pattern width & height. Should only be 1,3,5 or 7.
        /// </summary>
        [JsonProperty("size")]
        public long Size { get; set; }

        /// <summary>
        /// Array of all the tile IDs. They are used randomly or as stamps, based on `tileMode` value.
        /// </summary>
        [JsonProperty("tileIds")]
        public long[] TileIds { get; set; }

        /// <summary>
        /// Defines how tileIds array is used Possible values: `Single`, `Stamp`
        /// </summary>
        [JsonProperty("tileMode")]
        public TileMode TileMode { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// X cell coord modulo
        /// </summary>
        [JsonProperty("xModulo")]
        public long XModulo { get; set; }

        /// <summary>
        /// Y cell coord modulo
        /// </summary>
        [JsonProperty("yModulo")]
        public long YModulo { get; set; }
    }
}