using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LayerInstance : ILDtkIdentifier
    {
        public LayerDefinition Definition => LDtkProviderUid.GetUidData<LayerDefinition>(LayerDefUid);
        public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkProviderUid.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;

        public Level LevelReference => LDtkProviderUid.GetUidData<Level>(LevelId);
        
        public bool IsIntGridLayer => !IntGridCsv.NullOrEmpty();
        public bool IsAutoLayer => !AutoLayerTiles.NullOrEmpty();
        public bool IsTilesLayer => !GridTiles.NullOrEmpty();
        public bool IsEntitiesLayer => !EntityInstances.NullOrEmpty();

        public Vector2Int UnityCellSize => new Vector2Int((int)CWid, (int)CHei);
        public Vector2Int UnityPxTotalOffset => new Vector2Int((int)PxTotalOffsetX, (int)PxTotalOffsetY);
        public Vector2Int UnityPxOffset => new Vector2Int((int)PxOffsetX, (int)PxOffsetY);
        
        public Vector2 UnityWorldPosition => LevelReference.UnityWorldSpaceCoord((int)GridSize);

        public int IntGridValueCount => IntGridCsv.Count(p => p != 0);
    }
}