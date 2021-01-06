// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDataLayerExtensions
    {
        public static LDtkDefinitionLayer Definition(this LDtkDataLayer data) => LDtkProviderUid.GetUidData<LDtkDefinitionLayer>(data.layerDefUid);
        public static LDtkDefinitionTileset TilesetDefinition(this LDtkDataLayer data) => LDtkProviderUid.GetUidData<LDtkDefinitionTileset>(data.__tilesetDefUid);
        public static LDtkDataLevel LevelReference(this LDtkDataLayer data) => LDtkProviderUid.GetUidData<LDtkDataLevel>(data.levelId);
        
        public static bool IsIntGridLayer(this LDtkDataLayer data) => !data.intGrid.NullOrEmpty();
        public static bool IsAutoTilesLayer(this LDtkDataLayer data) => !data.autoLayerTiles.NullOrEmpty();
        public static bool IsGridTilesLayer(this LDtkDataLayer data) => !data.gridTiles.NullOrEmpty();
        public static bool IsEntityInstancesLayer(this LDtkDataLayer data) => !data.entityInstances.NullOrEmpty();

        public static Vector2 WorldAdjustedPosition(this LDtkDataLayer data) => data.LevelReference().UnityWorldCoord(data.__gridSize);
        public static Vector2Int CellSize(this LDtkDataLayer data) => new Vector2Int(data.__cWid, data.__cHei);
        public static Vector2Int PxTotalOffset(this LDtkDataLayer data) => new Vector2Int(data.__pxTotalOffsetX, data.__pxTotalOffsetY);
        public static Vector2Int PxOffset(this LDtkDataLayer data) => new Vector2Int(data.pxOffsetX, data.pxOffsetY);
        
        public static Bounds LayerUnitBounds(this LDtkDataLayer data) => new Bounds((Vector2)data.CellSize() / 2, (Vector3Int)data.CellSize());
        
    }
}