using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class LayerDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <value>
        /// Reference to the AutoLayer source definition. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public LayerDefinition AutoSourceLayerDefinition => AutoSourceLayerDefUid != null ? LDtkUidBank.GetUidData<LayerDefinition>(AutoSourceLayerDefUid.Value) : null;

        /// <value>
        /// Reference to the tileset definition being used by this auto-layer rules. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public TilesetDefinition AutoTilesetDefinition => AutoTilesetDefUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(AutoTilesetDefUid.Value) : null;

        /// <value>
        /// Reference to the tileset definition being used by this Tile layer. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;
        
        /// <value>
        /// Returns true if this layer is an IntGrid layer.
        /// </value>
        [JsonIgnore] public bool IsIntGridLayer => LayerDefinitionType == TypeEnum.IntGrid;
        
        /// <value>
        /// Returns true if this layer is an Entities layer.
        /// </value>
        [JsonIgnore] public bool IsEntitiesLayer => LayerDefinitionType == TypeEnum.Entities;
        
        /// <value>
        /// Returns true if this layer is a Tiles layer.
        /// </value>
        [JsonIgnore] public bool IsTilesLayer => LayerDefinitionType == TypeEnum.Tiles;
        
        /// <value>
        /// Returns true if this layer is an Auto Layer.
        /// </value>
        [JsonIgnore] public bool IsAutoLayer => LayerDefinitionType == TypeEnum.AutoLayer;
    }
}