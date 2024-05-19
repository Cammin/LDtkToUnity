using System;
using System.Collections.Generic;
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

        public void BuildTileset(TileInstance[] tiles)
        {
            _tiles = tiles;
            
            Profiler.BeginSample("ConstructNewTilemap");
            ConstructNewTilemap();
            Profiler.EndSample();
            
            Profiler.BeginSample("new ConstructNewTilemap");
            _tilesetProvider = new TilemapTilesBuilder(Map, tiles.Length);
            Profiler.EndSample();
            
            //if we are also an intgrid layer, then we already reduced our position in the intGridBuilder
            Profiler.BeginSample("TryRoundTilemapPos");
            if (!Layer.IsIntGridLayer)
            {
                RoundTilemapPos();
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("EvaluateTilesetDefinition");
            TilesetDefinition tilesetDef = EvaluateTilesetDefinition();
            if (tilesetDef == null)
            {
                //It is possible that a layer has no tileset definition assigned. In this case, it's fine to not build any tiles.
                Profiler.EndSample();
                return;
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("LoadTilesetArtifacts");
            LDtkArtifactAssetsTileset artifacts = Importer.LoadTilesetArtifacts(Project, tilesetDef);
            Profiler.EndSample();
            
            if (artifacts == null)
            {
                //failure to load should not spend time calculating tiles
                return;
            }
            
            //figure out if we have already built a tile in this position. otherwise, build up to the next tilemap. build in a completely separate path if this is an offset position from the normal standard coordinates
            Profiler.BeginSample("AddTiles");
            for (int i = _tiles.Length - 1; i >= 0; i--)
            {
                TileInstance tileInstance = _tiles[i];
                
                Profiler.BeginSample("GetTileArtifact");
                TileBase tile;
                int tileID = tileInstance.T;
                try
                {
                    tile = artifacts._tiles[tileInstance.T];
                }
                catch (IndexOutOfRangeException)
                {
                    Importer.Logger.LogError($"IndexOutOfRangeException: Failed to load a tile artifact at id \"{tileID}\" from \"{tilesetDef.Identifier}\". It's possible that the tileset definition file has imported improperly");
                    tile = null;
                }
                Profiler.EndSample();

                Profiler.BeginSample("SetPendingTile");
                SetPendingTile(tileInstance, tile);
                Profiler.EndSample();
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("ApplyPendingTiles");
            _tilesetProvider.ApplyPendingTiles(false);
            Profiler.EndSample();
            
            Profiler.BeginSample("SetOpacity");
            Map.SetOpacity(Layer);
            Profiler.EndSample();
        }
        
        public static Vector3Int GetCellForTileCoord(TileInstance tile, LayerInstance layer)
        {
            int id = layer.IsAutoLayer ? tile.AutoLayerCoordId : tile.TileLayerCoordId;

            int x = id % layer.CWid;
            int y = id / layer.CWid;
            
            return new Vector3Int(x, y, 0);
        }
        
        private TilesetDefinition EvaluateTilesetDefinition()
        {
            if (Layer.OverrideTilesetUid != null)
            {
                return Layer.OverrideTilesetDefinition;
            }

            return Layer.TilesetDefinition;
        }

        private void ConstructNewTilemap()
        {
            SortingOrder.Next();
            
            string tilemapName = Layer.IsTilesLayer ? "Tiles" : "AutoLayer";
            
            
            Profiler.BeginSample("CreateChildGameObject");
            GameObject tilemapObj = LayerGameObject.CreateChildGameObject(tilemapName);
            Profiler.EndSample();
            
            Profiler.BeginSample("AddComponent<Tilemap>");
            Map = tilemapObj.AddComponent<Tilemap>();
            Profiler.EndSample();
            
            Profiler.BeginSample("SetOffset");
            AddLayerOffset(Map);
            Profiler.EndSample();

            Profiler.BeginSample("AddComponent<TilemapRenderer>");
            TilemapRenderer renderer = tilemapObj.AddComponent<TilemapRenderer>();
            Profiler.EndSample();
            
            Profiler.BeginSample("sortingOrder =");
            renderer.sortingOrder = SortingOrder.SortingOrderValue;
            Profiler.EndSample();

            Profiler.BeginSample("AddTilemapCollider");
            AddTilemapCollider(tilemapObj);
            Profiler.EndSample();
        }

        private void SetPendingTile(TileInstance tileData, TileBase tile)
        {
            Profiler.BeginSample("GetCellForTileCoord");
            Vector3Int cell = GetCellForTileCoord(tileData, Layer);
            Profiler.EndSample();
            
            Profiler.BeginSample("GetTileInstanceFlips");
            Matrix4x4 matrix = GetTileInstanceFlips(cell, tileData);
            Profiler.EndSample();
            
            Profiler.BeginSample("ConvertCellCoord");
            cell = ConvertCellCoord(cell);
            Profiler.EndSample();
            
            Profiler.BeginSample("GetNextCellZ");
            cell.z = _tilesetProvider.GetNextCellZ(cell);
            Profiler.EndSample();
            
            Profiler.BeginSample("SetPendingTile");
            _tilesetProvider.SetPendingTile(cell, tile);
            Profiler.EndSample();

            Profiler.BeginSample("SetColorAndMatrix");
            Color color = new Color(1, 1, 1, tileData.A);
            _tilesetProvider.SetColorAndMatrix(cell, ref color, ref matrix);
            Profiler.EndSample();
        }
        
        private Matrix4x4 GetTileInstanceFlips(Vector3Int cell, TileInstance tileData)
        {
            int gridSize = Layer.GridSize;

            int pxOffsetX = tileData.Px[0] - cell.x * gridSize;
            int pxOffsetY = tileData.Px[1] - cell.y * gridSize;
            
            Vector3 offset = Vector3.zero;
            offset.x = pxOffsetX / (float)gridSize;
            offset.y = -pxOffsetY / (float)gridSize;
            
            float scaleFactor = 1f / LayerScale;
            Vector3 scale = new Vector3(scaleFactor, scaleFactor, 1);
            scale.x *= tileData.FlipX ? -1 : 1;
            scale.y *= tileData.FlipY ? -1 : 1;

            return Matrix4x4.TRS(offset, Quaternion.identity, scale);
        }
    }
}
