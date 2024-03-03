using System.Runtime.Serialization;

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
        [DataMember(Name = "active")]
        public bool Active { get; set; }

        [DataMember(Name = "alpha")]
        public float Alpha { get; set; }

        /// <summary>
        /// When TRUE, the rule will prevent other rules to be applied in the same cell if it matches
        /// (TRUE by default).
        /// </summary>
        [DataMember(Name = "breakOnMatch")]
        public bool BreakOnMatch { get; set; }

        /// <summary>
        /// Chances for this rule to be applied (0 to 1)
        /// </summary>
        [DataMember(Name = "chance")]
        public float Chance { get; set; }

        /// <summary>
        /// Checker mode Possible values: `None`, `Horizontal`, `Vertical`
        /// </summary>
        [DataMember(Name = "checker")]
        public Checker Checker { get; set; }

        /// <summary>
        /// If TRUE, allow rule to be matched by flipping its pattern horizontally
        /// </summary>
        [DataMember(Name = "flipX")]
        public bool FlipX { get; set; }

        /// <summary>
        /// If TRUE, allow rule to be matched by flipping its pattern vertically
        /// </summary>
        [DataMember(Name = "flipY")]
        public bool FlipY { get; set; }
        
        /// <summary>
        /// If TRUE, then the rule should be re-evaluated by the editor at one point
        /// </summary>
        [DataMember(Name = "invalidated")]
        public bool Invalidated { get; set; }

        /// <summary>
        /// Default IntGrid value when checking cells outside of level bounds
        /// </summary>
        [DataMember(Name = "outOfBoundsValue")]
        public int? OutOfBoundsValue { get; set; }

        /// <summary>
        /// Rule pattern (size x size)
        /// </summary>
        [DataMember(Name = "pattern")]
        public int[] Pattern { get; set; }

        /// <summary>
        /// If TRUE, enable Perlin filtering to only apply rule on specific random area
        /// </summary>
        [DataMember(Name = "perlinActive")]
        public bool PerlinActive { get; set; }

        [DataMember(Name = "perlinOctaves")]
        public float PerlinOctaves { get; set; }

        [DataMember(Name = "perlinScale")]
        public float PerlinScale { get; set; }

        [DataMember(Name = "perlinSeed")]
        public float PerlinSeed { get; set; }

        /// <summary>
        /// X pivot of a tile stamp (0-1)
        /// </summary>
        [DataMember(Name = "pivotX")]
        public float PivotX { get; set; }

        /// <summary>
        /// Y pivot of a tile stamp (0-1)
        /// </summary>
        [DataMember(Name = "pivotY")]
        public float PivotY { get; set; }

        /// <summary>
        /// Pattern width and height. Should only be 1,3,5 or 7.
        /// </summary>
        [DataMember(Name = "size")]
        public int Size { get; set; }

        /// <summary>
        /// **WARNING**: this deprecated value is no longer exported since version 1.5.0  Replaced
        /// by: `tileRectsIds`
        /// </summary>
        [DataMember(Name = "tileIds")]
        public int[] TileIds { get; set; }

        /// <summary>
        /// Defines how tileIds array is used Possible values: `Single`, `Stamp`
        /// </summary>
        [DataMember(Name = "tileMode")]
        public TileMode TileMode { get; set; }

        /// <summary>
        /// Max random offset for X tile pos
        /// </summary>
        [DataMember(Name = "tileRandomXMax")]
        public int TileRandomXMax { get; set; }

        /// <summary>
        /// Min random offset for X tile pos
        /// </summary>
        [DataMember(Name = "tileRandomXMin")]
        public int TileRandomXMin { get; set; }

        /// <summary>
        /// Max random offset for Y tile pos
        /// </summary>
        [DataMember(Name = "tileRandomYMax")]
        public int TileRandomYMax { get; set; }

        /// <summary>
        /// Min random offset for Y tile pos
        /// </summary>
        [DataMember(Name = "tileRandomYMin")]
        public int TileRandomYMin { get; set; }

        /// <summary>
        /// Array containing all the possible tile IDs rectangles (picked randomly).
        /// </summary>
        [DataMember(Name = "tileRectsIds")]
        public int[][] TileRectsIds { get; set; }

        /// <summary>
        /// Tile X offset
        /// </summary>
        [DataMember(Name = "tileXOffset")]
        public int TileXOffset { get; set; }

        /// <summary>
        /// Tile Y offset
        /// </summary>
        [DataMember(Name = "tileYOffset")]
        public int TileYOffset { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [DataMember(Name = "uid")]
        public int Uid { get; set; }

        /// <summary>
        /// X cell coord modulo
        /// </summary>
        [DataMember(Name = "xModulo")]
        public int XModulo { get; set; }

        /// <summary>
        /// X cell start offset
        /// </summary>
        [DataMember(Name = "xOffset")]
        public int XOffset { get; set; }

        /// <summary>
        /// Y cell coord modulo
        /// </summary>
        [DataMember(Name = "yModulo")]
        public int YModulo { get; set; }

        /// <summary>
        /// Y cell start offset
        /// </summary>
        [DataMember(Name = "yOffset")]
        public int YOffset { get; set; }
    }
}