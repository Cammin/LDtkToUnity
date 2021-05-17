using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
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

            GameObject tilemapGameObject = LayerGameObject.CreateChildGameObject(Layer.Type);
            
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
                LDtkIntGridTile intGridTile = Importer.GetIntGridValueTile(intGridValueKey);

                if (intGridTile == null)
                {
                    intGridTile = LDtkResourcesLoader.LoadDefaultTile();
                }

                BuildIntGridValue(intGridValueDef, i, intGridTile, _tilemap);
            }
            
            _tilemap.SetOpacity(Layer);
        }

        private void BuildIntGridValue(IntGridValueDefinition definition, int intValueData, LDtkIntGridTile tileAsset, Tilemap tilemap)
        {
            Vector2Int cellCoord = LDtkToolOriginCoordConverter.IntGridValueCsvCoord(intValueData, Layer.UnityCellSize);
            Vector2 coord = LDtkToolOriginCoordConverter.ConvertCell(cellCoord, (int)Layer.CHei);
            
            Vector3Int cell = new Vector3Int((int)coord.x, (int)coord.y, 0);

            tilemap.SetTile(cell, tileAsset);
            tilemap.SetColor(cell, definition.UnityColor);
        }
    }
}