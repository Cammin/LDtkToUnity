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
                Sprite spriteAsset = Project.GetIntGridValue(intGridValueKey);

                if (spriteAsset == null)
                {
                    continue;
                }

                BuildIntGridValue(definition, i, spriteAsset, tilemap);
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

        private void BuildIntGridValue(IntGridValueDefinition definition, int intValueData, Sprite spriteAsset, Tilemap tilemap)
        {
            Vector2Int cellCoord = LDtkToolOriginCoordConverter.IntGridValueCsvCoord(intValueData, Layer.UnityCellSize);
            Vector2 coord = LDtkToolOriginCoordConverter.ConvertCell(cellCoord, (int)Layer.CHei);

            //make the instance of this else where. we are making a new instance for each anyways so it's not optimized
            LDtkIntGridValueFactory factory = new LDtkIntGridValueFactory();

            Tile tileAsset = factory.GetTile(spriteAsset, definition.UnityColor);

            Vector3Int c = new Vector3Int((int)coord.x, (int)coord.y, 0);
            tilemap.SetTile(c, tileAsset);
        }

        
    }
}