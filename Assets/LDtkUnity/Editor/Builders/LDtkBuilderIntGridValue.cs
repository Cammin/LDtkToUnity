using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal class LDtkBuilderIntGridValue : LDtkBuilderLayer
    {
        private readonly Dictionary<TilemapKey, Tilemap> _tilemaps = new Dictionary<TilemapKey, Tilemap>();

        public LDtkBuilderIntGridValue(LDtkProjectImporter importer, GameObject layerGameObject, LDtkSortingOrder sortingOrder) : base(importer, layerGameObject, sortingOrder)
        {
        }

        public void BuildIntGridValues()
        {
            RoundTilemapPos();
            SortingOrder.Next();

            long[] intGridValues = Layer.IntGridCsv;

            for (int i = 0; i < intGridValues.Length; i++)
            {
                long intGridValue = intGridValues[i];
                
                //all empty intgrid values are 0
                if (intGridValue <= 0)
                {
                    continue;
                }

                LayerDefinition layerDef = Layer.Definition;
                long index = intGridValue - 1;
                int defsLength = layerDef.IntGridValues.Length;
                if (index < 0 || index >= defsLength)
                {
                    LDtkDebug.LogError($"Can't build IntGrid value when trying to access a IntGridValue definition due to OutOfBoundsException. Tried index \"{index}\" of array length \"{defsLength}\"" +
                                       $"Level:{Layer.LevelReference.Identifier}, Layer:{Layer.Identifier}, IntGridValue:{intGridValue}");
                    continue;
                }
                
                IntGridValueDefinition intGridValueDef = layerDef.IntGridValues[intGridValue-1];
                string intGridValueKey = LDtkKeyFormatUtil.IntGridValueFormat(layerDef, intGridValueDef);
                LDtkIntGridTile intGridTile = TryGetIntGridTile(intGridValueKey);

                if (intGridTile == null)
                {
                    LDtkDebug.LogError("Issue loading a IntGridTile. This is always expected to not be null");
                    continue;
                }
                
                TilemapKey key = new TilemapKey(intGridTile.TilemapTag, intGridTile.TilemapLayerMask, intGridTile.PhysicsMaterial);
                Tilemap tilemapToBuildOn = GetTilemapToBuildOn(key);

                BuildIntGridValue(tilemapToBuildOn, intGridValueDef, i, intGridTile);
            }

            foreach (KeyValuePair<TilemapKey, Tilemap> pair in _tilemaps)
            {
                TilemapKey key = pair.Key;
                Tilemap tilemap = pair.Value;
                
                tilemap.SetOpacity(Layer);
                AddLayerOffset(tilemap);

                GameObject obj = tilemap.gameObject;
                obj.tag = key.Tag;
                obj.layer = key.LayerMask;
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.sharedMaterial = key.PhysicsMaterial;
                }
            }
        }

        private LDtkIntGridTile TryGetIntGridTile(string intGridValueKey)
        {
            LDtkIntGridTile intGridTile = Importer.GetIntGridValueTile(intGridValueKey);

            if (intGridTile == null)
            {
                long layerGridSize = Layer.GridSize;
                intGridTile = (LDtkIntGridTile)Importer.GetDefaultTileArtifact(layerGridSize);
                if (intGridTile == null)
                {
                    LDtkDebug.Log($"Issue loading a default tile for {layerGridSize}");
                }
            }

            return intGridTile;
        }

        private Tilemap GetTilemapToBuildOn(TilemapKey key)
        {
            if (_tilemaps.ContainsKey(key))
            {
                return _tilemaps[key];
            }
            
            Tilemap newTilemap = CreateNewTilemap(key);
            _tilemaps[key] = newTilemap;
            return _tilemaps[key];
        }

        private Tilemap CreateNewTilemap(TilemapKey key)
        {
            string name = key.GetNameFormat(Layer.Type);
            GameObject tilemapGameObject = LayerGameObject.CreateChildGameObject(name);

            /*if (Importer.DeparentInRuntime)
            {
                tilemapGameObject.AddComponent<LDtkDetachChildren>();
            }*/

            Tilemap tilemap = tilemapGameObject.AddComponent<Tilemap>();


            if (Importer.IntGridValueColorsVisible)
            {
                TilemapRenderer renderer = tilemapGameObject.AddComponent<TilemapRenderer>();
                renderer.sortingOrder = SortingOrder.SortingOrderValue;
            }

            TilemapCollider2D collider = tilemapGameObject.AddComponent<TilemapCollider2D>();

            if (Importer.UseCompositeCollider)
            {
                Rigidbody2D rb = tilemapGameObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;

                CompositeCollider2D composite = tilemapGameObject.AddComponent<CompositeCollider2D>();
                collider.usedByComposite = true;
            }

            return tilemap;
        }


        private void BuildIntGridValue(Tilemap tilemapToBuildOn, IntGridValueDefinition definition, int intValueData, TileBase tileAsset)
        {
            Vector2Int cellCoord = LDtkCoordConverter.IntGridValueCsvCoord(intValueData, Layer.UnityCellSize);
            Vector2 coord = ConvertCellCoord(cellCoord);
            
            Vector3Int cell = new Vector3Int((int)coord.x, (int)coord.y, 0);

            tilemapToBuildOn.SetTile(cell, tileAsset);
            tilemapToBuildOn.SetTileFlags(cell, TileFlags.None);
            tilemapToBuildOn.SetColor(cell, definition.UnityColor);
            
            //for some reason a GameObject is instantiated causing two to exist in play mode; maybe because its the import process. destroy it
            GameObject instantiatedObject = tilemapToBuildOn.GetInstantiatedObject(cell);
            if (instantiatedObject != null)
            {
                Object.DestroyImmediate(instantiatedObject);
            }
        }
    }
}