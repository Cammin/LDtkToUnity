using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkIntGridTile))]
    public class LDtkIntGridTileEditor : UnityEditor.Editor
    {
        private SpritePhysicsPointsDrawer _shapeDrawer;

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
            
            SerializedProperty colliderTypeProp = DrawProp(LDtkIntGridTile.PROP_COLLIDER_TYPE);
            if (colliderTypeProp.enumValueIndex == (int)Tile.ColliderType.Sprite)
            {
                SerializedProperty physicsSpriteProp = DrawProp(LDtkIntGridTile.PROP_CUSTOM_PHYSICS_SPRITE);
                if (physicsSpriteProp.objectReferenceValue != null)
                {
                    //DrawCollisionShape((Sprite)physicsSpriteProp.objectReferenceValue);
                }
            }
            
            LDtkEditorGUIUtility.DrawDivider();
            
            SerializedProperty useColorProp = DrawProp(LDtkIntGridTile.PROP_USE_LDTK_DEFINITION_COLOR);
            if (!useColorProp.boolValue)
            {
                DrawProp(LDtkIntGridTile.PROP_CUSTOM_COLOR);
            }
            
            LDtkEditorGUIUtility.DrawDivider();

            SerializedProperty gameObjectProp = DrawProp(LDtkIntGridTile.PROP_GAME_OBJECT);
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
            GUI.DrawTexture(GetFrame(tex.width), tex);
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
            Rect area = GetFrame(100);
            _shapeDrawer.Draw(sprite, area);
        }



        private SerializedProperty DrawProp(string propName)
        {
            SerializedProperty property = serializedObject.FindProperty(propName);
            EditorGUILayout.PropertyField(property);
            return property;
        }
    }
}