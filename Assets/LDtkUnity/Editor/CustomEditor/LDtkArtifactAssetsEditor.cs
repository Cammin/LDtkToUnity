using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkArtifactAssets))]
    public class LDtkArtifactAssetsEditor : UnityEditor.Editor
    {
        
        
        public override void OnInspectorGUI()
        {
            SerializedProperty spritesProp = serializedObject.FindProperty(LDtkArtifactAssets.PROP_SPRITE_LIST);
            SerializedProperty tilesProp = serializedObject.FindProperty(LDtkArtifactAssets.PROP_TILE_LIST);

            if (spritesProp == null || tilesProp == null)
            {
                return;
            }


            DrawSection(spritesProp, "Sprite");
            LDtkEditorGUIUtility.DrawDivider();
            DrawSection(tilesProp, "Tile");
        }

        private static void DrawSection(SerializedProperty tilesProp, string sprites)
        {
            EditorGUIUtility.SetIconSize(Vector2.one * 16);
            Texture image = LDtkIconUtility.GetUnityIcon(sprites);

            GUIContent tilesContent = new GUIContent()
            {
                text = $"{tilesProp.arraySize} {sprites}s",
                image = image
            };


            EditorGUILayout.LabelField(tilesContent);
            


            using (new LDtkIndentScope())
            {
                DrawElements(tilesProp, image);
            }
        }

        private static void DrawElements(SerializedProperty arrayProp, Texture image)
        {
            const int maxDrawn = 500;
            int drawAmount = Mathf.Min(arrayProp.arraySize, maxDrawn);
            
            for (int i = 0; i < drawAmount; i++)
            {
                SerializedProperty tileProp = arrayProp.GetArrayElementAtIndex(i);

                if (tileProp == null || tileProp.objectReferenceValue == null)
                {
                    Debug.LogError("TileCollection drawer error");
                    return;
                }

                GUIContent tileContent = new GUIContent()
                {
                    text = tileProp.objectReferenceValue.name,
                    image = image
                };

                EditorGUILayout.LabelField(tileContent);
            }

            if (arrayProp.arraySize > maxDrawn)
            {
                EditorGUILayout.LabelField("(Plus more)");
            }
        }
    }
}