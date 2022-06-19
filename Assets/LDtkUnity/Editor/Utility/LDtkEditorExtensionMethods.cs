using UnityEditor;
using UnityEditor.U2D;
using UnityEngine.U2D;

namespace LDtkUnity.Editor
{
    internal static class LDtkEditorExtensionMethods
    {
        public static SerializedProperty DrawField(this SerializedObject obj, string propName)
        {
            SerializedProperty prop = obj.FindProperty(propName);
            EditorGUILayout.PropertyField(prop);
            return prop;
        }
        
        public static void RemoveAll(this SpriteAtlas atlas)
        {
            atlas.Remove(atlas.GetPackables());
        }
        
        public static SerializedProperty[] GetArrayElements(this SerializedProperty prop)
        {
            if (!prop.isArray)
            {
                LDtkDebug.LogError("SerializedProperty was not an array");
                return null;
            }
            
            SerializedProperty[] array = new SerializedProperty[prop.arraySize];
            for (int i = 0; i < prop.arraySize; i++)
            {
                array[i] = prop.GetArrayElementAtIndex(i);
            }

            return array;
        }
    }
}