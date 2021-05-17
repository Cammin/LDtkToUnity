using UnityEngine;

namespace LDtkUnity.Editor
{
    public abstract class LDtkLayerBuilder
    {
        protected readonly LDtkProjectImporter Importer;
        protected readonly GameObject LayerGameObject;
        protected readonly LDtkSortingOrder SortingOrder;
        protected LayerInstance Layer;
        
        protected LDtkLayerBuilder(LDtkProjectImporter importer, GameObject layerGameObject, LDtkSortingOrder sortingOrder)
        {
            Importer = importer;
            LayerGameObject = layerGameObject;
            SortingOrder = sortingOrder;
        }

        public void SetLayer(LayerInstance layer)
        {
            Layer = layer;
        }
        
        protected Vector2Int ConvertCellCoord(Vector2Int cellCoord)
        {
            return LDtkToolOriginCoordConverter.ConvertCell(cellCoord, (int) Layer.CHei);
        }
    }
}