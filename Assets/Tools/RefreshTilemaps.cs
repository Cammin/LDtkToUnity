using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal static class RefreshTilemaps
    {
        [MenuItem("LDtkUnity/Refresh Tilemaps", false, 10)]
        private static void UpdateTilemaps()
        {
            Tilemap[] tilemaps = Object.FindObjectsOfType<Tilemap>();
            foreach (Tilemap map in tilemaps)
            {
                Debug.Log(map.gameObject.name);
                map.RefreshAllTiles();
            }
        }
        [MenuItem("LDtkUnity/Refresh Tilemap Colliders", false, 10)]
        private static void UpdateTilemapColliders()
        {
            TilemapCollider2D[] colliders = Object.FindObjectsOfType<TilemapCollider2D>();
            foreach (TilemapCollider2D collider in colliders)
            {
                Unsupported.SmartReset(collider);
                PrefabUtility.RevertObjectOverride(collider, InteractionMode.AutomatedAction);
            }
        }

    }
}