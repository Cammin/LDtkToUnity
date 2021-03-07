using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public abstract class AutoAssetLinker<T>
    {
        protected abstract string ButtonText { get; }
        protected abstract string GetRelPath(T definition);
        
        public void DrawButton(SerializedProperty arrayProp, T[] defs, LDtkProjectFile projectAsset)
        {
            if (arrayProp.arraySize <= 0)
            {
                return;
            }
            
            if (!GUILayout.Button(ButtonText))
            {
                return;
            }

            for (int i = 0; i < defs.Length; i++)
            {
                T def = defs[i];
                
                SerializedProperty arrayElementProp = arrayProp.GetArrayElementAtIndex(i);
                SerializedProperty assetOfKeyValueProp = arrayElementProp.FindPropertyRelative(LDtkAsset.PROP_ASSET);
                
                Object foundAsset = LDtkRelPath.GetAssetRelativeToAsset<Texture2D>(projectAsset, GetRelPath(def));
                assetOfKeyValueProp.objectReferenceValue = foundAsset;

                arrayElementProp.serializedObject.ApplyModifiedProperties();
            }
            
            //Debug.Log("Linked assets");

            arrayProp.serializedObject.ApplyModifiedProperties();
        }
    }
}