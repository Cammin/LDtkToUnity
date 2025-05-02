using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// This job is for calculating the tile matrix and cell position for each tile in a layer.
    /// </summary>
    internal struct TileBuildingJob : IJobFor
    {
        public struct InputData
        {
            public int CoordId;
            public int PxX;
            public int PxY;
            public int Flip;
        }

        public struct OutputData
        {
            public Vector3Int Cell;
            public Matrix4x4 Matrix;
        }
        
        [ReadOnly] public NativeArray<InputData> Input;
        [WriteOnly] public NativeArray<OutputData> Output;
        
        [ReadOnly] private int LayerGridSize;
        [ReadOnly] private int LayerCWid;
        [ReadOnly] private int LayerCHei;
        [ReadOnly] private float ScaleFactor;

        public TileBuildingJob(TileInstance[] tiles, LayerInstance layer, float layerScale)
        {
            LayerGridSize = layer.GridSize;
            LayerCWid = layer.CWid;
            LayerCHei = layer.CHei;
            ScaleFactor = 1 / layerScale;
            
            int tilesCount = tiles.Length;
            Input = new NativeArray<InputData>(tilesCount, Allocator.TempJob);
            Output = new NativeArray<OutputData>(tilesCount, Allocator.TempJob);

            bool isAutoLayer = layer.IsAutoLayer;
            for (int i = 0; i < tilesCount; i++)
            {
                TileInstance tile = tiles[i];
                Input[i] = new InputData
                {
                    CoordId = isAutoLayer ? tile.D[1] : tile.D[0],
                    PxX = tile.Px[0],
                    PxY = tile.Px[1],
                    Flip = tile.F,
                };
            }
        }
        
        public void Execute(int i)
        {
            InputData input = Input[i];

            int cX = input.CoordId % LayerCWid;
            int cY = input.CoordId / LayerCWid;
            
            int pxOffsetX = input.PxX - cX * LayerGridSize;
            int pxOffsetY = input.PxY - cY * LayerGridSize;
            
            Vector3 offset = Vector3.zero;
            offset.x = pxOffsetX / (float)LayerGridSize;
            offset.y = -pxOffsetY / (float)LayerGridSize;
            
            //Rules can have multiple tiles built (like a 2x2 of art), but they all occupy the same coordId despite being located full cell(s) away!
            //this results in offsets that can exceed 1 or -1, which at that point, should occupy the next cell over.
            //not only is it easier to track down the tile in the editor, but it renders in a better z order with other tiles in that other cell.
            int cellShiftX = offset.x > 0 ? (int)Math.Floor(offset.x) : (int)Math.Ceiling(offset.x);
            int cellShiftY = offset.y > 0 ? (int)Math.Floor(offset.y) : (int)Math.Ceiling(offset.y);
            cX += cellShiftX;
            cY -= cellShiftY;
            offset.x -= cellShiftX;
            offset.y -= cellShiftY;
            
            bool flipX = (input.Flip & 1) == 1;
            bool flipY = (input.Flip & 2) == 2;
            
            Vector3 scale = new Vector3(ScaleFactor, ScaleFactor, 1);
            scale.x *= flipX ? -1 : 1;
            scale.y *= flipY ? -1 : 1;
            
            //convert y into unity tilemap coordinate space
            cY = -cY + LayerCHei - 1;
            
            Output[i] = new OutputData()
            {
                Cell = new Vector3Int(cX, cY, 0),
                Matrix = Matrix4x4.TRS(offset, Quaternion.identity, scale),
            };
        }
    }
}