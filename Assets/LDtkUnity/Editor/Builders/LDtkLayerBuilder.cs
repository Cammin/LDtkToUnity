using UnityEngine;

namespace LDtkUnity.Editor.Builders
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
    }
}