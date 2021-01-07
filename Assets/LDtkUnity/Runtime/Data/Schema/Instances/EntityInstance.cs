using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDtkUnity.Data
{
    public class EntityInstance : ILDtkIdentifier
    {
        /// <summary>
        /// Grid-based coordinates (`[x,y]` format)
        /// </summary>
        [JsonProperty("__grid")]
        public long[] Grid { get; set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty("__identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Optional Tile used to display this entity (it could either be the default Entity tile, or
        /// some tile provided by a field value, like an Enum).
        /// </summary>
        [JsonProperty("__tile")]
        public Dictionary<string, dynamic> Tile { get; set; }

        /// <summary>
        /// Reference of the **Entity definition** UID
        /// </summary>
        [JsonProperty("defUid")]
        public long DefUid { get; set; }

        [JsonProperty("fieldInstances")]
        public FieldInstance[] FieldInstances { get; set; }

        /// <summary>
        /// Pixel coordinates (`[x,y]` format). Don't forget optional layer offsets, if they exist!
        /// </summary>
        [JsonProperty("px")]
        public long[] Px { get; set; }
    }
}