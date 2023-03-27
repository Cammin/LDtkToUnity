using UnityEditor;
using UnityEditor.AssetImporters;

namespace ExperimentWithSpriteImporter
{
    /*[CustomEditor(typeof(MyAssetPostProcessor))]
    public class EditorForImporter : ScriptedImporterEditor
    {
        public override bool showImportedObject => false;
        protected override bool useAssetDrawPreview => true;

        /*public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ppu"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("filterMode"));

            serializedObject.ApplyModifiedProperties();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Sprite Editor", GUILayout.Width(85)))
            {
                EditorWindow window = EditorWindow.GetWindow(Type.GetType("UnityEditor.U2D.Sprites.SpriteEditorWindow,Unity.2D.Sprite.Editor"));
                window.Show();
            }
            GUILayout.EndHorizontal();
            
            ApplyRevertGUI();

            MyAssetPostProcessor importer = (MyAssetPostProcessor)target;
            EditorGUILayout.LabelField($"format: {importer.texture.format}");
            EditorGUILayout.LabelField($"graphicsFormat: {importer.texture.graphicsFormat}");
        }#1#
    }*/
}