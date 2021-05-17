using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LDtkUnity
{
    //doing a custom class to ensure our usages, also simpler to write the functions and the #if defines.
    //These need to be in the runtime assembly because the building system needs to be possible for runtime.
    //Later down the road we can do some listener patterns so that the editor responds accordingly without editor functionality living in the runtime assembly.
    /*public static class LDtkEditorUtil
    {
	    public static void Dirty(Object obj)
        {
#if UNITY_EDITOR
            
            if (obj == null)
            {
                Debug.LogWarning("Did not set object dirty; was null");
                return;
            }

            RecordObject(obj, obj.name);
#endif
        }
        
        //found from https://gist.github.com/lazlo-bonin/a85586dd37fdf7cf4971d93fa5d2f6f7
        private static void RecordObject(Object uo, string name)
		{
#if UNITY_EDITOR
			
			if (uo == null)
			{
				return;
			}

			if (!IsSceneBound(uo))
			{
				EditorUtility.SetDirty(uo);
			}
			
			if (IsPrefabInstance(uo))
			{
				PrefabUtility.RecordPrefabInstancePropertyModifications(uo);
			}
#endif
		}

        private static bool IsSceneBound(Object uo)
        {
#if UNITY_EDITOR
	        return 
		        uo is GameObject && !IsPrefabDefinition(uo) ||
		        uo is Component component && !IsPrefabDefinition(component.gameObject);
#else
	        return default;
#endif
        }

        private static bool IsPrefabInstance(Object uo)
        {
#if UNITY_EDITOR
	        return GetPrefabDefinition(uo) != null;
#else
	        return default;
#endif
        }

        private static Object GetPrefabDefinition(Object uo)
        {
#if UNITY_EDITOR
	        return PrefabUtility.GetCorrespondingObjectFromSource(uo);
#else
	        return default;
#endif
        }

        private static bool IsPrefabDefinition(Object uo)
        {
#if UNITY_EDITOR
	        return GetPrefabDefinition(uo) == null && PrefabUtility.GetPrefabInstanceHandle(uo) != null;
#else
	        return default;
#endif
        }
    }*/
}