using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class LdtkTableOfContentEntry
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("instances")]
        public ReferenceToAnEntityInstance[] Instances { get; set; }
    }
}