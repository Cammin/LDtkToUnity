using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets.IntGridValue;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.Builders
{
    public static class LDtkBuilderIntGridValue
    {
        public static void BuildIntGridValues(LDtkDataLayer layer, LDtkIntGridValueAssetCollection valueAssets, Tilemap tilemap)
        {
            foreach (LDtkDataIntGridValue intTile in layer.intGrid)
            {
                BuildIntGridValue(layer, intTile, valueAssets, tilemap);
            }

            TryTurnOffRenderer(valueAssets, tilemap);
        }

        private static void TryTurnOffRenderer(LDtkIntGridValueAssetCollection valueAssets, Tilemap tilemap)
        {
            if (valueAssets.CollisionTilesVisible) return;

            TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }

        private static void BuildIntGridValue(LDtkDataLayer layer, LDtkDataIntGridValue intValueData, LDtkIntGridValueAssetCollection valueAssets, Tilemap tilemap)
        {
            LDtkDefinitionIntGridValue definition = layer.Definition.intGridValues[intValueData.v];

            LDtkIntGridValueAsset asset = valueAssets.GetAssetByIdentifier(definition.identifier);
            if (asset == null) return;

            Vector2Int cellCoord = LDtkToolOriginCoordConverter.GetTopLeftOriginCellCoordFromCoordID(intValueData.coordId, layer.__cWid);
            Vector2 coord = LDtkToolOriginCoordConverter.ConvertCell(cellCoord, layer.__cHei);
            Tile tileAsset = LDtkProviderTileBasicColor.GetTile(asset, definition.Color);

            Vector3Int c = new Vector3Int((int)coord.x, (int)coord.y, 0);
            tilemap.SetTile(c, tileAsset);
        }
    }
}