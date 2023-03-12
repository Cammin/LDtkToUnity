using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal abstract class LDtkBuilderLayer
    {
        protected LayerInstance Layer;
        protected readonly LDtkProjectImporter Importer;
        protected readonly GameObject LayerGameObject;
        protected readonly LDtkSortingOrder SortingOrder;
        protected LDtkComponentLayer LayerComponent;
        public float LayerScale;

        protected LDtkBuilderLayer(LDtkProjectImporter importer, LDtkComponentLayer layerComponent, LDtkSortingOrder sortingOrder)
        {
            Importer = importer;
            LayerComponent = layerComponent;
            LayerGameObject = layerComponent.gameObject;
            SortingOrder = sortingOrder;
            
        }
        
        public void SetLayer(LayerInstance layer)
        {
            Layer = layer;
            LayerScale = Layer.GridSize / (float)Importer.PixelsPerUnit;
            LayerComponent.SetScale(LayerScale);
        }
        
        protected Vector2Int ConvertCellCoord(Vector2Int cellCoord)
        {
            return LDtkCoordConverter.ConvertCell(cellCoord, Layer.CHei);
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