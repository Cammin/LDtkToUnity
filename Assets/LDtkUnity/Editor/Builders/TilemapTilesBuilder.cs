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
        private readonly Dictionary<Vector3Int, TileBase> _tilesToBuild = new Dictionary<Vector3Int, TileBase>();
        private readonly Dictionary<Vector3Int, ExtraData> _extraData = new Dictionary<Vector3Int, ExtraData>();

        private class ExtraData
        {
            public Color? color;
            public Matrix4x4? matrix;

            public void ApplyExtraValues(Tilemap map, Vector3Int cell)
            {
                map.SetTileFlags(cell, TileFlags.None);
                
                if (color != null)
                {
                    map.SetColor(cell, color.Value);
                }

                if (matrix != null)
                {
                    map.SetTransformMatrix(cell, matrix.Value);
                }
            }
        }

        
        public TilemapTilesBuilder(Tilemap map)
        {
            Map = map;
        }
        
        public void SetPendingTile(Vector3Int cell, TileBase tileAsset)
        {
            if (_tilesToBuild.ContainsKey(cell))
            {
                LDtkDebug.Log("Tried adding a tile to a dict that already has that position");
                return;
            }
            _tilesToBuild.Add(cell, tileAsset);
        }

        public void ApplyPendingTiles()
        {
            Vector3Int[] cells = _tilesToBuild.Keys.ToArray();
            TileBase[] tiles = _tilesToBuild.Values.ToArray();
            Map.SetTiles(cells, tiles);
            Map.CompressBounds();
            
            //only applies to intgrid tile assets
            foreach (Vector3Int cell in cells)
            {
                if (_extraData.TryGetValue(cell, out var extra))
                {
                    extra.ApplyExtraValues(Map, cell);
                }
                else
                {
                    LDtkDebug.LogError("Didn't get the same bonus data?");
                }
                
                //for some reason a GameObject is instantiated causing two to exist in play mode; maybe because its the import process. destroy it
                GameObject instantiatedObject = Map.GetInstantiatedObject(cell);
                if (instantiatedObject != null)
                {
                    Object.DestroyImmediate(instantiatedObject);
                }
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
    }
}