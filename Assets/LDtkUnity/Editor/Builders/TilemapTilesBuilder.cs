using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// A wrapper on the tilemap that is only meant to cache the tiles and positions until we call SetTiles in one performant fell swoop
    /// After setting all tiles, then we apply any extra info like flags, color, transform because it only works after a tile was set
    /// </summary>
    internal sealed class TilemapTilesBuilder
    {
        public readonly Tilemap Map;
        private readonly Dictionary<Vector3Int, TileBase> _tilesToBuild;
        private readonly Dictionary<Vector3Int, int> _depth;
        private readonly Dictionary<Vector3Int, ExtraData> _extraData;

        private class ExtraData
        {
            public Color? color;
            public Matrix4x4? matrix;

            public void ApplyExtraValues(Tilemap map, Vector3Int cell)
            {
                //avoiding this api call. tilemaps are none by default anyway
                //map.SetTileFlags(cell, TileFlags.None);
                
                //only do the tilemap api calls when necessary, as it could get expensive
                if (color != null && color.Value != Color.white)
                {
                    map.SetColor(cell, color.Value);
                }
                if (matrix != null && matrix.Value != Matrix4x4.identity)
                {
                    map.SetTransformMatrix(cell, matrix.Value);
                }
            }
        }
        
        public TilemapTilesBuilder(Tilemap map, int capacity)
        {
            Map = map;
            _tilesToBuild = new Dictionary<Vector3Int, TileBase>(capacity);
            _extraData = new Dictionary<Vector3Int, ExtraData>(capacity);
            
            //in most realistic situations, tiles will not overlap, but we can overestimate anyways to avoid resizing 
            _depth = new Dictionary<Vector3Int, int>(10);
        }
        
        public int GetNextCellZ(Vector3Int cell)
        {
            if (!_depth.ContainsKey(cell))
            {
                _depth.Add(cell, 0);
                return 0;
            }

            _depth[cell] += 1;
            return _depth[cell];
        }
        
        public void SetPendingTile(Vector3Int cell, TileBase tileAsset)
        {
            //if we try placing a tile on top of a spot that already occupies a tile, then increment z
            _tilesToBuild.Add(cell, tileAsset);
        }

        public void ApplyPendingTiles(bool isIntGrid)
        {
            LDtkProfiler.BeginSample("ToArray Keys&Values");
            Vector3Int[] cells = _tilesToBuild.Keys.ToArray();
            TileBase[] tiles = _tilesToBuild.Values.ToArray();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("Tilemap.SetTiles");
            Map.SetTiles(cells, tiles);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("CompressBounds");
            Map.CompressBounds();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("ApplyExtraData");
            ApplyExtraData();
            LDtkProfiler.EndSample();

            if (!isIntGrid)
            {
                return;
            }
            
            LDtkProfiler.BeginSample("TryDestroyExtra");
            //for some reason a GameObject is instantiated causing two to exist in play mode; maybe because it's the import process. destroy it
            foreach (Vector3Int cell in cells)
            {
                GameObject instantiatedObject = Map.GetInstantiatedObject(cell);
                if (instantiatedObject != null)
                {
                    Object.DestroyImmediate(instantiatedObject);
                }
            }
            LDtkProfiler.EndSample();
        }

        public void ApplyExtraData()
        {
            foreach (KeyValuePair<Vector3Int,ExtraData> pair in _extraData)
            {
                pair.Value.ApplyExtraValues(Map, pair.Key);
            }
        }

        public void SetColor(Vector3Int cell, Color color)
        {            
            if (!_extraData.ContainsKey(cell))
            {
                _extraData.Add(cell, new ExtraData());
            }
            _extraData[cell].color = color;
        }
        
        public void SetTransformMatrix(Vector3Int cell, Matrix4x4 matrix)
        {
            if (!_extraData.ContainsKey(cell))
            {
                _extraData.Add(cell, new ExtraData());
            }
            _extraData[cell].matrix = matrix;
        }
        
        public void SetColorAndMatrix(Vector3Int cell, ref Color color, ref Matrix4x4 matrix)
        {            
            _extraData.Add(cell, new ExtraData());
            _extraData[cell].color = color;
            _extraData[cell].matrix = matrix;
        }
    }
}