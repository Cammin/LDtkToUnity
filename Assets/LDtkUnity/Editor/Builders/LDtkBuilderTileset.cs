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

        public HashSet<Tilemap> Tilemaps = new HashSet<Tilemap>();
        
        public LDtkBuilderTileset(LDtkProjectImporter project, LDtkComponentLayer layerComponent, LDtkSortingOrder sortingOrder, LDtkJsonImporter importer) : base(project, layerComponent, sortingOrder, importer)
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
                //It is possible that a layer has no tileset definition assigned. In this case, it's fine to not build any tiles.
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
                
                Profiler.BeginSample("GetTileArtifact");
                TileBase tile = Importer.GetTileArtifact(Project, tilesetDef, tileData.T);
                Profiler.EndSample();

                Profiler.BeginSample("CacheTile");
                SetPendingTile(tileData, tilesBuilder, tile);
                Profiler.EndSample();
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("SetCachedTiles");
            _tilesetProvider.SetPendingTiles();
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

            return Layer.TilesetDefinition;
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

            AddTilemapCollider(tilemapObj);
            
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

        private void SetPendingTile(TileInstance tileData, TilemapTilesBuilder tiles, TileBase tile)
        {
            Vector3Int cell = GetConvertedCoord(tileData);

            //Vector2Int px = tileData.UnityPx;
            //int tilemapLayer = GetTilemapLayerToBuildOn(px);

            tiles.SetPendingTile(cell, tile);
            tiles.SetTransformMatrix(cell, GetTileInstanceFlips(tileData));
            tiles.SetColor(cell, new Color(1, 1, 1, tileData.A));
        }

        private Vector3Int GetConvertedCoord(TileInstance tileData)
        {
            //doing the division like this because the operator is not available in older unity versions
            Vector3Int coord = new Vector3Int(
                tileData.UnityPx.x / Layer.GridSize,
                tileData.UnityPx.y / Layer.GridSize,
                0);

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
