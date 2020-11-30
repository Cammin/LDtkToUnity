using LDtkUnity.Editor.AssetManagement.AssetWindow;
using LDtkUnity.Runtime.UnityAssets.Settings;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkProject))]
    public class LDtkProjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Not editable directly, go to the Asset Manager Window");
            if (GUILayout.Button("Go to Asset Manager"))
            {
                LDtkAssetWindow.CustomInit((LDtkProject)target);
            }
            
            GUI.enabled = false;
            base.OnInspectorGUI();
            GUI.enabled = true;
        }
    }
}