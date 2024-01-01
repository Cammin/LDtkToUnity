using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class EntityInstance
    {
        /// <summary>
        /// Grid-based coordinates (`[x,y]` format)
        /// </summary>
        [DataMember(Name = "__grid")]
        public int[] Grid { get; set; }

        /// <summary>
        /// Entity definition identifier
        /// </summary>
        [DataMember(Name = "__identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Pivot coordinates  (`[x,y]` format, values are from 0 to 1) of the Entity
        /// </summary>
        [DataMember(Name = "__pivot")]
        public float[] Pivot { get; set; }

        /// <summary>
        /// The entity "smart" color, guessed from either Entity definition, or one its field
        /// instances.
        /// </summary>
        [DataMember(Name = "__smartColor")]
        public string SmartColor { get; set; }

        /// <summary>
        /// Array of tags defined in this Entity definition
        /// </summary>
        [DataMember(Name = "__tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Optional TilesetRect used to display this entity (it could either be the default Entity
        /// tile, or some tile provided by a field value, like an Enum).
        /// </summary>
        [DataMember(Name = "__tile")]
        public TilesetRectangle Tile { get; set; }

        /// <summary>
        /// X world coordinate in pixels. Only available in GridVania or Free world layouts.
        /// </summary>
        [DataMember(Name = "__worldX")]
        public int? WorldX { get; set; }

        /// <summary>
        /// Y world coordinate in pixels Only available in GridVania or Free world layouts.
        /// </summary>
        [DataMember(Name = "__worldY")]
        public int? WorldY { get; set; }

        /// <summary>
        /// Reference of the **Entity definition** UID
        /// </summary>
        [DataMember(Name = "defUid")]
        public int DefUid { get; set; }

        /// <summary>
        /// An array of all custom fields and their values.
        /// </summary>
        [DataMember(Name = "fieldInstances")]
        public FieldInstance[] FieldInstances { get; set; }

        /// <summary>
        /// Entity height in pixels. For non-resizable entities, it will be the same as Entity
        /// definition.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Unique instance identifier
        /// </summary>
        [DataMember(Name = "iid")]
        public string Iid { get; set; }

        /// <summary>
        /// Pixel coordinates (`[x,y]` format) in current level coordinate space. Don't forget
        /// optional layer offsets, if they exist!
        /// </summary>
        [DataMember(Name = "px")]
        public int[] Px { get; set; }

        /// <summary>
        /// Entity width in pixels. For non-resizable entities, it will be the same as Entity
        /// definition.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }
    }
}