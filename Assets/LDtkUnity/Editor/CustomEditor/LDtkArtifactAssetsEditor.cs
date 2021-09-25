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
            SerializedProperty backgroundsProp = serializedObject.FindProperty(LDtkArtifactAssets.PROP_BACKGROUND_LIST);

            if (spritesProp == null || tilesProp == null || backgroundsProp == null)
            {
                Debug.LogError("Drawing error");
                return;
            }

            DrawSection(spritesProp, "Sprite", "IntGrid Sprite");
            LDtkEditorGUIUtility.DrawDivider();
            DrawSection(backgroundsProp, "Image", "Background Sprite");
            LDtkEditorGUIUtility.DrawDivider();
            DrawSection(tilesProp, "Tile","Art Tile");
        }

        private static void DrawSection(SerializedProperty tilesProp, string icon, string label)
        {
            EditorGUIUtility.SetIconSize(Vector2.one * 16);
            Texture image = LDtkIconUtility.GetUnityIcon(icon);

            string pluralizedText = $"{tilesProp.arraySize} {label}" + (tilesProp.arraySize > 1 ? "s" : "");
            GUIContent tilesContent = new GUIContent()
            {
                text = pluralizedText,
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
                DrawItem(arrayProp, image, i);
            }

            if (arrayProp.arraySize > maxDrawn)
            {
                EditorGUILayout.LabelField("(Plus more)");
            }
        }

        private static void DrawItem(SerializedProperty arrayProp, Texture image, int i)
        {
            SerializedProperty element = arrayProp.GetArrayElementAtIndex(i);

            if (element == null)
            {
                Debug.LogError("tileProp is null");
                return;
            }

            GUIContent tileContent = new GUIContent()
            {
                text = element.displayName,
                image = image
            };

            EditorGUILayout.PropertyField(element, tileContent);
        }
    }
}