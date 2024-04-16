using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal abstract class LDtkBuilderLayer
    {
        protected LayerInstance Layer;
        protected readonly LDtkProjectImporter Project;
        protected LDtkJsonImporter Importer;
        protected readonly GameObject LayerGameObject;
        protected readonly LDtkSortingOrder SortingOrder;
        protected LDtkComponentLayer LayerComponent;
        public float LayerScale;

        protected LDtkBuilderLayer(LDtkProjectImporter project, LDtkComponentLayer layerComponent, LDtkSortingOrder sortingOrder, LDtkJsonImporter importer)
        {
            Project = project;
            LayerComponent = layerComponent;
            LayerGameObject = layerComponent.gameObject;
            SortingOrder = sortingOrder;
            Importer = importer;
        }
        
        public float SetLayerAndScale(LayerInstance layer)
        {
            Layer = layer;
            LayerScale = Layer.GridSize / (float)Project.PixelsPerUnit;
            
            //todo need to evaluate scale here too potentially?
            return LayerScale;
        }
        
        protected Vector3Int ConvertCellCoord(Vector3Int cellCoord)
        {
            return LDtkCoordConverter.ConvertCell(cellCoord, Layer.CHei);
        }

        protected void RoundTilemapPos()
        {
            long cellHeightPx = Layer.CHei * Layer.GridSize;
            long extraPixels = cellHeightPx - Layer.LevelReference.PxHei;
            float worldOffset = extraPixels / (float)Project.PixelsPerUnit;

            Vector2 pos = LayerGameObject.transform.position;
            pos.y -= worldOffset;
            LayerGameObject.transform.position = pos;
        }

        protected void AddLayerOffset(Tilemap tilemap)
        {
            tilemap.tileAnchor += (Vector3)Layer.UnityWorldTotalOffset;
        }
        
        protected void AddTilemapCollider(GameObject tilemapGameObject)
        {
            //intentionally making the order of the composite collider first because of issues with physics particles:
            //https://forum.unity.com/threads/tilemap-collider-with-composite-doesnt-work-with-particle-system-collision-trigger.833737/#post-9173561
            
            if (Project.UseCompositeCollider)
            {
                Rigidbody2D rb = tilemapGameObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;

                CompositeCollider2D composite = tilemapGameObject.AddComponent<CompositeCollider2D>();
                composite.geometryType = Project.GeometryType;
            }
            
            TilemapCollider2D collider = tilemapGameObject.AddComponent<TilemapCollider2D>();
            ConfigureTilemapCollider(collider);
        }

        public static bool ConfigureTilemapCollider(TilemapCollider2D collider)
        {
            if (!collider.GetComponent<CompositeCollider2D>())
            {
                return false;
            }

            bool usedByComposite = collider.GetComponent<CompositeCollider2D>();
            
#if UNITY_2023_1_OR_NEWER
            collider.compositeOperation = usedByComposite ? Collider2D.CompositeOperation.Merge : Collider2D.CompositeOperation.None;
#else
            collider.usedByComposite = usedByComposite;
#endif
            return true;
        }
    }
}