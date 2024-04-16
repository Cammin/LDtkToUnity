using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkEntityDrawerComponent))]
    internal sealed class LDtkEntityDrawerComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This object draws the scene content. Configure what to draw in preferences", MessageType.None);
            EditorGUILayout.HelpBox("This will be removed in a future update", MessageType.Warning);
            if (GUILayout.Button("LDtk To Unity's Preferences"))
            {
                SettingsService.OpenUserPreferences(LDtkPrefsProvider.PROVIDER_PATH);
            }
            //DrawDefaultInspector();
        }
    }
}