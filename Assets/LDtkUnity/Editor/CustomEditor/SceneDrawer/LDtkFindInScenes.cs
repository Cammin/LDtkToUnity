using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LDtkUnity.Editor
{
    public static class LDtkFindInScenes
    {
        public static List<T> FindInAllScenes<T>()
        {
            List<T> interfaces = new List<T>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                List<T> inScene = FindInScene<T>(scene);
                foreach (T obj in inScene)
                {
                    interfaces.Add(obj);
                }
            }
            return interfaces;
        }

        public static List<T> FindInScene<T>(Scene scene)
        {
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            List<T> interfaces = new List<T>();
            foreach(GameObject rootGameObject in rootGameObjects)
            {
                T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
                foreach(T childInterface in childrenInterfaces)
                {
                    interfaces.Add(childInterface);
                }
            }
            return interfaces;
        }
    }
}