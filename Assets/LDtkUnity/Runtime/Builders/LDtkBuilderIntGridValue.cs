using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets.IntGridValue;
using LDtkUnity.Runtime.UnityAssets.Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.Builders
{
    public static class LDtkBuilderIntGridValue
    {
        public static void BuildIntGridValues(LDtkDataLayer layer, LDtkProject project, Tilemap tilemap)
        {
            foreach (LDtkDataIntGridValue intGridValue in layer.intGrid)
            {
                LDtkDefinitionIntGridValue definition = layer.Definition.intGridValues[intGridValue.v];
                LDtkIntGridValueAsset asset = project.GetIntGridValue(definition.identifier);
                
                if (asset == null) continue;
                
                BuildIntGridValue(layer, definition, intGridValue, asset, tilemap);
            }

            TryTurnOffRenderer(project, tilemap);
        }

        private static void TryTurnOffRenderer(LDtkProject project, Tilemap tilemap)
        {
            if (project.IntGridValueColorsVisible) return;

            TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }

        private static void BuildIntGridValue(LDtkDataLayer layer, LDtkDefinitionIntGridValue definition, LDtkDataIntGridValue intValueData, LDtkIntGridValueAsset asset, Tilemap tilemap)
        {
            Vector2Int cellCoord = LDtkToolOriginCoordConverter.GetTopLeftOriginCellCoordFromCoordID(intValueData.coordId, layer.__cWid);
            Vector2 coord = LDtkToolOriginCoordConverter.ConvertCell(cellCoord, layer.__cHei);
            Tile tileAsset = LDtkProviderTileBasicColor.GetTile(asset, definition.Color);

            Vector3Int c = new Vector3Int((int)coord.x, (int)coord.y, 0);
            tilemap.SetTile(c, tileAsset);
        }
    }
}