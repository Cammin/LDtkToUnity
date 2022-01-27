using Newtonsoft.Json;

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
        /// User defined unique identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Unique instance identifer
        /// </summary>
        [JsonProperty("iid")]
        public string Iid { get; set; }

        /// <summary>
        /// All levels from this world. The order of this array is only relevant in
        /// `LinearHorizontal` and `linearVertical` world layouts (see `worldLayout` value).
        /// Otherwise, you should refer to the `worldX`,`worldY` coordinates of each Level.
        /// </summary>
        [JsonProperty("levels")]
        public Level[] Levels { get; set; }

        /// <summary>
        /// Height of the world grid in pixels.
        /// </summary>
        [JsonProperty("worldGridHeight")]
        public long WorldGridHeight { get; set; }

        /// <summary>
        /// Width of the world grid in pixels.
        /// </summary>
        [JsonProperty("worldGridWidth")]
        public long WorldGridWidth { get; set; }

        /// <summary>
        /// An enum that describes how levels are organized in this project (ie. linearly or in a 2D
        /// space). Possible values: `Free`, `GridVania`, `LinearHorizontal`, `LinearVertical`
        /// </summary>
        [JsonProperty("worldLayout")]
        public WorldLayout WorldLayout { get; set; }
    }
}