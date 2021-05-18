using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkIntGridTile))]
    public class LDtkIntGridTileEditor : UnityEditor.Editor
    {
        private SpritePhysicsPointsDrawer _shapeDrawer;

        protected override bool ShouldHideOpenButton() => true;

        private readonly GUIContent _colliderLabel = new GUIContent
        {
            text = "Collider Type",
            tooltip = "None > No collision\n" +
                      "Sprite > Use a sprite's physics shape(s) for collision\n" +
                      "Grid > Full, square collision"
        };
        private readonly GUIContent _spriteLabel = new GUIContent
        {
            text = "Custom Physics Sprite",
            tooltip = "The collision shape is based on the physics shape(s) of the sprite which is previewed here for convenience. Commonly useful for slopes, etc"
        };
        private readonly GUIContent _gameObjectLabel = new GUIContent
        {
            text = "GameObject Prefab",
            tooltip = "Define a namespace for the enum script if desired."
        };
        
        private void OnEnable()
        {
            _shapeDrawer = new SpritePhysicsPointsDrawer();
        }
        private void OnDisable()
        {
            _shapeDrawer.Dispose();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.HelpBox("After finished editing, Use 'File > Save Project' to reimport all LDtk projects that use this tile", MessageType.None);
            
            LDtkEditorGUIUtility.DrawDivider();
            
            SerializedProperty colliderTypeProp = DrawProp(LDtkIntGridTile.PROP_COLLIDER_TYPE, _colliderLabel);
            if (colliderTypeProp.enumValueIndex == (int)Tile.ColliderType.Sprite)
            {
                SerializedProperty physicsSpriteProp = DrawProp(LDtkIntGridTile.PROP_CUSTOM_PHYSICS_SPRITE, _spriteLabel);
                if (physicsSpriteProp.objectReferenceValue != null)
                {
                    DrawCollisionShape((Sprite)physicsSpriteProp.objectReferenceValue);
                }
            }

            LDtkEditorGUIUtility.DrawDivider();

            SerializedProperty gameObjectProp = DrawProp(LDtkIntGridTile.PROP_GAME_OBJECT, _gameObjectLabel);
            if (gameObjectProp.objectReferenceValue != null)
            {
                DrawGameObjectPreview((GameObject)gameObjectProp.objectReferenceValue);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGameObjectPreview(GameObject go)
        {
            Texture2D tex = AssetPreview.GetAssetPreview(go);
            //if gameobject has no render of any kind, don't try drawing
            if (tex == null)
            {
                return;
            }

            //Debug.Log(tex.width);
            Rect frame = GetFrame(tex.width);
            GUI.DrawTexture(frame, tex);
        }

        private Rect GetFrame(int size)
        {
            Rect fieldArea = EditorGUILayout.GetControlRect(false, size);

            fieldArea.width = size;
            fieldArea.x += LDtkEditorGUIUtility.LabelWidth(fieldArea.width) + 2;
            return fieldArea;
        }

        private void DrawCollisionShape(Sprite sprite)
        {
            Rect area = GetFrame(128);
            //EditorGUI.DrawRect(area, Color.cyan);
            _shapeDrawer.Draw(sprite, area);
        }



        private SerializedProperty DrawProp(string propName, GUIContent content)
        {
            SerializedProperty property = serializedObject.FindProperty(propName);
            EditorGUILayout.PropertyField(property, content);
            return property;
        }
    }
}