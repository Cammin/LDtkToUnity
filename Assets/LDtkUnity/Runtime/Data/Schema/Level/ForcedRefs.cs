using System.Text.Json.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// This object is not actually used by LDtk. It ONLY exists to force explicit references to
    /// all types, to make sure QuickType finds them and integrate all of them. Otherwise,
    /// Quicktype will drop types that are not explicitely used.
    /// </summary>
    public partial class ForcedRefs
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("AutoLayerRuleGroup")]
        public AutoLayerRuleGroup AutoLayerRuleGroup { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("AutoRuleDef")]
        public AutoLayerRuleDefinition AutoRuleDef { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Definitions")]
        public Definitions Definitions { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("EntityDef")]
        public EntityDefinition EntityDef { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("EntityInstance")]
        public EntityInstance EntityInstance { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("EntityReferenceInfos")]
        public FieldInstanceEntityReference EntityReferenceInfos { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("EnumDef")]
        public EnumDefinition EnumDef { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("EnumDefValues")]
        public EnumValueDefinition EnumDefValues { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("EnumTagValue")]
        public EnumTagValue EnumTagValue { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("FieldDef")]
        public FieldDefinition FieldDef { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("FieldInstance")]
        public FieldInstance FieldInstance { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("GridPoint")]
        public FieldInstanceGridPoint GridPoint { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("IntGridValueDef")]
        public IntGridValueDefinition IntGridValueDef { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("IntGridValueInstance")]
        public IntGridValueInstance IntGridValueInstance { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("LayerDef")]
        public LayerDefinition LayerDef { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("LayerInstance")]
        public LayerInstance LayerInstance { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Level")]
        public Level Level { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("LevelBgPosInfos")]
        public LevelBackgroundPosition LevelBgPosInfos { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("NeighbourLevel")]
        public NeighbourLevel NeighbourLevel { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Tile")]
        public TileInstance Tile { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("TileCustomMetadata")]
        public TileCustomMetadata TileCustomMetadata { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("TilesetDef")]
        public TilesetDefinition TilesetDef { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("TilesetRect")]
        public TilesetRectangle TilesetRect { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("World")]
        public World World { get; set; }
    }
}