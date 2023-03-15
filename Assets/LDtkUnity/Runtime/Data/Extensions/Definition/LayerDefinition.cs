﻿using System.Runtime.Serialization;

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
        [IgnoreDataMember] public LayerDefinition AutoSourceLayerDefinition => AutoSourceLayerDefUid != null ? LDtkUidBank.GetUidData<LayerDefinition>(AutoSourceLayerDefUid.Value) : null;
        
        /// <value>
        /// Reference to the tileset definition being used by this Tile layer. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;
        
        /// <value>
        /// Returns true if this layer is an IntGrid layer.
        /// </value>
        [IgnoreDataMember] public bool IsIntGridLayer => LayerDefinitionType == TypeEnum.IntGrid;
        
        /// <value>
        /// Returns true if this layer is an Entities layer.
        /// </value>
        [IgnoreDataMember] public bool IsEntitiesLayer => LayerDefinitionType == TypeEnum.Entities;
        
        /// <value>
        /// Returns true if this layer is a Tiles layer.
        /// </value>
        [IgnoreDataMember] public bool IsTilesLayer => LayerDefinitionType == TypeEnum.Tiles;
        
        /// <value>
        /// Returns true if this layer is an Auto Layer.
        /// </value>
        [IgnoreDataMember] public bool IsAutoLayer => LayerDefinitionType == TypeEnum.AutoLayer;
    }
}