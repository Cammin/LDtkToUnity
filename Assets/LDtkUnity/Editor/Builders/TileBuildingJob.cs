using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal struct TileBuildingJob : IJobFor
    {
        public struct InputData
        {
            public int CoordId;
            public int PxX;
            public int PxY;
            public bool FlipX;
            public bool FlipY;
        }

        public struct OutputData
        {
            public Vector3Int Cell;
            public Matrix4x4 Matrix;
        }
        
        [ReadOnly] public NativeArray<InputData> Input;
        [WriteOnly] public NativeArray<OutputData> Output;
        
        [ReadOnly] public int LayerGridSize;
        [ReadOnly] public int LayerCWid;
        [ReadOnly] public int LayerCHei;
        [ReadOnly] public float ScaleFactor;

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
                    FlipX = tile.FlipX,
                    FlipY = tile.FlipY,
                };
            }
        }
        
        public void Execute(int i)
        {
            int cX = Input[i].CoordId % LayerCWid;
            int cY = Input[i].CoordId / LayerCWid;
            
            int pxOffsetX = Input[i].PxX - cX * LayerGridSize;
            int pxOffsetY = Input[i].PxY - cY * LayerGridSize;
            
            Vector3 offset = Vector3.zero;
            offset.x = pxOffsetX / (float)LayerGridSize;
            offset.y = -pxOffsetY / (float)LayerGridSize;
            
            Vector3 scale = new Vector3(ScaleFactor, ScaleFactor, 1);
            scale.x *= Input[i].FlipX ? -1 : 1;
            scale.y *= Input[i].FlipY ? -1 : 1;
            
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