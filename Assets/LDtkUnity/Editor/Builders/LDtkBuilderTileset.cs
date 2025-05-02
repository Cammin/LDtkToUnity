using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderTileset : LDtkBuilderLayer
    {
        private TileInstance[] _tiles;

        //in most realistic situations, tiles will not overlap, but we can overestimate anyways to avoid resizing 
        private Dictionary<Vector3Int, int> _depth;

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
            _depth = new Dictionary<Vector3Int, int>(10);
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
            
            LDtkProfiler.BeginSample("CacheNeededTilesArtifactsAndSetupColor");
            TileChangeData[] tileAssets = new TileChangeData[tilesLength];
            //Determine tile asset and setup color while the job is running
            LDtkTilesetTile[] artifactTiles = artifacts._tiles;
            int artifactCount = artifactTiles.Length;
            for (int i = 0; i < tilesLength; i++)
            {
                tileAssets[i].color = new Color(1, 1, 1, _tiles[i].A);
                
                int? t = _tiles[i].T;
                //it's possible that a t value is null in the json, unfortunately for performance's sake
                if (t == null)
                {
                    continue;
                }
                
                //if the tile is ever higher than the number of artifacts,
                //it means the tileset definition was reshaped and some rogue tiles were left behind in the level,
                //awaiting to be recovered upon bringing back the tileset def size.
                int tValue = t.Value;
                if (tValue < artifactCount)
                {
                    tileAssets[i].tile = artifactTiles[tValue];
                }
            }
            LDtkProfiler.EndSample();
            
            //Job is done, we can use the output now!
            LDtkProfiler.BeginSample("handle.Complete");
            handle.Complete();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("Input.Dispose");
            job.Input.Dispose();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("RecalculateCellPositionsAndSetupMatrix");
            for (int i = 0; i < tilesLength; i++)
            {
                //todo find a way to improve performance of this somehow where we dont need to make a vector3int copy
                Vector3Int cell = job.Output[i].Cell;
                cell.z = GetNextCellZ(cell);
                tileAssets[i].position = cell;
                tileAssets[i].transform = job.Output[i].Matrix;
            }
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("Tilemap.SetTiles");
            TilemapTilesBuilder.SetTiles(Map, tileAssets);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("Output.Dispose");
            job.Output.Dispose();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("CompressBounds");
            Map.CompressBounds();
            LDtkProfiler.EndSample();
        }
        
        private TilesetDefinition EvaluateTilesetDefinition()
        {
            return Layer.OverrideTilesetUid != null ? Layer.OverrideTilesetDefinition : Layer.TilesetDefinition;
        }
        
        /// <param name="cell">Z is always 0</param>
        private int GetNextCellZ(Vector3Int cell)
        {
            if (!_depth.ContainsKey(cell))
            {
                _depth.Add(cell, 0);
                return 0;
            }

            _depth[cell] += 1;
            return _depth[cell];
        }
    }
}
