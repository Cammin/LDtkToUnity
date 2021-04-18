namespace LDtkUnity
{
    public partial class LayerDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// 
        /// </summary>
        public LayerDefinition AutoSourceLayerDefinition => AutoSourceLayerDefUid != null ? LDtkUidBank.GetUidData<LayerDefinition>(AutoSourceLayerDefUid.Value) : null;

        /// <summary>
        /// Reference to the tileset definition being used by this auto-layer rules
        /// </summary>
        public TilesetDefinition AutoTilesetDefinition => AutoTilesetDefUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(AutoTilesetDefUid.Value) : null;

        /// <summary>
        /// Reference to the tileset definition being used by this Tile layer
        /// </summary>
        public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;
        
        /// <summary>
        /// Returns true if this layer is an IntGrid layer.
        /// </summary>
        public bool IsIntGridLayer => LayerDefinitionType == TypeEnum.IntGrid;
        
        /// <summary>
        /// Returns true if this layer is an Entities layer.
        /// </summary>
        public bool IsEntitiesLayer => LayerDefinitionType == TypeEnum.Entities;
        
        /// <summary>
        /// Returns true if this layer is a Tiles layer.
        /// </summary>
        public bool IsTilesLayer => LayerDefinitionType == TypeEnum.Tiles;
        
        /// <summary>
        /// Returns true if this layer is an Auto Layer.
        /// </summary>
        public bool IsAutoLayer => LayerDefinitionType == TypeEnum.AutoLayer;
    }
}