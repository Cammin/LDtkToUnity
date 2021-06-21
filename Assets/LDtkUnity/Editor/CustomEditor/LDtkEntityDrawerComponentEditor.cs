using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkEntityDrawerComponent))]
    public class LDtkEntityDrawerComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This object draws the scene content. Configure what to draw in preferences", MessageType.None);
            if (GUILayout.Button("LDtk To Unity's Preferences"))
            {
                SettingsService.OpenUserPreferences(LDtkPrefsProvider.PREFS_PATH);
            }
            //DrawDefaultInspector();
        }
    }
}