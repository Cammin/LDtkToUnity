using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// class for altering stuff in the active scenes
    /// </summary>
    internal static class LDtkPostImportSceneAlterations
    {
        private static bool _willRefreshTilemapsInScene;
        private static bool _willRevertOverrides;
        private static InteractionMode _interaction;
        
        public static void QueueTilemapColliderSmartReset()
        {
            //Refresh tilemap colliders in the current scene.
            //Tiles would normally not update in the scene view until entering play mode, or reloading the scene, or resetting the component.
            //This will immediately update it. 
            //There is currently no easy solution found for refreshing tilemap colliders in the scene, so this is the best solution I could find for now.
            //Related forum: https://forum.unity.com/threads/ispritephysicsoutlinedataprovider-is-not-updating-the-tilemapcollider2d-in-the-scene-immediately.1458874/#post-9358610
            
            if (!_willRefreshTilemapsInScene)
            {
                TrySetupDeferredEvent();
                _willRefreshTilemapsInScene = true;
            }
        }

        public static void QueueRevertPrefabs(InteractionMode interaction)
        {
            //Revert prefab overrides in the current scene.
            //This is by preference for when unity unintentionally dirties the project/level automatically, and the user only wants to maintain a completely immutable project/level
            _interaction = interaction;
            if (!_willRevertOverrides)
            {
                TrySetupDeferredEvent();
                _willRevertOverrides = true;
            }
        }

        private static void TrySetupDeferredEvent()
        {
            if (!_willRefreshTilemapsInScene && !_willRevertOverrides)
            {
                EditorApplication.delayCall += DoAReset;
            }
        }

        private static void DoAReset()
        {
            //both queued events try a level find, so it's shared here in case it can be reused for both needs at the same time
            var levels = LDtkFindInScenes.FindInAllScenes<LDtkComponentLevel>();

            RevertPrefabOverrides();
            SmartResetTilemaps();
            
            _willRefreshTilemapsInScene = false;
            _willRevertOverrides = false;

            void RevertPrefabOverrides()
            {
                if (!_willRevertOverrides) return;
                
                //.ldtk instances
                var projects = LDtkFindInScenes.FindInAllScenes<LDtkComponentProject>();
                foreach (LDtkComponentProject project in projects)
                {
                    GameObject projectGameObject = project.gameObject;
                    if (PrefabUtility.IsOutermostPrefabInstanceRoot(projectGameObject))
                    {
                        PrefabUtility.RevertPrefabInstance(projectGameObject, _interaction);
                    }
                }
                    
                //.ldtkl instances
                foreach (LDtkComponentLevel level in levels)
                {
                    GameObject levelGameObject = level.gameObject;
                    if (PrefabUtility.IsOutermostPrefabInstanceRoot(levelGameObject))
                    {
                        PrefabUtility.RevertPrefabInstance(levelGameObject, _interaction);
                    }
                }
            }
            
            void SmartResetTilemaps()
            {
                if (!_willRefreshTilemapsInScene) return;
                
                Stopwatch watch = Stopwatch.StartNew();
                int affected = 0;
                
                //should only try resetting tilemaps that are in an LDtk hierarchy 
                foreach (LDtkComponentLevel level in levels)
                {
                    if (!level) continue;
                    
                    foreach (LDtkComponentLayer layer in level.LayerInstances)
                    {
                        if (!layer) continue;
                        
                        var colliders = layer.GetComponentsInChildren<TilemapCollider2D>();
                        foreach (var collider in colliders)
                        {
                            Unsupported.SmartReset(collider);
                            if (LDtkBuilderLayer.ConfigureTilemapCollider(collider))
                            {
                                affected++;
                            }
                        }
                    }
                }
                
                watch.Stop();

                if (affected <= 0)
                {
                    return;
                }
                
                float seconds = watch.ElapsedMilliseconds * 0.001f;
                SceneView view = SceneView.lastActiveSceneView;
                string msg = $"Refreshed LDtk scene tilemaps\n({affected} in {seconds:F2}s)";

                if (view != null)
                {
                    view.ShowNotification(new GUIContent(msg), 2.5f);
                }

                if (LDtkPrefs.VerboseLogging)
                {
                    LDtkDebug.Log(msg);
                }
            }
        }
    }
}