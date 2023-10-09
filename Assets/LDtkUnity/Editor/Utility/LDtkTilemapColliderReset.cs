using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal static class LDtkTilemapColliderReset
    {
        private static bool _willRefreshTilemapsInScene;
        
        public static void TilemapColliderTileUpdate()
        {
            //Refresh tilemap colliders in the current scene.
            //Tiles would normally not update in the scene view until entering play mode, or reloading the scene, or resetting the component.
            //This will immediately update it. 
            //There is currently no easy solution found for refreshing tilemap colliders in the scene, so this is the best solution I could find for now.
            //Related forum: https://forum.unity.com/threads/ispritephysicsoutlinedataprovider-is-not-updating-the-tilemapcollider2d-in-the-scene-immediately.1458874/#post-9358610
            
            if (_willRefreshTilemapsInScene)
            {
                return;
            }
            _willRefreshTilemapsInScene = true;

            EditorApplication.delayCall += () =>
            {
                _willRefreshTilemapsInScene = false;
                
                Stopwatch watch = Stopwatch.StartNew();
                
                //should only try resetting tilemaps that are in an LDtk hierarchy 
                var levels = LDtkFindInScenes.FindInAllScenes<LDtkComponentLevel>();
                if (levels.IsNullOrEmpty())
                {
                    watch.Stop();
                    return;
                }
                
                var layers = levels.SelectMany(p => p.GetComponentsInChildren<LDtkComponentLayer>()).ToList();
                if (layers.IsNullOrEmpty())
                {
                    watch.Stop();
                    return;
                }

                var colliders = layers.SelectMany(p => p.GetComponentsInChildren<TilemapCollider2D>()).ToList();
                int affected = 0;
                foreach (var collider in colliders)
                {
                    Unsupported.SmartReset(collider);
                    if (LDtkBuilderLayer.ConfigureTilemapCollider(collider))
                    {
                        affected++;
                    }
                }
                watch.Stop();

                SceneView view = SceneView.lastActiveSceneView;
                if (view != null && affected > 0)
                {
                    float seconds = watch.ElapsedMilliseconds * 0.001f;
                    view.ShowNotification(new GUIContent($"Refreshed LDtk scene tilemaps\n({affected} in {seconds:F2}s)"), 2.5f);
                }
            };
        }
    }
}