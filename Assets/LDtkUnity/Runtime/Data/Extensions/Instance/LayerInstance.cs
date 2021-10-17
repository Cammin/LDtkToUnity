using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Instance Data
    /// </summary>
    public partial class LayerInstance : ILDtkIdentifier
    {
        /// <value>
        /// Reference of this instance's definition. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public LayerDefinition Definition => LDtkUidBank.GetUidData<LayerDefinition>(LayerDefUid);
        
        /// <value>
        /// The definition of corresponding Tileset, if any. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;

        /// <value>
        /// This layer can use another tileset by overriding the tileset here. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public TilesetDefinition OverrideTilesetDefinition => OverrideTilesetUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(OverrideTilesetUid.Value) : null;

        /// <value>
        /// Reference to the level containing this layer instance. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public Level LevelReference => LDtkUidBank.GetUidData<Level>(LevelId);
        
        /// <value>
        /// Returns true if this layer is an IntGrid layer.
        /// </value>
        [JsonIgnore] public bool IsIntGridLayer => IntGridValueCount > 0;
        
        /// <value>
        /// Returns true if this layer is an Entities layer.
        /// </value>
        [JsonIgnore] public bool IsEntitiesLayer => !EntityInstances.IsNullOrEmpty();
        
        /// <value>
        /// Returns true if this layer is a Tiles layer.
        /// </value>
        [JsonIgnore] public bool IsTilesLayer => !GridTiles.IsNullOrEmpty();
        
        /// <value>
        /// Returns true if this layer is an Auto Layer.
        /// </value>
        [JsonIgnore] public bool IsAutoLayer => !AutoLayerTiles.IsNullOrEmpty();
        
        /// <value>
        /// Grid-based size
        /// </value>
        [JsonIgnore] public Vector2Int UnityCellSize => new Vector2Int((int)CWid, (int)CHei);
        
        /// <value>
        /// Total layer pixel offset, including both instance and definition offsets.
        /// </value>
        [JsonIgnore] public Vector2Int UnityPxTotalOffset => new Vector2Int((int)PxTotalOffsetX, (int)PxTotalOffsetY);
        
        /// <value>
        /// Total layer world-space offset, including both instance and definition offsets.
        /// </value>
        [JsonIgnore] public Vector2 UnityWorldTotalOffset => new Vector2((float)PxTotalOffsetX/GridSize, -(float)PxTotalOffsetY/GridSize);

        /// <value>
        /// Offset in pixels to render this layer, usually 0,0
        /// </value>
        [JsonIgnore] public Vector2Int UnityPxOffset => new Vector2Int((int)PxOffsetX, (int)PxOffsetY);
        
        /// <value>
        /// Offset in world space to render this layer, usually 0,0
        /// </value>
        [JsonIgnore] public Vector2 UnityWorldOffset => new Vector2((float)PxOffsetX/GridSize, -(float)PxOffsetY/GridSize);

        /// <value>
        /// Total count of IntGrid values that are not empty spaces.
        /// </value>
        [JsonIgnore] public int IntGridValueCount => IntGridCsv.Count(value => value != 0);
    }
}