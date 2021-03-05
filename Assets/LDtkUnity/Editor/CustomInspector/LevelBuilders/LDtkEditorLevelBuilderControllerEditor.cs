using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkEditorLevelBuilderController))]
    public class LDtkEditorLevelBuilderControllerEditor : LDtkLevelBuilderControllerEditor
    {
        private LDtkEditorLevelBuilderController Builder => (LDtkEditorLevelBuilderController) target;

        public override void OnInspectorGUI()
        {
            if (!DrawMainContent())
            {
                return;
            }

            if (!Application.isPlaying)
            {
                EditorGUILayout.Space();
                DrawBuildButton();
            }
        }

        private void DrawLinkedLevel()
        {
            SerializedProperty property = serializedObject.FindProperty(LDtkEditorLevelBuilderController.PREV_BUILT);
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(property);
            GUI.enabled = true;
        }

        //todo implement a feature where we can automatically regenerate levels if a change was made from last.
        private void DrawAutoUpdateToggle()
        {
            
        }

        
        
        private void DrawBuildButton()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            controlRect.width = 150;
            
            SerializedProperty property = serializedObject.FindProperty(LDtkEditorLevelBuilderController.PREV_BUILT);

            string buttonMessage = Builder.PrevExists ? "Regenerate Project" : "Build Project";
            
            if (!GUI.Button(controlRect, buttonMessage))
            {
                return;
            }

            Builder.BuildLevels();
            
            EditorUtility.SetDirty(Builder);
        }
    }
}