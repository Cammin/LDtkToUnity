using System.Linq;
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
        public static void BuildIntGridValues(LayerInstance layerInstance, LDtkProject project, Tilemap tilemap)
        {
            int[] intGridValues = layerInstance.IntGridCsv.Select(p => (int) p).ToArray();

            for (int i = 0; i < intGridValues.Length; i++)
            {
                int intGridValue = intGridValues[i];
                
                //all empty intgrid values are 0
                if (intGridValue == 0)
                {
                    continue;
                }

                LayerDefinition intGridDef = layerInstance.Definition;
                IntGridValueDefinition definition = intGridDef.IntGridValues[intGridValue-1];

                string intGridValueKey = LDtkIntGridKeyFormat.GetKeyFormat(intGridDef, definition);
                Sprite spriteAsset = project.GetIntGridValue(intGridValueKey);

                if (spriteAsset == null)
                {
                    continue;
                }

                BuildIntGridValue(layerInstance, definition, i, spriteAsset, tilemap);
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

        private static void BuildIntGridValue(LayerInstance layer, IntGridValueDefinition definition, int intValueData, Sprite spriteAsset, Tilemap tilemap)
        {
            Vector2Int cellCoord = LDtkToolOriginCoordConverter.IntGridValueCsvCoord(intValueData, layer.UnityCellSize);
            Vector2 coord = LDtkToolOriginCoordConverter.ConvertCell(cellCoord, (int)layer.CHei);

            //make the instance of this else where. we are making a new instance for each anyways so it's not optimized
            LDtkIntGridValueFactory factory = new LDtkIntGridValueFactory();

            Tile tileAsset = factory.GetTile(spriteAsset, definition.UnityColor);

            Vector3Int c = new Vector3Int((int)coord.x, (int)coord.y, 0);
            tilemap.SetTile(c, tileAsset);
        }
    }
}