using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// This object describes the "location" of an Entity instance in the project worlds.
    /// </summary>
    public partial class ReferenceToAnEntityInstance
    {
        /// <summary>
        /// IID of the refered EntityInstance
        /// </summary>
        [JsonProperty("entityIid")]
        public string EntityIid { get; set; }

        /// <summary>
        /// IID of the LayerInstance containing the refered EntityInstance
        /// </summary>
        [JsonProperty("layerIid")]
        public string LayerIid { get; set; }

        /// <summary>
        /// IID of the Level containing the refered EntityInstance
        /// </summary>
        [JsonProperty("levelIid")]
        public string LevelIid { get; set; }

        /// <summary>
        /// IID of the World containing the refered EntityInstance
        /// </summary>
        [JsonProperty("worldIid")]
        public string WorldIid { get; set; }
    }
}