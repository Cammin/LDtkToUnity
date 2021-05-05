using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

namespace LDtkUnity.Editor.Builders
{
    public class LDtkBuilderIntGridValue : LDtkLayerBuilder
    {
        private Tilemap _tilemap;
        

        public LDtkBuilderIntGridValue(LDtkProjectImporter importer, GameObject layerGameObject, LDtkSortingOrder sortingOrder) : base(importer, layerGameObject, sortingOrder)
        {
        }

        public void BuildIntGridValues()
        {
            SortingOrder.Next();

            GameObject tilemapGameObject = LayerGameObject.AddChild(Layer.Type);
            
            _tilemap = tilemapGameObject.AddComponent<Tilemap>();

            TilemapCollider2D collider = tilemapGameObject.AddComponent<TilemapCollider2D>();

            if (Importer.IntGridValueColorsVisible)
            {
                TilemapRenderer renderer = tilemapGameObject.AddComponent<TilemapRenderer>();
            }

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
                IntGridValueDefinition intGridValueDef = intGridDef.IntGridValues[intGridValue-1];

                string intGridValueKey = LDtkKeyFormatUtil.IntGridValueFormat(intGridDef, intGridValueDef);
                Tile intGridTile = null;//Importer.GetIntGridValue(intGridValueKey); //todo

                if (intGridTile == null)
                {
                    continue;
                }

                BuildIntGridValue(intGridValueDef, i, intGridTile, _tilemap);
            }

            TryTurnOffRenderer(_tilemap);
            
            _tilemap.SetOpacity(Layer);
        }

        private void TryTurnOffRenderer(Tilemap tilemap)
        {
            if (Importer.IntGridValueColorsVisible)
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