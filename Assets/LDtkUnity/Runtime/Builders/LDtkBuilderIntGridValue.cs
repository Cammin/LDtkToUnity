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
            foreach (IntGridValueInstance intGridValue in layer.IntGrid)
            {
                IntGridValueDefinition definition = layer.Definition.IntGridValues[intGridValue.V];
                Sprite spriteAsset = project.GetIntGridValue(definition.Identifier);
                
                if (spriteAsset == null)
                {
                    continue;
                }
                
                BuildIntGridValue(layer, definition, intGridValue, spriteAsset, tilemap);
            }

            TryTurnOffRenderer(project, tilemap);
        }

        private static void TryTurnOffRenderer(LDtkProject project, Tilemap tilemap)
        {
            if (project.IntGridValueColorsVisible)
            {
                return;
            }

            TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }

        private static void BuildIntGridValue(LayerInstance layer, IntGridValueDefinition definition, IntGridValueInstance intValueData, Sprite spriteAsset, Tilemap tilemap)
        {
            Vector2Int cellCoord = intValueData.UnityCellCoord((int)layer.CWid);
            Vector2 coord = LDtkToolOriginCoordConverter.ConvertCell(cellCoord, (int)layer.CHei);
            Tile tileAsset = LDtkIntGridValueFactory.GetTile(spriteAsset, definition.UnityColor);

            Vector3Int c = new Vector3Int((int)coord.x, (int)coord.y, 0);
            tilemap.SetTile(c, tileAsset);
        }
    }
}