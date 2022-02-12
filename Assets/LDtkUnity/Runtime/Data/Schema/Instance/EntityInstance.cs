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
        /// Array of tags defined in this Entity definition
        /// </summary>
        [JsonProperty("__tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Optional TilesetRect used to display this entity (it could either be the default Entity
        /// tile, or some tile provided by a field value, like an Enum).
        /// </summary>
        [JsonProperty("__tile")]
        public TilesetRectangle Tile { get; set; }

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
        /// Unique instance identifier
        /// </summary>
        [JsonProperty("iid")]
        public string Iid { get; set; }

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