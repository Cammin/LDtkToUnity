using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class AutoLayerRuleGroup
    {
        [DataMember(Name = "active")]
        public bool Active { get; set; }

        [DataMember(Name = "biomeRequirementMode")]
        public int BiomeRequirementMode { get; set; }

        /// <summary>
        /// *This field was removed in 1.0.0 and should no longer be used.*
        /// </summary>
        [DataMember(Name = "collapsed")]
        public bool? Collapsed { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }

        [DataMember(Name = "icon")]
        public TilesetRectangle Icon { get; set; }

        [DataMember(Name = "isOptional")]
        public bool IsOptional { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "requiredBiomeValues")]
        public string[] RequiredBiomeValues { get; set; }

        [DataMember(Name = "rules")]
        public AutoLayerRuleDefinition[] Rules { get; set; }

        [DataMember(Name = "uid")]
        public int Uid { get; set; }

        [DataMember(Name = "usesWizard")]
        public bool UsesWizard { get; set; }
    }
}