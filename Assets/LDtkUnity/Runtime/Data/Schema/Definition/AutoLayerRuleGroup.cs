using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class AutoLayerRuleGroup
    {
        [JsonProperty("active")]
        public bool Active { get; set; }

        /// <summary>
        /// *This field was removed in 1.0.0 and should no longer be used.*
        /// </summary>
        [JsonProperty("collapsed")]
        public bool? Collapsed { get; set; }

        [JsonProperty("isOptional")]
        public bool IsOptional { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rules")]
        public AutoLayerRuleDefinition[] Rules { get; set; }

        [JsonProperty("uid")]
        public long Uid { get; set; }

        [JsonProperty("usesWizard")]
        public bool UsesWizard { get; set; }
    }
}