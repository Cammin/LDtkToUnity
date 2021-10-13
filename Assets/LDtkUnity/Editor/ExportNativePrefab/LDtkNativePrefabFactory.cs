using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkNativePrefabFactory
    {
        public GameObject ExportNativePrefab(GameObject importRoot)
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

            if (layerElement.TryGetComponent<Tilemap>(out var tilemap))
            {
                IEnumerable<TileBase> tiles = tilemap.GetTilesBlock(tilemap.cellBounds).Distinct();
                
                foreach (TileBase tile in tiles)
                {
                    if (tile == null)
                    {
                        Debug.Log("Null");
                        return;
                    }
                    Debug.Log(tile);
                }
            }
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

        
    }
}