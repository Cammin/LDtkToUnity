using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.Tilemaps;

//todo: doesnt support SpriteMaskInteraction
//todo: doesn't support tile animation
namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.COMPONENT_CUSTOM_RENDERER)]
    [AddComponentMenu("")]
    [ExecuteAlways]
    public class LDtkTilemapRenderer : MonoBehaviour
    {
        internal const string TILEMAP = nameof(_tilemap); 
        internal const string FILTER = nameof(_filter); 
        internal const string RENDERER = nameof(_renderer);
        internal const string ORDERINGS = nameof(_orderings);
        
        internal Tilemap _tilemap;
        internal MeshFilter _filter;
        internal MeshRenderer _renderer;
        [SerializeField] private List<Vector3Int> _orderings;
    
        private LDtkTilemapRendererMesh _mesh;

        private void OnEnable()
        {
            Tilemap.tilemapTileChanged += OnTileChanged;
        }

        private void OnDisable()
        {
            Tilemap.tilemapTileChanged -= OnTileChanged;
        }

        
        private void Start()
        {
            /*if (Application.isPlaying)
            {
                TweakRuntimeMaterial();
            }*/
            BakeTilemapLayer();
        }
        
        private void OnDestroy()
        {
            _mesh?.OnDestroy();
        }
    
        private void Reset()
        {
            
        }
    
        private void OnValidate()
        {

        }
    
        internal void OnImport(Tilemap tilemap, MeshFilter filter, MeshRenderer meshRenderer, int sortingOrder)
        {
            _tilemap = tilemap;
            _filter = filter;
            _renderer = meshRenderer;
            _renderer.sortingOrder = sortingOrder;
        }

        internal void OnImportCoords(TileChangeData[] coords)
        {
            _orderings = new List<Vector3Int>(coords.Length);
            foreach (TileChangeData coord in coords)
            {
                _orderings.Add(coord.position);
            }
        }

        private void TweakMaterial()
        {
            Vector3Int cell = _orderings.FirstOrDefault();
            var usedSprite = _tilemap.GetSprite(cell);
            if (usedSprite == null)
            {
                LDtkDebug.LogError($"TilemapRenderer has no sprite at cell {cell}", gameObject);
                return;
            }
            //gets the atlas texture if there was one
            _renderer.sharedMaterial.mainTexture = usedSprite.texture;
        }
        
        private void OnTileChanged(Tilemap tilemap, Tilemap.SyncTile[] changes)
        {
            if (_orderings.IsNullOrEmpty()) return;
            if (!EnsureTilemap()) return;
            if (tilemap != _tilemap) return;
        
            //any new tiles will append the ordering to the front-most render order
            //todo: add a config to have the option on where the ordering should go
            foreach (var change in changes)
            {
                if (!_orderings.Contains(change.position))
                {
                    _orderings.Add(change.position);
                }
            }
            //todo have a system in place for this so that the mesh only has to change incrementally for a single tile change instead of rebuilding the mesh from scratch; store verts, etc
            BakeTilemapLayer();
        }
    
        [ContextMenu("Bake Tilemap Layer")]
        public void BakeTilemapLayer()
        {
            if (!EnsureTilemap()) return;
            if (!EnsureFilter()) return;
            if (!EnsureRenderer()) return;
        
            if (_orderings.IsNullOrEmpty())
            {
                Debug.LogWarning($"{nameof(LDtkTilemapRenderer)} cannot bake because _orderings is empty.", this);
                return;
            }
        
            _mesh ??= new LDtkTilemapRendererMesh();
            _filter.sharedMesh = _mesh.mesh;
            
            LDtkProfiler.BeginSample("BakeMesh");
            _mesh.Bake(_tilemap, _orderings);
            LDtkProfiler.EndSample();

            //ensure that the material is using the right texture
            TweakMaterial();
        }
    
        private bool EnsureTilemap()
        {
            if (_tilemap) return true;
        
            _tilemap = GetComponent<Tilemap>();
            if (_tilemap) return true;
        
            Debug.LogError($"{nameof(LDtkTilemapRenderer)} cannot find Tilemap component.", gameObject);
            return false;
        }
        private bool EnsureFilter()
        {
            if (_filter) return true;
        
            _filter = GetComponent<MeshFilter>();
            if (_filter) return true;
        
            Debug.LogError($"{nameof(LDtkTilemapRenderer)} cannot find MeshFilter component.", gameObject);
            return false;
        }

        internal bool EnsureRenderer()
        {
            if (_renderer) return true;
        
            _renderer = GetComponent<MeshRenderer>();
            if (_renderer) return true;
        
            Debug.LogError($"{nameof(LDtkTilemapRenderer)} cannot find MeshRenderer component.", gameObject);
            return false;
        }
    }
}