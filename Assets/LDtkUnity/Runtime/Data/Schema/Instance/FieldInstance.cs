using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class FieldInstance
    {
        /// <summary>
        /// Field definition identifier
        /// </summary>
        [JsonProperty("__identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Optional TilesetRect used to display this field (this can be the field own Tile, or some
        /// other Tile guessed from the value, like an Enum).
        /// </summary>
        [JsonProperty("__tile")]
        public TilesetRectangle Tile { get; set; }

        /// <summary>
        /// Type of the field, such as `Int`, `Float`, `String`, `Enum(my_enum_name)`, `Bool`,
        /// etc.<br/>  NOTE: if you enable the advanced option **Use Multilines type**, you will have
        /// "*Multilines*" instead of "*String*" when relevant.
        /// </summary>
        [JsonProperty("__type")]
        public string Type { get; set; }

        /// <summary>
        /// Actual value of the field instance. The value type varies, depending on `__type`:<br/>
        /// - For **classic types** (ie. Integer, Float, Boolean, String, Text and FilePath), you
        /// just get the actual value with the expected type.<br/>   - For **Color**, the value is an
        /// hexadecimal string using "#rrggbb" format.<br/>   - For **Enum**, the value is a String
        /// representing the selected enum value.<br/>   - For **Point**, the value is a
        /// [GridPoint](#ldtk-GridPoint) object.<br/>   - For **Tile**, the value is a
        /// [TilesetRect](#ldtk-TilesetRect) object.<br/>   - For **EntityRef**, the value is an
        /// [EntityReferenceInfos](#ldtk-EntityReferenceInfos) object.<br/><br/>  If the field is an
        /// array, then this `__value` will also be a JSON array.
        /// </summary>
        [JsonProperty("__value")]
        public object Value { get; set; }

        /// <summary>
        /// Reference of the **Field definition** UID
        /// </summary>
        [JsonProperty("defUid")]
        public long DefUid { get; set; }

        /// <summary>
        /// Editor internal raw values
        /// </summary>
        [JsonProperty("realEditorValues")]
        public object[] RealEditorValues { get; set; }
    }
}