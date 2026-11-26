using UnityEngine;

namespace LDtkUnity
{
    internal static class LDtkFindObjects
    {
        public static T[] FindObjectsByType<T>() where T : Object
        {
#if UNITY_6000_4_OR_NEWER
            return Object.FindObjectsByType<T>(FindObjectsInactive.Include);
#elif UNITY_2023_1_OR_NEWER
            return Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#elif UNITY_2020_1_OR_NEWER
            return Object.FindObjectsOfType<T>(true);
#else
            return Object.FindObjectsOfType<T>();
#endif
        }
    }
}