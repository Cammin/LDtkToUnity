using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection
{
    public static class LDtkInjectionErrorContext
    {
        public static Object Context;

#if UNITY_2019_2_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void Dispose()
        {
            Context = null;
        }

        public static void SetLogErrorContext(Object obj)
        {
            Context = obj;
        }
    }
}