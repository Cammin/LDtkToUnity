using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// This structure represents a single tile from a given Tileset.
    /// </summary>
    public partial class TileInstance
    {
        /// <summary>
        /// Alpha/opacity of the tile (0-1, defaults to 1)
        /// </summary>
        [DataMember(Name = "a")]
        public float A { get; set; }

        /// <summary>
        /// Internal data used by the editor.<br/>  For auto-layer tiles: `[ruleId, coordId]`.<br/>
        /// For tile-layer tiles: `[coordId]`.
        /// </summary>
        [DataMember(Name = "d")]
        public int[] D { get; set; }

        /// <summary>
        /// "Flip bits", a 2-bits integer to represent the mirror transformations of the tile.<br/>
        /// - Bit 0 = X flip<br/>   - Bit 1 = Y flip<br/>   Examples: f=0 (no flip), f=1 (X flip
        /// only), f=2 (Y flip only), f=3 (both flips)
        /// </summary>
        [DataMember(Name = "f")]
        public int F { get; set; }

        /// <summary>
        /// Pixel coordinates of the tile in the **layer** (`[x,y]` format). Don't forget optional
        /// layer offsets, if they exist!
        /// </summary>
        [DataMember(Name = "px")]
        public int[] Px { get; set; }

        /// <summary>
        /// Pixel coordinates of the tile in the **tileset** (`[x,y]` format)
        /// </summary>
        [DataMember(Name = "src")]
        public int[] Src { get; set; }

        /// <summary>
        /// The *Tile ID* in the corresponding tileset.
        /// </summary>
        [DataMember(Name = "t")]
        public int T { get; set; }
    }
}