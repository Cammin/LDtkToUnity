using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderTileset : LDtkBuilderLayer
    {
        private TileInstance[] _tiles;

        private readonly OffsetTilemapStacks _tilesetProvider;
        private int _layerCount = 0;

        public List<Tilemap> Tilemaps = new List<Tilemap>();
        
        public LDtkBuilderTileset(LDtkProjectImporter importer, LDtkComponentLayer layerComponent, LDtkSortingOrder sortingOrder) : base(importer, layerComponent, sortingOrder)
        {
            _tilesetProvider = new OffsetTilemapStacks(ConstructNewTilemap);
            
        }

        public void BuildTileset(TileInstance[] tiles)
        {
            //if we are also an intgrid layer, then we already reduced our position in the intGridBuilder
            if (!Layer.IsIntGridLayer)
            {
                RoundTilemapPos();
            }
            
            _tiles = tiles;
            _tilesetProvider.Clear();

            TilesetDefinition tilesetDef = EvaluateTilesetDefinition();
            if (tilesetDef == null)
            {
                LDtkDebug.LogError($"Tileset Definition for {Layer.Identifier} was null.");
                return;
            }
            
            //figure out if we have already built a tile in this position. otherwise, build up to the next tilemap. build in a completely separate path if this is an offset position from the normal standard coordinates
            Profiler.BeginSample("AddTiles");
            for (int i = _tiles.Length - 1; i >= 0; i--)
            {
                TileInstance tileData = _tiles[i];
                if (!CanPlaceTileInLevelBounds(tileData))
                {
                    continue;
                }
                
                Profiler.BeginSample("GetTilemapFromStacks");
                TilemapTilesBuilder tilesBuilder = _tilesetProvider.GetTilemapFromStacks(tileData.UnityPx, Layer.GridSize);
                Tilemaps.Add(tilesBuilder.Map);
                Profiler.EndSample();

                Profiler.BeginSample("GetTileForTileInstance");
                TileBase tile = GetTileForTileInstance(tileData, tilesetDef);
                Profiler.EndSample();
                
                Profiler.BeginSample("CacheTile");
                CacheTile(tileData, tilesBuilder, tile);
                Profiler.EndSample();
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("SetCachedTiles");
            _tilesetProvider.SetCachedTiles();
            Profiler.EndSample();

            Profiler.BeginSample("SetOpacityAndOffset");
            //set each layer's alpha
            foreach (Tilemap tilemap in _tilesetProvider.Tilemaps)
            {
                AddLayerOffset(tilemap);
                tilemap.SetOpacity(Layer);
            }
            Profiler.EndSample();
        }

        private TileBase GetTileForTileInstance(TileInstance tileData, TilesetDefinition tilesetDef)
        {
            Profiler.BeginSample("InitCalc");
            Vector2Int srcPos = tileData.UnitySrc;
            int gridSize = tilesetDef.TileGridSize;
            Rect slice = new Rect(srcPos.x, srcPos.y, gridSize, gridSize);
            Profiler.EndSample();
            
            Profiler.BeginSample("GetTileArtifact");
            TileBase tile = Importer.GetTileArtifact(tilesetDef, slice);
            Profiler.EndSample();
            
            return tile;
        }

        private bool CanPlaceTileInLevelBounds(TileInstance tileInstance)
        {
            Level level = Layer.LevelReference;
            RectInt rect = level.UnityWorldRect;
            rect.x = 0;
            rect.y = 0;

            return rect.Contains(tileInstance.UnityPx);
        }

        

        private TilesetDefinition EvaluateTilesetDefinition()
        {
            if (Layer.OverrideTilesetUid != null)
            {
                return Layer.OverrideTilesetDefinition;
            }

            return Layer.Definition.TilesetDefinition;
        }

        private Tilemap ConstructNewTilemap()
        {
            SortingOrder.Next();
            
            string objName = $"{GetLayerName(Layer)}_{_layerCount}";
            GameObject tilemapObj = LayerGameObject.CreateChildGameObject(objName);
            Tilemap tilemap = tilemapObj.AddComponent<Tilemap>();

            TilemapRenderer renderer = tilemapObj.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = SortingOrder.SortingOrderValue;

            _layerCount++;
            
            return tilemap;
        }

        // Layer type (possible values: IntGrid, Entities, Tiles or AutoLayer)
        private string GetLayerName(LayerInstance layer)
        {
            if (layer.IsTilesLayer)
            {
                return "Tiles";
            }

            return "AutoLayer";

        }

        private void CacheTile(TileInstance tileData, TilemapTilesBuilder tiles, TileBase tile)
        {
            Vector2Int coord = GetConvertedCoord(tileData);

            //Vector2Int px = tileData.UnityPx;
            //int tilemapLayer = GetTilemapLayerToBuildOn(px);
            Vector3Int cell = new Vector3Int(coord.x, coord.y, 0);

            tiles.CacheTile(cell, tile);
            tiles.SetTransformMatrix(cell, GetTileInstanceFlips(tileData));
        }

        private Vector2Int GetConvertedCoord(TileInstance tileData)
        {
            //doing the division like this because the operator is not available in older unity versions
            Vector2Int coord = new Vector2Int(
                tileData.UnityPx.x / Layer.GridSize,
                tileData.UnityPx.y / Layer.GridSize);

            return ConvertCellCoord(coord);
        }
        
        private Matrix4x4 GetTileInstanceFlips(TileInstance tileData)
        {
            float scaleFactor = 1f / LayerScale;
            Vector3 scale = new Vector3(scaleFactor, scaleFactor, 1);
            scale.x *= tileData.FlipX ? -1 : 1;
            scale.y *= tileData.FlipY ? -1 : 1;
            Matrix4x4 matrix = Matrix4x4.Scale(scale);
            return matrix;
        }
    }
}
