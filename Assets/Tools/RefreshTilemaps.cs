using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal static class RefreshTilemaps
    {
        [MenuItem("LDtkUnity/Refresh Tilemaps", false, 10)]
        private static void UpdateTilemaps()
        {
            Tilemap[] tilemaps = Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
            foreach (Tilemap map in tilemaps)
            {
                Debug.Log(map.gameObject.name);
                map.RefreshAllTiles();
            }
        }
        [MenuItem("LDtkUnity/Refresh Tilemap Colliders", false, 10)]
        private static void UpdateTilemapColliders()
        {
            LDtkPostImportSceneAlterations.QueueTilemapColliderSmartReset();
        }

    }
}