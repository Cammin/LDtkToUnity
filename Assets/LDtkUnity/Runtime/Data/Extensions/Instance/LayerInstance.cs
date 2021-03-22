using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LayerInstance : ILDtkIdentifier
    {
        /// <summary>
        /// Reference of this instance's definition.
        /// </summary>
        public LayerDefinition Definition => LDtkProviderUid.GetUidData<LayerDefinition>(LayerDefUid);
        
        /// <summary>
        /// The definition of corresponding Tileset, if any.
        /// </summary>
        public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkProviderUid.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;

        /// <summary>
        /// Reference to the level containing this layer instance
        /// </summary>
        public Level LevelReference => LDtkProviderUid.GetUidData<Level>(LevelId);
        
        /// <summary>
        /// Returns true if this layer is an IntGrid layer.
        /// </summary>
        public bool IsIntGridLayer => !IntGridCsv.NullOrEmpty();
        
        /// <summary>
        /// Returns true if this layer is an Entities layer.
        /// </summary>
        public bool IsEntitiesLayer => !EntityInstances.NullOrEmpty();
        
        /// <summary>
        /// Returns true if this layer is a Tiles layer.
        /// </summary>
        public bool IsTilesLayer => !GridTiles.NullOrEmpty();
        
        /// <summary>
        /// Returns true if this layer is an Auto Layer.
        /// </summary>
        public bool IsAutoLayer => !AutoLayerTiles.NullOrEmpty();
        
        /// <summary>
        /// Grid-based size
        /// </summary>
        public Vector2Int UnityCellSize => new Vector2Int((int)CWid, (int)CHei);
        
        /// <summary>
        /// Total layer pixel offset, including both instance and definition offsets.
        /// </summary>
        public Vector2Int UnityPxTotalOffset => new Vector2Int((int)PxTotalOffsetX, (int)PxTotalOffsetY);
        
        /// <summary>
        /// Offset in pixels to render this layer, usually 0,0
        /// </summary>
        public Vector2Int UnityPxOffset => new Vector2Int((int)PxOffsetX, (int)PxOffsetY);
        
        /// <summary>
        /// A special Vector2 position that solves where the layer's position should be in Unity's world space based off of LDtk's top-left origin
        /// </summary>
        public Vector2 UnityWorldPosition => LevelReference.UnityWorldSpaceCoord((int)GridSize);

        /// <summary>
        /// Total count of IntGrid values that are not empty spaces.
        /// </summary>
        public int IntGridValueCount => IntGridCsv.Count(value => value != 0);
    }
}