using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Sprites;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

namespace LDtkUnity
{
    //Wrapper on the mesh that handles baking etc
    internal class LDtkTilemapRendererMesh
    {
        public Mesh mesh;
        
        private readonly List<Vector3> _vertices = new List<Vector3>();
        private readonly List<Vector2> _uvs = new List<Vector2>();
        private readonly List<Color> _colors = new List<Color>();
        private readonly List<int> _triangles = new List<int>();
        
        public LDtkTilemapRendererMesh()
        {
            mesh = new Mesh
            {
                name = "LDtkTilemapRenderer Mesh",
                hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor,
            };
            mesh.MarkDynamic();
        }
        
        public void Clear() 
        {
            mesh.Clear();
            
            _vertices.Clear();
            _uvs.Clear();
            _colors.Clear();
            _triangles.Clear();
        }

        public void OnDestroy() 
        {
            if (mesh != null) 
            {
                if (Application.isEditor && !Application.isPlaying)
                {
                    Object.DestroyImmediate(mesh);
                }
                else
                {
                    Object.Destroy(mesh);
                }
            }
            mesh = null;
        }

        public void Bake(Tilemap tilemap, List<Vector3Int> orderedCells)
        {
            Clear();
            
            LDtkProfiler.BeginSample("PopulateData");
            PopulateData(tilemap, orderedCells);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("ApplyMesh");
            ApplyMesh();
            LDtkProfiler.EndSample();
        }
        
        private void PopulateData(Tilemap tilemap, List<Vector3Int> orderedCells)
        {
            //todo maybe this could be dispatched as a job
            //todo maybe use this for optimizing: tilemap.GetTilesRangeNonAlloc()
            
            //spriteVertices: 4, spriteUvs: 4, spriteTriangles: 6
            int tileCount = orderedCells.Count;
            _vertices.Capacity = tileCount * 4;
            _uvs.Capacity = tileCount * 4;
            _colors.Capacity = tileCount * 4;
            _triangles.Capacity = tileCount * 6;
            
            foreach (Vector3Int cell in orderedCells)
            {
                Sprite sprite = tilemap.GetSprite(cell);

                if (sprite == null)
                {
                    LDtkDebug.LogWarning($"Tilemap has an ordered cell at {cell} but no sprite is there", tilemap.gameObject);
                    continue;
                }

                Vector2[] spriteVertices = sprite.vertices;
                ushort[] spriteTriangles = sprite.triangles;
                Vector2[] spriteUvs = sprite.uv;
                
                //
                /*Vector4 atlasUVs = DataUtility.GetOuterUV(sprite);
                spriteUvs = new Vector2[]
                {
                    new Vector2(atlasUVs.x, atlasUVs.y), // Bottom-Left
                    new Vector2(atlasUVs.x, atlasUVs.w), // Top-Left
                    new Vector2(atlasUVs.z, atlasUVs.w), // Top-Right
                    new Vector2(atlasUVs.z, atlasUVs.y)  // Bottom-Right
                };*/

                /*if (Application.isPlaying)
                {
                    Vector4 atlasUVs = DataUtility.GetOuterUV(sprite);
                    spriteUvs = new Vector2[]
                    {
                        new Vector2(atlasUVs.x, atlasUVs.y), // Bottom-Left
                        new Vector2(atlasUVs.x, atlasUVs.w), // Top-Left
                        new Vector2(atlasUVs.z, atlasUVs.w), // Top-Right
                        new Vector2(atlasUVs.z, atlasUVs.y)  // Bottom-Right
                    };
                }
                else
                {
                    spriteUvs = sprite.uv;
                }*/

                //var atlas = new UnityEngine.U2D.SpriteAtlas();
                //atlas.

                int baseVertex = _vertices.Count;
                Matrix4x4 transform = tilemap.GetTransformMatrix(cell);
                Vector3 cellCenter = tilemap.GetCellCenterLocal(cell);
                Color color = tilemap.GetColor(cell);

                for (int i = 0; i < spriteVertices.Length; i++)
                {
                    Vector3 spriteVertex = spriteVertices[i];
                    Vector3 transformedVertex = cellCenter + transform.MultiplyPoint3x4(spriteVertex) * (1.001f);

                    _vertices.Add(transformedVertex);
                    _uvs.Add(spriteUvs[i]);
                    _colors.Add(color);
                }
                
                for (int i = 0; i < spriteTriangles.Length; i++)
                {
                    _triangles.Add(baseVertex + spriteTriangles[i]);
                }
            }
        }

        private void ApplyMesh(bool recalculateNormals = false)
        {
            if (_vertices.Count == 0)
            {
                return;
            }

            mesh.indexFormat = _vertices.Count > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16;
            mesh.SetVertices(_vertices);
            mesh.SetUVs(0, _uvs);
            mesh.SetColors(_colors);
            mesh.SetTriangles(_triangles, 0, true);

            if (recalculateNormals)
            {
                mesh.RecalculateNormals();
            }

            mesh.RecalculateBounds();
        }
    }
}