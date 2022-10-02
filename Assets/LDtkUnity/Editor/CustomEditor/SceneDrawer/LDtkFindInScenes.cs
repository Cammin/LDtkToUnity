using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif

namespace LDtkUnity.Editor
{
    internal static class LDtkFindInScenes
    {
        public static List<T> FindInAllScenes<T>()
        {
            Profiler.BeginSample("FindInAllScenes");
            List<T> interfaces = new List<T>();

            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                TryAddToList(prefabStage.prefabContentsRoot, interfaces);
            }
            else
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (!scene.isLoaded)
                    {
                        continue;
                    }
                    
                    List<T> inScene = FindInScene<T>(scene);
                    foreach (T obj in inScene)
                    {
                        interfaces.Add(obj);
                    }
                }
            }

            Profiler.EndSample();
            return interfaces;
        }

        private static List<T> FindInScene<T>(Scene scene)
        {
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            List<T> interfaces = new List<T>();
            foreach(GameObject rootGameObject in rootGameObjects)
            {
                TryAddToList(rootGameObject, interfaces);
            }
            return interfaces;
        }

        private static void TryAddToList<T>(GameObject rootGameObject, List<T> interfaces)
        {
            T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
            foreach (T childInterface in childrenInterfaces)
            {
                interfaces.Add(childInterface);
            }
        }
    }
}