using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// This object is used in Field Instances to describe an EntityRef value.
    /// </summary>
    public partial class FieldInstanceEntityReference
    {
        /// <summary>
        /// IID of the refered EntityInstance
        /// </summary>
        [DataMember(Name = "entityIid")]
        public string EntityIid { get; set; }

        /// <summary>
        /// IID of the LayerInstance containing the refered EntityInstance
        /// </summary>
        [DataMember(Name = "layerIid")]
        public string LayerIid { get; set; }

        /// <summary>
        /// IID of the Level containing the refered EntityInstance
        /// </summary>
        [DataMember(Name = "levelIid")]
        public string LevelIid { get; set; }

        /// <summary>
        /// IID of the World containing the refered EntityInstance
        /// </summary>
        [DataMember(Name = "worldIid")]
        public string WorldIid { get; set; }
    }
}