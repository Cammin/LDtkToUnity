using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// This object represents a custom sub rectangle in a Tileset image.
    /// </summary>
    public partial class TilesetRectangle
    {
        /// <summary>
        /// Height in pixels
        /// </summary>
        [DataMember(Name = "h")]
        public int H { get; set; }

        /// <summary>
        /// UID of the tileset
        /// </summary>
        [DataMember(Name = "tilesetUid")]
        public int TilesetUid { get; set; }

        /// <summary>
        /// Width in pixels
        /// </summary>
        [DataMember(Name = "w")]
        public int W { get; set; }

        /// <summary>
        /// X pixels coordinate of the top-left corner in the Tileset image
        /// </summary>
        [DataMember(Name = "x")]
        public int X { get; set; }

        /// <summary>
        /// Y pixels coordinate of the top-left corner in the Tileset image
        /// </summary>
        [DataMember(Name = "y")]
        public int Y { get; set; }
    }
}