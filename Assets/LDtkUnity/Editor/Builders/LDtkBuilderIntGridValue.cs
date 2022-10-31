﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderIntGridValue : LDtkBuilderLayer
    {
        private readonly Dictionary<TilemapKey, Tilemap> _tilemaps = new Dictionary<TilemapKey, Tilemap>();

        public LDtkBuilderIntGridValue(LDtkProjectImporter importer, LDtkComponentLayer layerComponent, LDtkSortingOrder sortingOrder) : base(importer, layerComponent, sortingOrder)
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
                    LDtkDebug.LogError($"Can't build IntGrid value when trying to access a IntGridValue definition due to OutOfBoundsException. Tried index \"{index}\" of array length \"{defsLength}\". " +
                                       $"Level:{Layer.LevelReference.Identifier}, Layer:{Layer.Identifier}, IntGridValue:{intGridValue}");
                    continue;
                }
                
                IntGridValueDefinition intGridValueDef = layerDef.IntGridValues[index];
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
                intGridTile = LDtkResourcesLoader.LoadDefaultTile();
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

            ApplyIntGridValueScale(tilemapToBuildOn, cell);
        }
        
        private void ApplyIntGridValueScale(Tilemap tilemap, Vector3Int coord)
        {
            LDtkIntGridTile tile = tilemap.GetTile<LDtkIntGridTile>(coord);
            if (tile == null)
            {
                LDtkDebug.LogError("Problem getting a tile to scale it");
            }
            
            if (tile.ColliderType != Tile.ColliderType.Sprite)
            {
                //we're always using the default IntGrid tile asset which is always covering one unit, so we should be doing nothing as it's size is normally resolved
                return;
            }

            Sprite sprite = tile.PhysicsSprite;
            if (sprite == null)
            {
                //if we chose sprite but assigned no sprite, do no scaling
                return;
            }
            
            Vector2 scale = Vector2.one;
            
            //make the scale correct across every pixels per unit configuration from the importer
            scale *= (float)Layer.GridSize / Importer.PixelsPerUnit;
            
            //when we're using a sprite, the physics sprite can be inaccurate if pixels per unit are wrong.
            //(ex. a physics sprite has a ppu of 8, and the importer has a ppu of 16. this would make the collision size twice as big and not what we want)
            //to solve this, we use the sprite's own ppu into account
            scale *= (sprite.rect.size / sprite.pixelsPerUnit);
            
            //if the sprite was a smaller slice in a texture, scale it to the size it should properly be
            Vector2 texSize = new Vector2(sprite.texture.width, sprite.texture.height);
            scale *= (texSize / sprite.rect.size);
            
            Matrix4x4 matrix = Matrix4x4.Scale(scale);
            tilemap.SetTransformMatrix(coord, matrix);
            
            //LDtkDebug.Log($"Scale Tile {Layer.Identifier}, scale {scale}");
        }
    }
}