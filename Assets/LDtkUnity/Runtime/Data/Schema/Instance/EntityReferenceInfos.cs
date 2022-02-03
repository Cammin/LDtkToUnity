using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// This object is used in Field Instances to describe an EntityRef value.
    /// </summary>
    public partial class EntityReferenceInfos
    {
        [JsonProperty("entityIid")]
        public string EntityIid { get; set; }

        [JsonProperty("layerIid")]
        public string LayerIid { get; set; }

        [JsonProperty("levelIid")]
        public string LevelIid { get; set; }
    }
}