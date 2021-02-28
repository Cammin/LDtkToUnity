using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class EntityInstance
    {
        /// <summary>
        /// Grid-based coordinates (`[x,y]` format)
        /// </summary>
        [JsonProperty("__grid")]
        public long[] Grid { get; set; }

        /// <summary>
        /// Entity definition identifier
        /// </summary>
        [JsonProperty("__identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Pivot coordinates  (`[x,y]` format, values are from 0 to 1) of the Entity
        /// </summary>
        [JsonProperty("__pivot")]
        public double[] Pivot { get; set; }

        /// <summary>
        /// Optional Tile used to display this entity (it could either be the default Entity tile, or
        /// some tile provided by a field value, like an Enum).
        /// </summary>
        [JsonProperty("__tile")]
        public EntityInstanceTile Tile { get; set; }

        /// <summary>
        /// Reference of the **Entity definition** UID
        /// </summary>
        [JsonProperty("defUid")]
        public long DefUid { get; set; }

        /// <summary>
        /// An array of all custom fields and their values.
        /// </summary>
        [JsonProperty("fieldInstances")]
        public FieldInstance[] FieldInstances { get; set; }

        /// <summary>
        /// Entity height in pixels. For non-resizable entities, it will be the same as Entity
        /// definition.
        /// </summary>
        [JsonProperty("height")]
        public long Height { get; set; }

        /// <summary>
        /// Pixel coordinates (`[x,y]` format) in current level coordinate space. Don't forget
        /// optional layer offsets, if they exist!
        /// </summary>
        [JsonProperty("px")]
        public long[] Px { get; set; }

        /// <summary>
        /// Entity width in pixels. For non-resizable entities, it will be the same as Entity
        /// definition.
        /// </summary>
        [JsonProperty("width")]
        public long Width { get; set; }
    }
}