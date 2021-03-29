using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public abstract class AutoAssetLinker<TData, TAsset> where TAsset : Object
    {
        protected abstract GUIContent ButtonContent { get; }
        protected abstract string GetRelPath(TData definition);

        public void DrawButton(SerializedProperty arrayProp, TData[] defs, LDtkProjectFile projectAsset)
        {
            if (arrayProp.arraySize <= 0)
            {
                return;
            }

            if (!GUILayout.Button(ButtonContent))
            {
                return;
            }

            for (int i = 0; i < defs.Length; i++)
            {
                TData def = defs[i];
                
                SerializedProperty arrayElementProp = arrayProp.GetArrayElementAtIndex(i);
                SerializedProperty assetOfKeyValueProp = arrayElementProp.FindPropertyRelative(LDtkAsset.PROP_ASSET);
                
                Object foundAsset = GetAssetRelativeToJsonProject(projectAsset, GetRelPath(def));
                assetOfKeyValueProp.objectReferenceValue = foundAsset;

                arrayElementProp.serializedObject.ApplyModifiedProperties();
            }
            
            arrayProp.serializedObject.ApplyModifiedProperties();
        }
        
        /// <summary>
        /// Used for LDtk's json relative paths.
        /// </summary>
        private TAsset GetAssetRelativeToJsonProject(LDtkProjectFile asset, string relPath)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string directory = Path.GetDirectoryName(assetPath);
            
            string assetsPath = $"{directory}/{relPath}";
            
            //simplify double dots
            assetsPath = SimplifyPathWithDoubleDots(assetsPath);
            
            //replace backslash with forwards
            assetsPath = assetsPath.Replace("\\", "/");

            TAsset assetAtPath = AssetDatabase.LoadAssetAtPath<TAsset>(assetsPath);

            if (assetAtPath == null)
            {
                Debug.LogError($"LDtk: Could not find an asset in the path relative to \"{asset.name}\": \"{relPath}\". " +
                               $"Is the asset also locatable by LDtk, and is the asset located in the Unity Project?", asset);
            }

            return assetAtPath;
        }
        
        private static string SimplifyPathWithDoubleDots(string inputPath)
        {
            string fullPath = Path.GetFullPath(inputPath);
            return "Assets" + fullPath.Substring(Application.dataPath.Length);
        }
    }
}