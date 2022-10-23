using System.Text.Json.Serialization;

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
        [JsonPropertyName("entityIid")]
        public string EntityIid { get; set; }

        /// <summary>
        /// IID of the LayerInstance containing the refered EntityInstance
        /// </summary>
        [JsonPropertyName("layerIid")]
        public string LayerIid { get; set; }

        /// <summary>
        /// IID of the Level containing the refered EntityInstance
        /// </summary>
        [JsonPropertyName("levelIid")]
        public string LevelIid { get; set; }

        /// <summary>
        /// IID of the World containing the refered EntityInstance
        /// </summary>
        [JsonPropertyName("worldIid")]
        public string WorldIid { get; set; }
    }
}