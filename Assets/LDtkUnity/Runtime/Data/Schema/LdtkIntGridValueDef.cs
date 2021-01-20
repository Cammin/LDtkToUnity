using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class LdtkIntGridValueDef
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
    }
}