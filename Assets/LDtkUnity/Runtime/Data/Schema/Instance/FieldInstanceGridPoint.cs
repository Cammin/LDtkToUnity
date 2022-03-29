using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// This object is just a grid-based coordinate used in Field values.
    /// </summary>
    public partial class FieldInstanceGridPoint
    {
        /// <summary>
        /// X grid-based coordinate
        /// </summary>
        [JsonProperty("cx")]
        public long Cx { get; set; }

        /// <summary>
        /// Y grid-based coordinate
        /// </summary>
        [JsonProperty("cy")]
        public long Cy { get; set; }
    }
}