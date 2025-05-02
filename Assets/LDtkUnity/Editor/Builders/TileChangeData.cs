#if !UNITY_2021_2_OR_NEWER
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// This artificial struct is for use before Unity 2021.2 where the better tilemap API is introduced for SetTiles with TileChangeData.
    /// </summary>
    internal struct TileChangeData
    {
        public Vector3Int position;
        public TileBase tile;
        public Color color;
        public Matrix4x4 transform;

        public TileChangeData(Vector3Int position, TileBase tile, Color color, Matrix4x4 transform)
        {
            this.position = position;
            this.tile = tile;
            this.color = color;
            this.transform = transform;
        }

        public static void Apply(Tilemap tilemap, TileChangeData[] datas)
        {
            Vector3Int[] positions = new Vector3Int[datas.Length];
            TileBase[] tiles = new TileBase[datas.Length];
                
            for (int i = 0; i < datas.Length; i++)
            {
                positions[i] = datas[i].position;
                tiles[i] = datas[i].tile;
            }

            tilemap.SetTiles(positions, tiles);
            
            foreach (TileChangeData data in datas)
            {
                tilemap.SetTransformMatrix(data.position, data.transform);
                tilemap.SetColor(data.position, data.color);
            }
        }
    }
}
#endif