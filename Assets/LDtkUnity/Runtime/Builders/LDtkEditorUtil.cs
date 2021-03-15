using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LDtkUnity
{
    //doing a custom class to ensure our usages, also simpler to write the functions and the #if defines.
    //These need to be in the runtime assembly because the building system needs to be possible for runtime.
    //Later down the road we can do some listener patterns so that the editor responds accordingly without editor functionality living in the runtime assembly.
    public static class LDtkEditorUtil
    {
	    public static void Dirty(Object obj)
        {
#if UNITY_EDITOR
            
            if (obj == null)
            {
                Debug.LogWarning("Did not set object dirty; was null");
                return;
            }
            
            //Debug.Log($"Dirtied \"{obj.name}\"", obj);
            RecordObject(obj, obj.name);
#endif
        }
        
        //found from https://gist.github.com/lazlo-bonin/a85586dd37fdf7cf4971d93fa5d2f6f7
        private static void RecordObject(Object uo, string name)
		{
			if (uo == null)
			{
				return;
			}
			//Undo.RegisterCompleteObjectUndo(uo, name); // GOOD
			
			if (!IsSceneBound(uo))
			{
				EditorUtility.SetDirty(uo);
			}
			
			if (IsPrefabInstance(uo))
			{
				PrefabUtility.RecordPrefabInstancePropertyModifications(uo);
			}
		}

        private static bool IsSceneBound(Object uo)
        {
	        return
		        uo is GameObject && !IsPrefabDefinition(uo) ||
		        uo is Component component && !IsPrefabDefinition(component.gameObject);
        }

        private static bool IsPrefabInstance(Object uo)
        {
	        return GetPrefabDefinition(uo) != null;
        }

        private static Object GetPrefabDefinition(Object uo)
        {
	        return PrefabUtility.GetCorrespondingObjectFromSource(uo);
        }

        private static bool IsPrefabDefinition(Object uo)
        {
	        return GetPrefabDefinition(uo) == null && PrefabUtility.GetPrefabInstanceHandle(uo) != null;
        }
    }
}