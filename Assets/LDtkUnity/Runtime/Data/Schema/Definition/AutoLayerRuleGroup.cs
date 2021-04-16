using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class AutoLayerRuleGroup
    {
        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("collapsed")]
        public bool Collapsed { get; set; }

        [JsonProperty("isOptional")]
        public bool IsOptional { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rules")]
        public AutoLayerRuleDefinition[] Rules { get; set; }

        [JsonProperty("uid")]
        public long Uid { get; set; }
    }
}