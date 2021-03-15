using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

namespace LDtkUnity
{
    public class LDtkBuilderIntGridValue : LDtkLayerBuilder
    {
        public LDtkBuilderIntGridValue(LayerInstance layer, LDtkProject project) : base(layer, project)
        {
        }
        
        public void BuildIntGridValues(Tilemap tilemap)
        {
            int[] intGridValues = Layer.IntGridCsv.Select(p => (int) p).ToArray();

            for (int i = 0; i < intGridValues.Length; i++)
            {
                int intGridValue = intGridValues[i];
                
                //all empty intgrid values are 0
                if (intGridValue == 0)
                {
                    continue;
                }

                LayerDefinition intGridDef = Layer.Definition;
                IntGridValueDefinition definition = intGridDef.IntGridValues[intGridValue-1];

                string intGridValueKey = LDtkIntGridKeyFormat.GetKeyFormat(intGridDef, definition);
                Tile intGridTile = Project.GetIntGridValue(intGridValueKey);

                if (intGridTile == null)
                {
                    continue;
                }

                BuildIntGridValue(definition, i, intGridTile, tilemap);
            }

            TryTurnOffRenderer(tilemap);
            
            LDtkEditorUtil.Dirty(tilemap);
        }

        private void TryTurnOffRenderer(Tilemap tilemap)
        {
            if (Project.IntGridValueColorsVisible)
            {
                return;
            }

            TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }

        private void BuildIntGridValue(IntGridValueDefinition definition, int intValueData, Tile tileAsset, Tilemap tilemap)
        {
            Vector2Int cellCoord = LDtkToolOriginCoordConverter.IntGridValueCsvCoord(intValueData, Layer.UnityCellSize);
            Vector2 coord = LDtkToolOriginCoordConverter.ConvertCell(cellCoord, (int)Layer.CHei);
            
            Vector3Int c = new Vector3Int((int)coord.x, (int)coord.y, 0);
            
            //todo this color application may not actually happen due to not dirtying the original tile asset
            tilemap.SetColor(c, definition.UnityColor);
            tilemap.SetTile(c, tileAsset);
        }

        
    }
}