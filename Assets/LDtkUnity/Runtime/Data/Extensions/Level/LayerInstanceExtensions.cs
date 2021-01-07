// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LayerInstanceExtensions
    {
        public static LayerDefinition Definition(this LayerInstance data) => LDtkProviderUid.GetUidData<LayerDefinition>(data.LayerDefUid);
        public static TilesetDefinition TilesetDefinition(this LayerInstance data)
        {
            if (data.TilesetDefUid != null)
            {
                return LDtkProviderUid.GetUidData<TilesetDefinition>(data.TilesetDefUid.Value);
            }

            return null;
        }

        public static Level LevelReference(this LayerInstance data) => LDtkProviderUid.GetUidData<Level>(data.LevelId);
        
        public static bool IsIntGridLayer(this LayerInstance data) => !data.IntGrid.NullOrEmpty();
        public static bool IsAutoTilesLayer(this LayerInstance data) => !data.AutoLayerTiles.NullOrEmpty();
        public static bool IsGridTilesLayer(this LayerInstance data) => !data.GridTiles.NullOrEmpty();
        public static bool IsEntityInstancesLayer(this LayerInstance data) => !data.EntityInstances.NullOrEmpty();

        public static Vector2 WorldAdjustedPosition(this LayerInstance data) => data.LevelReference().UnityWorldCoord((int)data.GridSize);
        public static Vector2Int CellSize(this LayerInstance data) => new Vector2Int((int)data.CWid, (int)data.CHei);
        public static Vector2Int PxTotalOffset(this LayerInstance data) => new Vector2Int((int)data.PxTotalOffsetX, (int)data.PxTotalOffsetY);
        public static Vector2Int PxOffset(this LayerInstance data) => new Vector2Int((int)data.PxOffsetX, (int)data.PxOffsetY);
        
        public static Bounds LayerUnitBounds(this LayerInstance data) => new Bounds((Vector2)data.CellSize() / 2, (Vector3Int)data.CellSize());
        
    }
}