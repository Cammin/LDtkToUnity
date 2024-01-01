using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// This object is not actually used by LDtk. It ONLY exists to force explicit references to
    /// all types, to make sure QuickType finds them and integrate all of them. Otherwise,
    /// Quicktype will drop types that are not explicitely used.
    /// </summary>
    public partial class ForcedRefs
    {
        [IgnoreDataMember]
        [DataMember(Name = "AutoLayerRuleGroup")]
        public AutoLayerRuleGroup AutoLayerRuleGroup { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "AutoRuleDef")]
        public AutoLayerRuleDefinition AutoRuleDef { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "CustomCommand")]
        public LdtkCustomCommand CustomCommand { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "Definitions")]
        public Definitions Definitions { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "EntityDef")]
        public EntityDefinition EntityDef { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "EntityInstance")]
        public EntityInstance EntityInstance { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "EntityReferenceInfos")]
        public ReferenceToAnEntityInstance EntityReferenceInfos { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "EnumDef")]
        public EnumDefinition EnumDef { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "EnumDefValues")]
        public EnumValueDefinition EnumDefValues { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "EnumTagValue")]
        public EnumTagValue EnumTagValue { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "FieldDef")]
        public FieldDefinition FieldDef { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "FieldInstance")]
        public FieldInstance FieldInstance { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "GridPoint")]
        public GridPoint GridPoint { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "IntGridValueDef")]
        public IntGridValueDefinition IntGridValueDef { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "IntGridValueGroupDef")]
        public IntGridValueGroupDefinition IntGridValueGroupDef { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "IntGridValueInstance")]
        public IntGridValueInstance IntGridValueInstance { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "LayerDef")]
        public LayerDefinition LayerDef { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "LayerInstance")]
        public LayerInstance LayerInstance { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "Level")]
        public Level Level { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "LevelBgPosInfos")]
        public LevelBackgroundPosition LevelBgPosInfos { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "NeighbourLevel")]
        public NeighbourLevel NeighbourLevel { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "TableOfContentEntry")]
        public LdtkTableOfContentEntry TableOfContentEntry { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "Tile")]
        public TileInstance Tile { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "TileCustomMetadata")]
        public TileCustomMetadata TileCustomMetadata { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "TilesetDef")]
        public TilesetDefinition TilesetDef { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "TilesetRect")]
        public TilesetRectangle TilesetRect { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "TocInstanceData")]
        public LdtkTocInstanceData TocInstanceData { get; set; }

        [IgnoreDataMember]
        [DataMember(Name = "World")]
        public World World { get; set; }
    }
}