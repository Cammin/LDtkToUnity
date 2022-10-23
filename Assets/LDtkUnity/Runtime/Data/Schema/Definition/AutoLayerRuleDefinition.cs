using System.Text.Json.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// This complex section isn't meant to be used by game devs at all, as these rules are
    /// completely resolved internally by the editor before any saving. You should just ignore
    /// this part.
    /// </summary>
    public partial class AutoLayerRuleDefinition
    {
        /// <summary>
        /// If FALSE, the rule effect isn't applied, and no tiles are generated.
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /// <summary>
        /// When TRUE, the rule will prevent other rules to be applied in the same cell if it matches
        /// (TRUE by default).
        /// </summary>
        [JsonPropertyName("breakOnMatch")]
        public bool BreakOnMatch { get; set; }

        /// <summary>
        /// Chances for this rule to be applied (0 to 1)
        /// </summary>
        [JsonPropertyName("chance")]
        public double Chance { get; set; }

        /// <summary>
        /// Checker mode Possible values: `None`, `Horizontal`, `Vertical`
        /// </summary>
        [JsonPropertyName("checker")]
        public Checker Checker { get; set; }

        /// <summary>
        /// If TRUE, allow rule to be matched by flipping its pattern horizontally
        /// </summary>
        [JsonPropertyName("flipX")]
        public bool FlipX { get; set; }

        /// <summary>
        /// If TRUE, allow rule to be matched by flipping its pattern vertically
        /// </summary>
        [JsonPropertyName("flipY")]
        public bool FlipY { get; set; }

        /// <summary>
        /// Default IntGrid value when checking cells outside of level bounds
        /// </summary>
        [JsonPropertyName("outOfBoundsValue")]
        public long? OutOfBoundsValue { get; set; }

        /// <summary>
        /// Rule pattern (size x size)
        /// </summary>
        [JsonPropertyName("pattern")]
        public long[] Pattern { get; set; }

        /// <summary>
        /// If TRUE, enable Perlin filtering to only apply rule on specific random area
        /// </summary>
        [JsonPropertyName("perlinActive")]
        public bool PerlinActive { get; set; }

        [JsonPropertyName("perlinOctaves")]
        public double PerlinOctaves { get; set; }

        [JsonPropertyName("perlinScale")]
        public double PerlinScale { get; set; }

        [JsonPropertyName("perlinSeed")]
        public double PerlinSeed { get; set; }

        /// <summary>
        /// X pivot of a tile stamp (0-1)
        /// </summary>
        [JsonPropertyName("pivotX")]
        public double PivotX { get; set; }

        /// <summary>
        /// Y pivot of a tile stamp (0-1)
        /// </summary>
        [JsonPropertyName("pivotY")]
        public double PivotY { get; set; }

        /// <summary>
        /// Pattern width & height. Should only be 1,3,5 or 7.
        /// </summary>
        [JsonPropertyName("size")]
        public long Size { get; set; }

        /// <summary>
        /// Array of all the tile IDs. They are used randomly or as stamps, based on `tileMode` value.
        /// </summary>
        [JsonPropertyName("tileIds")]
        public long[] TileIds { get; set; }

        /// <summary>
        /// Defines how tileIds array is used Possible values: `Single`, `Stamp`
        /// </summary>
        [JsonPropertyName("tileMode")]
        public TileMode TileMode { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonPropertyName("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// X cell coord modulo
        /// </summary>
        [JsonPropertyName("xModulo")]
        public long XModulo { get; set; }

        /// <summary>
        /// X cell start offset
        /// </summary>
        [JsonPropertyName("xOffset")]
        public long XOffset { get; set; }

        /// <summary>
        /// Y cell coord modulo
        /// </summary>
        [JsonPropertyName("yModulo")]
        public long YModulo { get; set; }

        /// <summary>
        /// Y cell start offset
        /// </summary>
        [JsonPropertyName("yOffset")]
        public long YOffset { get; set; }
    }
}