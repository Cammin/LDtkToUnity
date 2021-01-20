using LDtkUnity.Data;
using LDtkUnity.Providers;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

namespace LDtkUnity.Builders
{
    public static class LDtkBuilderIntGridValue
    {
        public static void BuildIntGridValues(LayerInstance layer, LDtkProject project, Tilemap tilemap)
        {
            foreach (LdtkIntGridValueInstance intGridValue in layer.IntGrid)
            {
                LdtkIntGridValueDef definition = layer.Definition.IntGridValues[intGridValue.V];
                LDtkIntGridValueAsset asset = project.GetIntGridValue(definition.Identifier);
                
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

        private static void BuildIntGridValue(LayerInstance layer, LdtkIntGridValueDef definition, LdtkIntGridValueInstance intValueData, LDtkIntGridValueAsset asset, Tilemap tilemap)
        {
            Vector2Int cellCoord = LDtkToolOriginCoordConverter.IntGridCoordID((int)intValueData.CoordId, (int)layer.CWid);
            Vector2 coord = LDtkToolOriginCoordConverter.ConvertCell(cellCoord, (int)layer.CHei);
            Tile tileAsset = LDtkProviderTileBasicColor.GetTile(asset, definition.UnityColor);

            Vector3Int c = new Vector3Int((int)coord.x, (int)coord.y, 0);
            tilemap.SetTile(c, tileAsset);
        }
    }
}