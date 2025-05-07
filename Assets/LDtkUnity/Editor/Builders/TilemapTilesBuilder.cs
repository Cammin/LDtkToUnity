using System.Collections.Generic;
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
        private readonly List<TileChangeData> _tilesToBuild;
        
        public TilemapTilesBuilder(Tilemap map, int capacity)
        {
            Map = map;
            _tilesToBuild = new List<TileChangeData>(capacity);
        }
        
        public void AddTileChangeData(ref TileChangeData data)
        {
            //if we try placing a tile on top of a spot that already occupies a tile, then increment z
            _tilesToBuild.Add(data);
        }

        public void ApplyIntGridTiles()
        {
            LDtkProfiler.BeginSample("ToArray Keys&Values");
            TileChangeData[] tiles = _tilesToBuild.ToArray();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("Tilemap.SetTiles");
            SetTiles(Map, tiles);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("CompressBounds");
            Map.CompressBounds();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("TryDestroyExtra");
            //for some reason a GameObject is instantiated causing two to exist in play mode; maybe because it's the import process. destroy it
            foreach (TileChangeData cell in tiles)
            {
                GameObject instantiatedObject = Map.GetInstantiatedObject(cell.position);
                if (instantiatedObject != null)
                {
                    Object.DestroyImmediate(instantiatedObject);
                }
            }
            LDtkProfiler.EndSample();
        }

        public static void SetTiles(Tilemap tilemap, TileChangeData[] tiles)
        {
#if UNITY_2021_2_OR_NEWER
            tilemap.SetTiles(tiles, true);
#else
            TileChangeData.Apply(tilemap, tiles);
#endif
        }
    }
}