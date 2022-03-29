using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// In a tileset definition, enum based tag infos
    /// </summary>
    public partial class EnumTagValue
    {
        [JsonProperty("enumValueId")]
        public string EnumValueId { get; set; }

        [JsonProperty("tileIds")]
        public long[] TileIds { get; set; }
    }
}