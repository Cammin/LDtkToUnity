using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class LdtkTableOfContentEntry
    {
        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// **WARNING**: this deprecated value will be *removed* completely on version 1.7.0+
        /// Replaced by: `instancesData`
        /// </summary>
        [IgnoreDataMember]
        [DataMember(Name = "instances")]
        public ReferenceToAnEntityInstance[] Instances { get; set; }

        [DataMember(Name = "instancesData")]
        public LdtkTocInstanceData[] InstancesData { get; set; }
    }
}