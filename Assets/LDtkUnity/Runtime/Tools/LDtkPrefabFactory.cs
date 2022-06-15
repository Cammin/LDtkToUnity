using UnityEngine;

namespace LDtkUnity
{
    internal static class LDtkPrefabFactory
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
            
            LDtkDebug.LogError("Entity null when trying to instantiate");
            return null;
        }
    }
}