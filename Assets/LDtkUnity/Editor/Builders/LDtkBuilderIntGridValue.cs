using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderIntGridValue : LDtkBuilderLayer
    {
        private readonly Dictionary<TilemapKey, TilemapTilesBuilder> _tilemaps = new Dictionary<TilemapKey, TilemapTilesBuilder>();

        public LDtkBuilderIntGridValue(LDtkProjectImporter importer, LDtkComponentLayer layerComponent, LDtkSortingOrder sortingOrder, LDtkJsonImporter debug) : base(importer, layerComponent, sortingOrder, debug)
        {
        }

        public void BuildIntGridValues()
        {
            RoundTilemapPos();
            SortingOrder.Next();

            int[] intGridValues = Layer.IntGridCsv;

            Profiler.BeginSample("IterateAllValues");
            for (int i = 0; i < intGridValues.Length; i++)
            {
                int intGridValue = intGridValues[i];
                
                //all empty intgrid values are 0
                if (intGridValue <= 0)
                {
                    continue;
                }

                LayerDefinition layerDef = Layer.Definition;
                IntGridValueDefinition[] intGridValueDefs = layerDef.IntGridValues;
                
                //IntGrid value defs are reorderable. instead of accessing index, we access the one with the matching value.
                //todo this could be cached so that mapping is faster
                int index = Array.FindIndex(intGridValueDefs, p => p.Value == intGridValue);
                
                int defsLength = intGridValueDefs.Length;
                if (index < 0 || index >= defsLength)
                {
                    LDtkDebug.LogError($"Can't build IntGrid value when trying to access a IntGridValue definition due to OutOfBoundsException. Tried index \"{index}\" of array length \"{defsLength}\". " +
                                       $"Level:{Layer.LevelReference.Identifier}, Layer:{Layer.Identifier}, IntGridValue:{intGridValue}");
                    continue;
                }
                
                IntGridValueDefinition intGridValueDef = layerDef.IntGridValues[index];
                string intGridValueKey = LDtkKeyFormatUtil.IntGridValueFormat(layerDef, intGridValueDef);
                TileBase tile = TryGetIntGridTile(intGridValueKey);

                if (tile == null)
                {
                    LDtkDebug.LogError("Issue loading a IntGridTile. This is always expected to not be null");
                    continue;
                }

                TilemapKey key = tile is LDtkIntGridTile intGridTile 
                    ? new TilemapKey(intGridTile.TilemapTag, intGridTile.TilemapLayerMask, intGridTile.PhysicsMaterial) 
                    : new TilemapKey("Untagged", 0, default);
                
                TilemapTilesBuilder tilemapToBuildOn = GetTilemapToBuildOn(key);

                Profiler.BeginSample("BuildIntGridValue");
                BuildIntGridValue(tilemapToBuildOn, intGridValueDef, i, tile);
                Profiler.EndSample();
            }
            Profiler.EndSample();

            Profiler.BeginSample("IterateAllTilemaps");
            foreach (KeyValuePair<TilemapKey, TilemapTilesBuilder> pair in _tilemaps)
            {
                TilemapKey key = pair.Key;
                TilemapTilesBuilder builder = pair.Value;
                Tilemap tilemap = builder.Map;
                
                Profiler.BeginSample("IntGrid.SetCachedTiles");
                builder.ApplyPendingTiles();
                Profiler.EndSample();
                
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
            Profiler.EndSample();
        }

        private TileBase TryGetIntGridTile(string intGridValueKey)
        {
            TileBase intGridTile = Project.GetIntGridValueTile(intGridValueKey);

            if (intGridTile == null)
            {
                intGridTile = LDtkResourcesLoader.LoadDefaultTile();
            }

            return intGridTile;
        }

        private TilemapTilesBuilder GetTilemapToBuildOn(TilemapKey key)
        {
            if (_tilemaps.ContainsKey(key))
            {
                return _tilemaps[key];
            }
            
            Tilemap newTilemap = CreateNewTilemap(key);
            _tilemaps[key] = new TilemapTilesBuilder(newTilemap);
            return _tilemaps[key];
        }

        private Tilemap CreateNewTilemap(TilemapKey key)
        {
            string name = key.GetNameFormat(Layer.Type);
            GameObject tilemapGameObject = LayerGameObject.CreateChildGameObject(name);

            Tilemap tilemap = tilemapGameObject.AddComponent<Tilemap>();


            if (Project.IntGridValueColorsVisible)
            {
                TilemapRenderer renderer = tilemapGameObject.AddComponent<TilemapRenderer>();
                renderer.sortingOrder = SortingOrder.SortingOrderValue;
            }

            AddTilemapCollider(tilemapGameObject);

            return tilemap;
        }
        
        //Set all of the tilemap call configurations, but set the actual tile later via an optimized SetTiles
        private void BuildIntGridValue(TilemapTilesBuilder tilemapTiles, IntGridValueDefinition definition, int intValueData, TileBase tileAsset)
        {
            Vector3Int cell = LDtkCoordConverter.IntGridValueCsvCoord(intValueData, Layer.UnityCellSize);
            cell = ConvertCellCoord(cell);
            tilemapTiles.SetPendingTile(cell, tileAsset);
            
            //color & transform
            tilemapTiles.SetColor(cell, definition.UnityColor);

            Matrix4x4? matrix = GetIntGridValueScale(tileAsset);
            if (matrix != null)
            {
                tilemapTiles.SetTransformMatrix(cell, matrix.Value);
            }
        }
        
        /// <summary>
        /// There's also scaling code from <see cref="LDtkBuilderLevel.BuildLayerInstance"/> and also in the tileset builder for scale there
        /// </summary>
        private Matrix4x4? GetIntGridValueScale(TileBase tile)
        {
            Vector2 scale = Vector2.one;
            
            //make the scale correct across every pixels per unit configuration from the importer
            
            //in terms of handling tiles on an individual basis, they should always be 1 unless bigger/smaller from being a sprite. Don't try any fancy scaling
            //scale *= LayerScale;

            if (!GetTileDataForTile(tile, out var data))
            {
                return null;
            }

            if (data.colliderType != Tile.ColliderType.Sprite)
            {
                //we're always using the default IntGrid tile asset which is always covering one unit, so we should be doing nothing as it's size is normally resolved
                return Matrix4x4.Scale(scale);
            }
            
            Sprite sprite = data.sprite;
            if (sprite == null)
            {
                //if we chose sprite but assigned no sprite, do no scaling
                return Matrix4x4.Scale(scale);
            }
            
            //when we're using a sprite, the physics sprite can be inaccurate if pixels per unit are wrong.
            //(ex. a physics sprite has a ppu of 8, and the importer has a ppu of 16. this would make the collision size twice as big and not what we want)
            //to solve this, we use the sprite's own ppu into account
            scale *= (sprite.rect.size / sprite.pixelsPerUnit);
            
            //if the sprite was a smaller slice in a texture, scale it to the size it should properly be
            //not using this after evaluating that it's not important and causes issues. bring back if other issues arise?
            //Vector2 texSize = new Vector2(sprite.texture.width, sprite.texture.height);
            //scale *= (texSize / sprite.rect.size);

            scale.x = 1f / scale.x;
            scale.y = 1f / scale.y;
            
            //LDtkDebug.Log($"Scale Tile {Layer.Identifier}, scale {scale}");
            return Matrix4x4.Scale(scale);
        }

        public bool GetTileDataForTile(TileBase tile, out TileData data)
        {
            data = new TileData();
            if (tile is LDtkIntGridTile intGridTile)
            {
                data.colliderType = intGridTile.ColliderType;
                data.sprite = intGridTile.PhysicsSprite;
                return true;
            }

            if (tile is LDtkTilesetTile tilesetTile)
            {
                data.colliderType = tilesetTile._type;
                data.sprite = tilesetTile._sprite;
                return true;
            }
            
            if (tile is Tile basicTile)
            {
                data.colliderType = basicTile.colliderType;
                data.sprite = basicTile.sprite;
                return true;
            }
            
            return false;
        }
        
    }
}