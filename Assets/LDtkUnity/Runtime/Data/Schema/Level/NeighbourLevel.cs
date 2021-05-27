using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class NeighbourLevel
    {
        /// <summary>
        /// A single lowercase character tipping on the level location (`n`orth, `s`outh, `w`est,
        /// `e`ast).
        /// </summary>
        [JsonProperty("dir")]
        public string Dir { get; set; }

        [JsonProperty("levelUid")]
        public long LevelUid { get; set; }
    }
}