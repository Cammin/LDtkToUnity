using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Instance Data
    /// </summary>
    public partial class LayerInstance : ILDtkIdentifier, ILDtkIid
    {
        /// <value>
        /// Reference of this instance's definition. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public LayerDefinition Definition => LDtkUidBank.GetUidData<LayerDefinition>(LayerDefUid);
        
        /// <value>
        /// The definition of corresponding Tileset, if any. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;

        /// <value>
        /// This layer can use another tileset by overriding the tileset here. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public TilesetDefinition OverrideTilesetDefinition => OverrideTilesetUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(OverrideTilesetUid.Value) : null;

        /// <value>
        /// Reference to the level containing this layer instance. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public Level LevelReference => LDtkUidBank.GetUidData<Level>(LevelId);
        
        /// <value>
        /// Returns true if this layer contains IntGrid values.
        /// </value>
        [IgnoreDataMember] public bool IsIntGridLayer => IntGridValueCount > 0;
        
        /// <value>
        /// Returns true if this layer contains Entities.
        /// </value>
        [IgnoreDataMember] public bool IsEntitiesLayer => !EntityInstances.IsNullOrEmpty();
        
        /// <value>
        /// Returns true if this layer contains Grid tiles.
        /// </value>
        [IgnoreDataMember] public bool IsTilesLayer => !GridTiles.IsNullOrEmpty();
        
        /// <value>
        /// Returns true if this layer contains AutoLayer tiles.
        /// </value>
        [IgnoreDataMember] public bool IsAutoLayer => !AutoLayerTiles.IsNullOrEmpty();
        
        /// <value>
        /// Returns true if this particular layer instance has no populated data.
        /// </value>
        [IgnoreDataMember] public bool IsDeadWeight => !IsIntGridLayer && !IsEntitiesLayer && !IsTilesLayer && !IsAutoLayer;
        
        /// <value>
        /// Grid-based size
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityCellSize => new Vector2Int(CWid, CHei);
        
        /// <value>
        /// Total layer pixel offset, including both instance and definition offsets.
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityPxTotalOffset => new Vector2Int(PxTotalOffsetX, PxTotalOffsetY);
        
        /// <value>
        /// Total layer world-space offset, including both instance and definition offsets.
        /// </value>
        [IgnoreDataMember] public Vector2 UnityWorldTotalOffset => new Vector2((float)PxTotalOffsetX/GridSize, -(float)PxTotalOffsetY/GridSize);

        /// <value>
        /// Offset in pixels to render this layer, usually 0,0
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityPxOffset => new Vector2Int(PxOffsetX, PxOffsetY);
        
        /// <value>
        /// Offset in world space to render this layer, usually 0,0
        /// </value>
        [IgnoreDataMember] public Vector2 UnityWorldOffset => new Vector2((float)PxOffsetX/GridSize, -(float)PxOffsetY/GridSize);

        /// <value>
        /// Total count of IntGrid values that are not empty spaces.
        /// </value>
        [IgnoreDataMember] public int IntGridValueCount => IntGridCsv.Count(value => value != 0);
    }
}