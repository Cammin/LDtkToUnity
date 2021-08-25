using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public abstract class LDtkLayerBuilder
    {
        protected LayerInstance Layer;
        protected readonly LDtkProjectImporter Importer;
        protected readonly GameObject LayerGameObject;
        protected readonly LDtkSortingOrder SortingOrder;
        public float LayerScale;
        
        protected LDtkLayerBuilder(LDtkProjectImporter importer, GameObject layerGameObject, LDtkSortingOrder sortingOrder)
        {
            Importer = importer;
            LayerGameObject = layerGameObject;
            SortingOrder = sortingOrder;
        }

        [ExcludeFromDocs]
        public void SetLayer(LayerInstance layer)
        {
            Layer = layer;
            LayerScale = Layer.GridSize / (float)Importer.PixelsPerUnit;
        }
        
        protected Vector2Int ConvertCellCoord(Vector2Int cellCoord)
        {
            return LDtkCoordConverter.ConvertCell(cellCoord, (int) Layer.CHei);
        }

        protected void RoundTilemapPos()
        {
            long cellHeightPx = Layer.CHei * Layer.GridSize;
            long extraPixels = cellHeightPx - Layer.LevelReference.PxHei;
            float worldOffset = extraPixels / (float)Importer.PixelsPerUnit;

            Vector2 pos = LayerGameObject.transform.position;
            pos.y -= worldOffset;
            LayerGameObject.transform.position = pos;
        }

        protected void AddLayerOffset(Tilemap tilemap)
        {
            tilemap.tileAnchor += (Vector3)Layer.UnityWorldTotalOffset;
        }
    }
}