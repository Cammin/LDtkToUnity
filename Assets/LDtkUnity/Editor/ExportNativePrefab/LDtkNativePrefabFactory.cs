using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    public class LDtkNativePrefabFactory
    {
        private readonly LDtkNativePrefabAssets _assets;
        
        //components that we'd have
        private readonly List<Tilemap> _tilemaps = new List<Tilemap>();
        private readonly List<SpriteRenderer> _renderers = new List<SpriteRenderer>();

        private readonly Dictionary<TileBase, TileBase> _oldToNewTiles = new Dictionary<TileBase, TileBase>();
        private readonly Dictionary<Sprite, Sprite> _oldToNewBackgrounds = new Dictionary<Sprite, Sprite>();

        public LDtkNativePrefabFactory(LDtkNativePrefabAssets assets)
        {
            _assets = assets;
        }
        
        public GameObject CreateNativePrefabInstance(GameObject importRoot)
        {
            if (importRoot == null)
            {
                Debug.LogError("Null input");
                return null;
            }

            GameObject newRoot = (GameObject)PrefabUtility.InstantiatePrefab(importRoot);
            if (newRoot == null)
            {
                Debug.LogError("Null instantiation");
                return null;
            }

            PrefabUtility.UnpackPrefabInstance(newRoot, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
            TryRemove<LDtkComponentProject>(newRoot);
            TryRemove<LDtkDetachChildren>(newRoot);

            foreach (Transform level in newRoot.transform)
            {
                StripLevel(level.gameObject);
            }

            PopulateOldToNew();
            SwapOldToNew();

            return newRoot;
        }

        private void StripLevel(GameObject level)
        {
            TryRemove<LDtkDetachChildren>(level);
            TryRemove<LDtkFields>(level);
            TryRemove<LDtkComponentLevel>(level);

            foreach (Transform layer in level.transform)
            {
                StripLayer(layer.gameObject);
            }
        }

        private void StripLayer(GameObject layer)
        {
            //for entities root obj
            TryRemove<LDtkDetachChildren>(layer);
            
            TryCollectComponent(layer, _renderers, renderer => renderer.sprite != null);
            
            foreach (Transform layerElement in layer.transform)
            {
                StripLayerElements(layerElement.gameObject);
            }
        }
        
        private void StripLayerElements(GameObject layerElement)
        {
            //for entity
            TryRemove<LDtkFields>(layerElement);
            TryRemove<LDtkEntityDrawerComponent>(layerElement);
            
            TryCollectComponent(layerElement, _tilemaps);
        }

        private static void TryRemove<T>(GameObject obj) where T : Component
        {
            T[] components = obj.GetComponents<T>();
            if (components.IsNullOrEmpty())
            {
                return;
            }

            foreach (T component in components)
            {
                Object.DestroyImmediate(component);
            }
        }

        private delegate bool ComponentCheck<in T>(T input);
        private void TryCollectComponent<T>(GameObject obj, List<T> list, ComponentCheck<T> onlyIf = null)
        {
            if (!obj.TryGetComponent(out T component))
            {
                return;
            }

            if (onlyIf == null || onlyIf.Invoke(component))
            {
                list.Add(component);
            }
        }

        private void PopulateOldToNew()
        {
            foreach (Tilemap tilemap in _tilemaps)
            {
                TileBase[] tiles = tilemap.GetTilesBlock(tilemap.cellBounds);
                foreach (TileBase oldTile in tiles)
                {
                    if (oldTile == null || _oldToNewTiles.ContainsKey(oldTile))
                    {
                        return;
                    }

                    TileBase newTile = _assets.ArtTiles.Concat(_assets.IntGridTiles).FirstOrDefault(newTile => newTile.name == oldTile.name);
                    if (newTile == null)
                    {
                        Debug.LogError("Problem getting a new tile, they should always exist");
                        continue;
                    }
                    _oldToNewTiles.Add(oldTile, newTile);
                }
            }

            foreach (SpriteRenderer renderer in _renderers)
            {
                if (renderer == null)
                {
                    Debug.LogError("null renderer");
                }

                Sprite oldBg = renderer.sprite;
                if (oldBg == null)
                {
                    continue; //ignore if there was no sprite to begin with
                }
                
                if (oldBg == null || _oldToNewBackgrounds.ContainsKey(oldBg))
                {
                    return;
                }

                Sprite newBg = _assets.BackgroundArtifacts.FirstOrDefault(newBg => newBg.name == oldBg.name);
                if (newBg == null)
                {
                    Debug.LogError("Problem getting a new background, they should always exist");
                    continue;
                }
                _oldToNewBackgrounds.Add(oldBg, newBg);
            }
        }
        
        private void SwapOldToNew()
        {
            //tilemaps
            foreach (Tilemap tilemap in _tilemaps)
            {
                foreach (TileBase oldTile in _oldToNewTiles.Keys)
                {
                    if (!tilemap.ContainsTile(oldTile))
                    {
                        continue;
                    }
                    
                    TileBase newTile = _oldToNewTiles[oldTile];
                    tilemap.SwapTile(oldTile, newTile);
                }
            }
            
            //background renderers
            foreach (SpriteRenderer renderer in _renderers)
            {
                if (renderer.sprite == null)
                {
                    continue;
                }
                
                foreach (Sprite oldBg in _oldToNewBackgrounds.Keys)
                {
                    if (renderer.sprite != oldBg)
                    {
                        continue;
                    }
                    
                    Sprite newBg = _oldToNewBackgrounds[oldBg];
                    renderer.sprite = newBg;
                }
            }
        }



    }
}