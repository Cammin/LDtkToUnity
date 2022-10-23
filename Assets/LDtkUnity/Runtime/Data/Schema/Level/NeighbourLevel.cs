using System.Text.Json.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// Nearby level info
    /// </summary>
    public partial class NeighbourLevel
    {
        /// <summary>
        /// A single lowercase character tipping on the level location (`n`orth, `s`outh, `w`est,
        /// `e`ast).
        /// </summary>
        [JsonPropertyName("dir")]
        public string Dir { get; set; }

        /// <summary>
        /// Neighbour Instance Identifier
        /// </summary>
        [JsonPropertyName("levelIid")]
        public string LevelIid { get; set; }

        /// <summary>
        /// **WARNING**: this deprecated value will be *removed* completely on version 1.2.0+
        /// Replaced by: `levelIid`
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("levelUid")]
        public long? LevelUid { get; set; }
    }
}