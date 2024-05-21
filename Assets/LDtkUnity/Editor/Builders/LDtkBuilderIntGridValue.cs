using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderIntGridValue : LDtkBuilderLayer
    {
        private readonly Dictionary<TilemapKey, TilemapTilesBuilder> _tilemaps;

        public LDtkBuilderIntGridValue(LDtkProjectImporter importer, Level level, LDtkComponentLayer layerComponent, LDtkSortingOrder sortingOrder, LDtkJsonImporter debug) : 
            base(importer, level, layerComponent, sortingOrder, debug)
        {
            _tilemaps = new Dictionary<TilemapKey, TilemapTilesBuilder>(5);
        }

        public void BuildIntGridValues()
        {
            RoundTilemapPos();
            SortingOrder.Next();
            
            LayerDefinition layerDef = Layer.Definition;
            IntGridValueDefinition[] intGridValueDefs = layerDef.IntGridValues;
            int defsLength = intGridValueDefs.Length;

            LDtkProfiler.BeginSample("MakeDefToTile");
            Dictionary<IntGridValueDefinition, TileBase> defToTile = new Dictionary<IntGridValueDefinition, TileBase>(defsLength);
            Dictionary<IntGridValueDefinition, TilemapKey> defToKey = new Dictionary<IntGridValueDefinition, TilemapKey>(defsLength);
            Dictionary<int, int> reorderableIntGridValuesMap = new Dictionary<int, int>(defsLength);
            
            for (int i = 0; i < defsLength; i++)
            {
                IntGridValueDefinition intGridValueDef = intGridValueDefs[i];
                reorderableIntGridValuesMap.Add(intGridValueDef.Value, i);
                string formatString = LDtkKeyFormatUtil.IntGridValueFormat(layerDef, intGridValueDef);
                TileBase tile = TryGetIntGridTile(formatString);

                if (tile == null)
                {
                    LDtkDebug.LogError("Issue loading a IntGridTile. This is always expected to exist");
                    continue;
                }

                defToTile.Add(intGridValueDef, tile);

                TilemapKey key = tile is LDtkIntGridTile intGridTile
                    ? new TilemapKey(intGridTile.TilemapTag, intGridTile.TilemapLayerMask, intGridTile.PhysicsMaterial)
                    : new TilemapKey("Untagged", 0, default);
                
                defToKey.Add(intGridValueDef, key);
            }
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("IterateAllValues");
            Vector3Int cell = new Vector3Int();
            cell.x = -1;
            int width = Layer.CWid;
            for (int i = 0; i < Layer.IntGridCsv.Length; i++)
            {
                cell.x++;
                if (cell.x >= width)
                {
                    cell.x = 0;
                    cell.y++;
                }
                
                int intGridValue = Layer.IntGridCsv[i];
                
                //all empty intgrid values are 0
                if (intGridValue <= 0)
                {
                    continue;
                }

                //IntGrid value defs are reorder-able. instead of accessing index, we access the one with the matching value.
                int index = reorderableIntGridValuesMap[intGridValue];

                if (index < 0 || index >= defsLength)
                {
                    LDtkDebug.LogError($"Can't build IntGrid value when trying to access a IntGridValue definition due to OutOfBoundsException. Tried index \"{index}\" of array length \"{defsLength}\". " +
                                       $"Level:{Layer.LevelReference.Identifier}, Layer:{Layer.Identifier}, IntGridValue:{intGridValue}");
                    continue;
                }

                IntGridValueDefinition intGridValueDef = intGridValueDefs[index];
                TileBase tile = defToTile[intGridValueDef];

                LDtkProfiler.BeginSample("GetTilemapToBuildOn");
                TilemapTilesBuilder tilemapToBuildOn = GetTilemapToBuildOn(defToKey[intGridValueDef]);
                LDtkProfiler.EndSample();

                //Set all the tilemap call configurations, but set the actual tile later via an optimized SetTiles
                LDtkProfiler.BeginSample("ConvertCellCoord");
                Vector3Int cellToPut = ConvertCellCoord(cell);
                LDtkProfiler.EndSample();

                LDtkProfiler.BeginSample("SetPendingTile");
                tilemapToBuildOn.SetPendingTile(cellToPut, tile);
                LDtkProfiler.EndSample();

                //color & transform
                LDtkProfiler.BeginSample("SetColorAndMatrix");
                tilemapToBuildOn.SetColor(cellToPut, intGridValueDef.UnityColor);
                Matrix4x4? matrix = GetIntGridValueScale(tile);
                if (matrix != null)
                {
                    tilemapToBuildOn.SetTransformMatrix(cellToPut, matrix.Value);
                }

                LDtkProfiler.EndSample();
            }

            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("IterateAllTilemaps");
            foreach (KeyValuePair<TilemapKey, TilemapTilesBuilder> pair in _tilemaps)
            {
                TilemapKey key = pair.Key;
                TilemapTilesBuilder builder = pair.Value;
                Tilemap tilemap = builder.Map;
                
                LDtkProfiler.BeginSample("IntGrid.ApplyPendingTiles");
                builder.ApplyPendingTiles(true);
                LDtkProfiler.EndSample();
                
                tilemap.SetOpacity(Layer);
                
                //todo: predetermine the position instead during gameobject creation. profile this if it's actually proves slow
                AddLayerOffset(tilemap);

                GameObject obj = tilemap.gameObject;
                obj.tag = key.Tag;
                obj.layer = key.LayerMask;
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.sharedMaterial = key.PhysicsMaterial;
                }
            }
            LDtkProfiler.EndSample();
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
            LDtkProfiler.BeginSample("CreateNewTilemap");
            if (_tilemaps.ContainsKey(key))
            {
                LDtkProfiler.EndSample();
                return _tilemaps[key];
            }
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("CreateNewTilemap");
            Tilemap newTilemap = CreateNewTilemap(key);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("new TilemapTilesBuilder");
            _tilemaps[key] = new TilemapTilesBuilder(newTilemap, 10);
            LDtkProfiler.EndSample();
            
            return _tilemaps[key];
        }

        private Tilemap CreateNewTilemap(TilemapKey key)
        {
            LDtkProfiler.BeginSample("GetNameFormat");
            string name = key.GetNameFormat(Layer.Type);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("CreateChildGameObject");
            GameObject tilemapGameObject = LayerGameObject.CreateChildGameObject(name);
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("AddComponent<Tilemap>");
            Tilemap tilemap = tilemapGameObject.AddComponent<Tilemap>();
            LDtkProfiler.EndSample();


            if (Project.IntGridValueColorsVisible)
            {
                TilemapRenderer renderer = tilemapGameObject.AddComponent<TilemapRenderer>();
                renderer.sortingOrder = SortingOrder.SortingOrderValue;
            }

            LDtkProfiler.BeginSample("AddTilemapCollider");
            AddTilemapCollider(tilemapGameObject);
            LDtkProfiler.EndSample();

            return tilemap;
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