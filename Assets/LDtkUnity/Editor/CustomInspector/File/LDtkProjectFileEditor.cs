using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkProjectFile))]
    public class LDtkProjectFileEditor : LDtkJsonFileEditor<LdtkJson>
    {
        protected override void DrawInspectorGUI(LdtkJson project)
        {
            EditorGUILayout.LabelField("Project");

            string version = $"Json Version: {project.JsonVersion}";
            
            
            
            EditorGUILayout.LabelField(version);
        }
    }
}