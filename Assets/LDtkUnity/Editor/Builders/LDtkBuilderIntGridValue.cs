using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkBuilderIntGridValue : LDtkLayerBuilder
    {
        public Tilemap Tilemap { get; private set; }
        

        public LDtkBuilderIntGridValue(LDtkProjectImporter importer, GameObject layerGameObject, LDtkSortingOrder sortingOrder) : base(importer, layerGameObject, sortingOrder)
        {
        }

        public void BuildIntGridValues()
        {
            RoundTilemapPos();
            
            SortingOrder.Next();

            GameObject tilemapGameObject = LayerGameObject.CreateChildGameObject(Layer.Type);
            
            /*if (Importer.DeparentInRuntime)
            {
                tilemapGameObject.AddComponent<LDtkDetachChildren>();
            }*/
            
            Tilemap = tilemapGameObject.AddComponent<Tilemap>();


            if (Importer.IntGridValueColorsVisible)
            {
                TilemapRenderer renderer = tilemapGameObject.AddComponent<TilemapRenderer>();
                renderer.sortingOrder = SortingOrder.SortingOrderValue;
            }
            
            TilemapCollider2D collider = tilemapGameObject.AddComponent<TilemapCollider2D>();

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

                BuildIntGridValue(intGridValueDef, i, intGridTile);
            }
            
            Tilemap.SetOpacity(Layer);
        }



        private void BuildIntGridValue(IntGridValueDefinition definition, int intValueData, LDtkIntGridTile tileAsset)
        {
            Vector2Int cellCoord = LDtkCoordConverter.IntGridValueCsvCoord(intValueData, Layer.UnityCellSize);
            Vector2 coord = ConvertCellCoord(cellCoord);
            
            Vector3Int cell = new Vector3Int((int)coord.x, (int)coord.y, 0);

            Tilemap.SetTile(cell, tileAsset);
            Tilemap.SetTileFlags(cell, TileFlags.None);
            Tilemap.SetColor(cell, definition.UnityColor);
            
            //for some reason a GameObject is instantiated causing two to exist in play mode; maybe because its the import process. destroy it
            GameObject instantiatedObject = Tilemap.GetInstantiatedObject(cell);
            if (instantiatedObject != null)
            {
                Object.DestroyImmediate(instantiatedObject);
            }
        }
    }
}