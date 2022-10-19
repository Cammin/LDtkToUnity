using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkArtifactAssets))]
    internal sealed class LDtkArtifactAssetsEditor : LDtkEditor
    {
        protected override Texture2D StaticPreview => (Texture2D)LDtkIconUtility.GetUnityIcon("Tilemap");

        private SerializedProperty _spritesProp;
        private SerializedProperty _tilesProp;
        private SerializedProperty _backgroundsProp;
        
        private string _searchString = "";
        
        private void OnEnable()
        {
            _spritesProp = serializedObject.FindProperty(LDtkArtifactAssets.PROPERTY_SPRITE_LIST);
            _tilesProp = serializedObject.FindProperty(LDtkArtifactAssets.PROPERTY_TILE_LIST);
            _backgroundsProp = serializedObject.FindProperty(LDtkArtifactAssets.PROPERTY_BACKGROUND_LIST);
        }

        public override void OnInspectorGUI()
        {
            if (_spritesProp == null || _tilesProp == null || _backgroundsProp == null)
            {
                LDtkDebug.LogError("Drawing error");
                return;
            }
            
            using (new LDtkGUIEnabledScope(true))
            {
                SearchBar();
                DrawSection(_spritesProp, "Sprite", "Sprite");
                LDtkEditorGUIUtility.DrawDivider();
                DrawSection(_backgroundsProp, "Image", "Background Sprite");
                LDtkEditorGUIUtility.DrawDivider();
                DrawSection(_tilesProp, "Tile","Art Tile");
            }
        }


        private void SearchBar()
        {
            GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            //GUILayout.FlexibleSpace();
            _searchString = GUILayout.TextField(_searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                // Remove focus if cleared
                _searchString = "";
                GUI.FocusControl(null);
            }
            GUILayout.EndHorizontal();
        }

        private void DrawSection(SerializedProperty tilesProp, string icon, string label)
        {
            EditorGUIUtility.SetIconSize(Vector2.one * 16);
            Texture image = LDtkIconUtility.GetUnityIcon(icon);

            string pluralizedText = $"{tilesProp.arraySize} {label}" + (tilesProp.arraySize != 1 ? "s" : "");
            GUIContent tilesContent = new GUIContent()
            {
                text = pluralizedText,
                image = image
            };
            
            EditorGUILayout.LabelField(tilesContent);
            
            using (new EditorGUI.IndentLevelScope())
            {
                DrawElements(tilesProp, image);
            }
        }

        private void DrawElements(SerializedProperty arrayProp, Texture image)
        {
            const int maxDrawn = 51;

            int i = 0;
            int drawn = 0;

            bool nullOrEmpty = string.IsNullOrEmpty(_searchString);
            string term = _searchString.ToLower();
            
            while (drawn < maxDrawn && i < arrayProp.arraySize)
            {
                SerializedProperty element = arrayProp.GetArrayElementAtIndex(i);
                if (element == null || element.objectReferenceValue == null)
                {
                    LDtkDebug.LogError("tileProp is null");
                    continue;
                }
                
                string elementName = element.objectReferenceValue.name.ToLower();
                if (nullOrEmpty || elementName.Contains(term))
                {
                    DrawItem(element, image);
                    drawn++;
                }
                
                i++;
            }

            if (drawn >= maxDrawn)
            {
                EditorGUILayout.LabelField("(Plus more)");
            }
        }

        private static void DrawItem(SerializedProperty element, Texture image)
        {
            if (element == null)
            {
                LDtkDebug.LogError("tileProp is null");
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