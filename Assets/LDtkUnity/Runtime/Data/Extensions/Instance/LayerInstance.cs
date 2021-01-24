using LDtkUnity.Providers;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LayerInstance : ILDtkIdentifier
    {
        public LayerDefinition Definition => LDtkProviderUid.GetUidData<LayerDefinition>(LayerDefUid);
        public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkProviderUid.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;

        public Level LevelReference => LDtkProviderUid.GetUidData<Level>(LevelId);
        
        public bool IsIntGridLayer => !IntGrid.NullOrEmpty();
        public bool IsAutoTilesLayer => !AutoLayerTiles.NullOrEmpty();
        public bool IsGridTilesLayer => !GridTiles.NullOrEmpty();
        public bool IsEntityInstancesLayer => !EntityInstances.NullOrEmpty();

        public Vector2Int CellSize => new Vector2Int((int)CWid, (int)CHei);
        public Vector2Int PxTotalOffset => new Vector2Int((int)PxTotalOffsetX, (int)PxTotalOffsetY);
        public Vector2Int PxOffset => new Vector2Int((int)PxOffsetX, (int)PxOffsetY);
        
        public Vector2 UnityWorldPosition => LevelReference.UnityWorldSpaceCoord((int)GridSize);
    }
}