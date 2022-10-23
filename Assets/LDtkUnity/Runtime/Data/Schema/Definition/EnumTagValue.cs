using System.Text.Json.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// In a tileset definition, enum based tag infos
    /// </summary>
    public partial class EnumTagValue
    {
        [JsonPropertyName("enumValueId")]
        public string EnumValueId { get; set; }

        [JsonPropertyName("tileIds")]
        public long[] TileIds { get; set; }
    }
}