namespace LDtkUnity.Editor
{
    /*[CustomEditor(typeof(LDtkEditorLevelBuilderController))]
    public class LDtkEditorLevelBuilderControllerEditor : LDtkLevelBuilderControllerEditor
    {
        private LDtkEditorLevelBuilderController Builder => (LDtkEditorLevelBuilderController) target;

        public override void OnInspectorGUI()
        {
            if (!DrawJsonField())
            {
                return;
            }
            
            DrawLinkedLevel();
       
            if (!Application.isPlaying)
            {
                LDtkDrawerUtil.DrawDivider();
                DrawLogBuildDetails();
                DrawOptionalCustomPos();
                DrawBuildButton();
            }
            
            DrawLevels();
            
            serializedObject.ApplyModifiedProperties();

            DrawEditorOnlyNotification();
        }

        private void DrawEditorOnlyNotification()
        {
            if (!Builder.gameObject.CompareTag("EditorOnly"))
            {
                EditorGUILayout.HelpBox("This GameObject is not tagged as \"EditorOnly\", change it to optimize scene size in builds.", MessageType.Warning);
            }
        }

        private void DrawLinkedLevel()
        {
            SerializedProperty property = serializedObject.FindProperty(LDtkEditorLevelBuilderController.PREV_BUILT);

            GUIContent content = new GUIContent("Project Root");
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(property, content);
            GUI.enabled = true;
        }

        private void DrawLogBuildDetails()
        {
            SerializedProperty boolProp = serializedObject.FindProperty(LDtkLevelBuilderController.LOG_BUILD_TIMES);
            EditorGUILayout.PropertyField(boolProp);
        }

        private void DrawOptionalCustomPos()
        {
            SerializedProperty boolProp = serializedObject.FindProperty(LDtkLevelBuilderController.USE_CUSTOM_SPAWN_POSITION);
            GUIContent boolContent = new GUIContent()
            {
                text = boolProp.displayName,
                tooltip = "Use this if spawning all levels at a custom position is desired. " +
                          "(Mostly useful if only one level is built, and the level in LDtk is very far away from origin, which would potentially fix some position complications)"
            };
            EditorGUILayout.PropertyField(boolProp, boolContent);

            if (!boolProp.boolValue)
            {
                return;
            }
            
            SerializedProperty vectorProp = serializedObject.FindProperty(LDtkLevelBuilderController.CUSTOM_SPAWN_POSITION);
            GUIContent vectorContent = new GUIContent(boolContent)
            {
                text = vectorProp.displayName
            };
            EditorGUILayout.PropertyField(vectorProp, vectorContent);

            
        }
        
        private void DrawBuildButton()
        {
            Rect controlRect = EditorGUILayout.GetControlRect(false, 30);
            controlRect.width = 160;

            string buttonMessage = Builder.PrevExists ? "Regenerate Project" : "Build Project";
            
            if (!GUI.Button(controlRect, buttonMessage))
            {
                return;
            }

            GameObject root = Builder.BuildLevels();
            
            EditorGUIUtility.PingObject(root);

            if (root != null)
            {
                Undo.RegisterCreatedObjectUndo(root, "Build Level");
                //EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                //EditorUtility.SetDirty(property.objectReferenceValue);
            }
        }
    }*/
}