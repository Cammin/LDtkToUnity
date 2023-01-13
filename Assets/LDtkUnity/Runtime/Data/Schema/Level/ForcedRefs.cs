using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// This object is not actually used by LDtk. It ONLY exists to force explicit references to
    /// all types, to make sure QuickType finds them and integrate all of them. Otherwise,
    /// Quicktype will drop types that are not explicitely used.
    /// </summary>
    public partial class ForcedRefs
    {
        [JsonProperty("AutoLayerRuleGroup", NullValueHandling = NullValueHandling.Ignore)]
        public AutoLayerRuleGroup AutoLayerRuleGroup { get; set; }

        [JsonProperty("AutoRuleDef", NullValueHandling = NullValueHandling.Ignore)]
        public AutoLayerRuleDefinition AutoRuleDef { get; set; }

        [JsonProperty("CustomCommand", NullValueHandling = NullValueHandling.Ignore)]
        public LdtkCustomCommand CustomCommand { get; set; }

        [JsonProperty("Definitions", NullValueHandling = NullValueHandling.Ignore)]
        public Definitions Definitions { get; set; }

        [JsonProperty("EntityDef", NullValueHandling = NullValueHandling.Ignore)]
        public EntityDefinition EntityDef { get; set; }

        [JsonProperty("EntityInstance", NullValueHandling = NullValueHandling.Ignore)]
        public EntityInstance EntityInstance { get; set; }

        [JsonProperty("EntityReferenceInfos", NullValueHandling = NullValueHandling.Ignore)]
        public ReferenceToAnEntityInstance EntityReferenceInfos { get; set; }

        [JsonProperty("EnumDef", NullValueHandling = NullValueHandling.Ignore)]
        public EnumDefinition EnumDef { get; set; }

        [JsonProperty("EnumDefValues", NullValueHandling = NullValueHandling.Ignore)]
        public EnumValueDefinition EnumDefValues { get; set; }

        [JsonProperty("EnumTagValue", NullValueHandling = NullValueHandling.Ignore)]
        public EnumTagValue EnumTagValue { get; set; }

        [JsonProperty("FieldDef", NullValueHandling = NullValueHandling.Ignore)]
        public FieldDefinition FieldDef { get; set; }

        [JsonProperty("FieldInstance", NullValueHandling = NullValueHandling.Ignore)]
        public FieldInstance FieldInstance { get; set; }

        [JsonProperty("GridPoint", NullValueHandling = NullValueHandling.Ignore)]
        public GridPoint GridPoint { get; set; }

        [JsonProperty("IntGridValueDef", NullValueHandling = NullValueHandling.Ignore)]
        public IntGridValueDefinition IntGridValueDef { get; set; }

        [JsonProperty("IntGridValueInstance", NullValueHandling = NullValueHandling.Ignore)]
        public IntGridValueInstance IntGridValueInstance { get; set; }

        [JsonProperty("LayerDef", NullValueHandling = NullValueHandling.Ignore)]
        public LayerDefinition LayerDef { get; set; }

        [JsonProperty("LayerInstance", NullValueHandling = NullValueHandling.Ignore)]
        public LayerInstance LayerInstance { get; set; }

        [JsonProperty("Level", NullValueHandling = NullValueHandling.Ignore)]
        public Level Level { get; set; }

        [JsonProperty("LevelBgPosInfos", NullValueHandling = NullValueHandling.Ignore)]
        public LevelBackgroundPosition LevelBgPosInfos { get; set; }

        [JsonProperty("NeighbourLevel", NullValueHandling = NullValueHandling.Ignore)]
        public NeighbourLevel NeighbourLevel { get; set; }

        [JsonProperty("TableOfContentEntry", NullValueHandling = NullValueHandling.Ignore)]
        public LdtkTableOfContentEntry TableOfContentEntry { get; set; }

        [JsonProperty("Tile", NullValueHandling = NullValueHandling.Ignore)]
        public TileInstance Tile { get; set; }

        [JsonProperty("TileCustomMetadata", NullValueHandling = NullValueHandling.Ignore)]
        public TileCustomMetadata TileCustomMetadata { get; set; }

        [JsonProperty("TilesetDef", NullValueHandling = NullValueHandling.Ignore)]
        public TilesetDefinition TilesetDef { get; set; }

        [JsonProperty("TilesetRect", NullValueHandling = NullValueHandling.Ignore)]
        public TilesetRectangle TilesetRect { get; set; }

        [JsonProperty("World", NullValueHandling = NullValueHandling.Ignore)]
        public World World { get; set; }
    }
}