using UnityEngine;

namespace LDtkUnity.Tools
{
    public static class LDtkPrefabFactory
    {
        public static T Instantiate<T>(T prefab) where T : Object
        {
            Object entityObj = null;
            if (Application.isPlaying)
            {
                entityObj = Object.Instantiate(prefab);
            }
            else
            {
#if UNITY_EDITOR
                entityObj = UnityEditor.PrefabUtility.InstantiatePrefab(prefab);
#endif
            }

            if (entityObj != null)
            {
                return (T)entityObj;
            }
            
            Debug.LogError("Entity null");
            return null;
        }
    }
}