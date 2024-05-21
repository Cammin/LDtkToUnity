using System;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderTileset : LDtkBuilderLayer
    {
        private TileInstance[] _tiles;
        private TilemapTilesBuilder _tilesetProvider;

        public Tilemap Map;
        
        public LDtkBuilderTileset(LDtkProjectImporter project, Level level, LDtkComponentLayer layerComponent, LDtkSortingOrder sortingOrder, LDtkJsonImporter importer) : 
            base(project, level, layerComponent, sortingOrder, importer)
        {
        }

        private void ConstructNewTilemap()
        {
            SortingOrder.Next();
            
            string tilemapName = Layer.IsTilesLayer ? "Tiles" : "AutoLayer";
            
            LDtkProfiler.BeginSample("CreateChildGameObject");
            GameObject tilemapObj = LayerGameObject.CreateChildGameObject(tilemapName);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("AddComponent<Tilemap>");
            Map = tilemapObj.AddComponent<Tilemap>();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("SetOffset");
            AddLayerOffset(Map);
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("AddComponent<TilemapRenderer>");
            TilemapRenderer renderer = tilemapObj.AddComponent<TilemapRenderer>();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("sortingOrder =");
            renderer.sortingOrder = SortingOrder.SortingOrderValue;
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("AddTilemapCollider");
            AddTilemapCollider(tilemapObj);
            LDtkProfiler.EndSample();
        }
        
        public void BuildTileset(TileInstance[] tiles)
        {
            _tiles = tiles;
            
            LDtkProfiler.BeginSample("EvaluateTilesetDefinition");
            TilesetDefinition tilesetDef = EvaluateTilesetDefinition();
            LDtkProfiler.EndSample();
            if (tilesetDef == null)
            {
                //It is possible that a layer has no tileset definition assigned. In this case, it's fine to not build any tiles.
                return;
            }
            
            LDtkProfiler.BeginSample("LoadTilesetArtifacts");
            LDtkArtifactAssetsTileset artifacts = Importer.LoadTilesetArtifacts(Project, tilesetDef);
            LDtkProfiler.EndSample();
            
            if (artifacts == null)
            {
                //failure to load should not spend time calculating tiles
                return;
            }
            
            LDtkProfiler.BeginSample("construct TileBuildingJob");
            TileBuildingJob job = new TileBuildingJob(_tiles, Layer, LayerScale);
            LDtkProfiler.EndSample();

            //figure out the number of jobs to put into processors. +1 to round up
            LDtkProfiler.BeginSample("TileBuildingJob.Schedule");
            int tilesLength = _tiles.Length;
            int innerLoopBatchCount = Mathf.Max(1, (tilesLength / System.Environment.ProcessorCount) + 1);
            JobHandle handle = job.ScheduleParallel(tilesLength, innerLoopBatchCount, default);
            JobHandle.ScheduleBatchedJobs();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("ConstructNewTilemap");
            ConstructNewTilemap();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("new ConstructNewTilemap");
            _tilesetProvider = new TilemapTilesBuilder(Map, tiles.Length);
            LDtkProfiler.EndSample();
            
            //if we are also an intgrid layer, then we already reduced our position in the intGridBuilder
            LDtkProfiler.BeginSample("TryRoundTilemapPos");
            if (!Layer.IsIntGridLayer)
            {
                RoundTilemapPos();
            }
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("SetOpacity");
            Map.SetOpacity(Layer);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("CacheNeededTilesArtifacts");
            TileBase[] tileAssets = new TileBase[tilesLength];
            List<LDtkTilesetTile> artifactTiles = artifacts._tiles;
            int artifactCount = artifactTiles.Count;
            for (int i = 0; i < tilesLength; i++)
            {
                int t = _tiles[i].T;
                
                //if the tile is ever higher than the number of artifacts,
                //it means the tileset definition was reshaped and some rogue tiles were left behind in the level,
                //awaiting to be recovered upon bringing back the tileset def size.
                if (t < artifactCount)
                {
                    tileAssets[i] = artifactTiles[t];
                }
            }
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("handle.Complete");
            handle.Complete();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("Input.Dispose");
            job.Input.Dispose();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("AddTiles");
            Vector3Int[] cells = new Vector3Int[tilesLength];
            for (int i = 0; i < tilesLength; i++)
            {
                cells[i] = job.Output[i].Cell;
                cells[i].z = _tilesetProvider.GetNextCellZ(cells[i]);
            }
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("Tilemap.SetTiles");
            Map.SetTiles(cells, tileAssets);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("SetColorAndMatrix");
            for (int i = 0; i < tilesLength; i++)
            {
                Color color = new Color(1, 1, 1, _tiles[i].A);
                Matrix4x4 matrix = job.Output[i].Matrix;
                _tilesetProvider.SetColorAndMatrix(cells[i], ref color, ref matrix);
            }
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("Output.Dispose");
            job.Output.Dispose();
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("ApplyExtraData");
            _tilesetProvider.ApplyExtraData();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("CompressBounds");
            Map.CompressBounds();
            LDtkProfiler.EndSample();
        }
        
        private TilesetDefinition EvaluateTilesetDefinition()
        {
            return Layer.OverrideTilesetUid != null ? Layer.OverrideTilesetDefinition : Layer.TilesetDefinition;
        }
    }
}
