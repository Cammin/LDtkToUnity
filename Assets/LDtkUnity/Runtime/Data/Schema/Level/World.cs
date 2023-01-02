using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// **IMPORTANT**: this type is not used *yet* in current LDtk version. It's only presented
    /// here as a preview of a planned feature.  A World contains multiple levels, and it has its
    /// own layout settings.
    /// </summary>
    public partial class World
    {
        /// <summary>
        /// Default new level height
        /// </summary>
        [DataMember(Name = "defaultLevelHeight")]
        public long DefaultLevelHeight { get; set; }

        /// <summary>
        /// Default new level width
        /// </summary>
        [DataMember(Name = "defaultLevelWidth")]
        public long DefaultLevelWidth { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Unique instance identifer
        /// </summary>
        [DataMember(Name = "iid")]
        public string Iid { get; set; }

        /// <summary>
        /// All levels from this world. The order of this array is only relevant in
        /// `LinearHorizontal` and `linearVertical` world layouts (see `worldLayout` value).
        /// Otherwise, you should refer to the `worldX`,`worldY` coordinates of each Level.
        /// </summary>
        [DataMember(Name = "levels")]
        public Level[] Levels { get; set; }

        /// <summary>
        /// Height of the world grid in pixels.
        /// </summary>
        [DataMember(Name = "worldGridHeight")]
        public long WorldGridHeight { get; set; }

        /// <summary>
        /// Width of the world grid in pixels.
        /// </summary>
        [DataMember(Name = "worldGridWidth")]
        public long WorldGridWidth { get; set; }

        /// <summary>
        /// An enum that describes how levels are organized in this project (ie. linearly or in a 2D
        /// space). Possible values: `Free`, `GridVania`, `LinearHorizontal`, `LinearVertical`, `null`
        /// </summary>
        [DataMember(Name = "worldLayout")]
        public WorldLayout? WorldLayout { get; set; }
    }
}