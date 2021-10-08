using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LDtkIntGridTile))]
    public class LDtkIntGridTileEditor : UnityEditor.Editor
    {
        private SpritePhysicsPointsDrawer _shapeDrawer;

        protected override bool ShouldHideOpenButton() => true;

        private readonly GUIContent _colliderLabel = new GUIContent
        {
            text = "Collider Type",
            tooltip = "None > No collision. Renders a square if rendering IntGridValues is enabled\n\n" +
                      "Sprite > Use a sprite's physics shape(s) for collision. Renders the sprite if rendering IntGridValues is enabled\n\n" +
                      "Grid > Square collision. Renders a square if rendering IntGridValues is enabled"
        };
        private readonly GUIContent _spriteLabel = new GUIContent
        {
            text = "Custom Physics Sprite",
            tooltip = "The collision shape is based on the physics shape(s) of the sprite which is previewed here for convenience. Commonly useful for slopes, etc"
        };
        private readonly GUIContent _gameObjectLabel = new GUIContent
        {
            text = "Game Object Prefab",
            tooltip = "Optional.\n" +
                      "The GameObject spawned at this TileBase."
        };
        private readonly GUIContent _tagLabel = new GUIContent
        {
            text = "Tilemap Tag",
            tooltip = "Sets the tag of this tile's tilemap.\n" +
                      "If tiles have the same tag, layer and physics material, then they will be grouped in the same tilemap and can merge colliders if using a composite collider."
        };
        private readonly GUIContent _layerMaskLabel = new GUIContent
        {
            text = "Tilemap Layer",
            tooltip = "Sets the layer mask of this tile's tilemap.\n" +
                      "If tiles have the same tag, layer and physics material, then they will be grouped in the same tilemap and can merge colliders if using a composite collider."
        };
        private readonly GUIContent _physicsMaterialLabel = new GUIContent
        {
            text = "Physics Material",
            tooltip = "Sets the physics material of this tile's tilemap.\n" +
                      "If tiles have the same tag, layer and physics material, then they will be grouped in the same tilemap and can merge colliders if using a composite collider."
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
            
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.HelpBox("After finished editing, Save Project to reimport all LDtk projects that use this tile", MessageType.None);
                
                GUIContent saveButtonLabel = new GUIContent
                {
                    tooltip = "Save Project",
                    image = LDtkIconUtility.GetUnityIcon("SaveAs", "")
                };
                if (GUILayout.Button(saveButtonLabel, GUILayout.Width(30), GUILayout.ExpandHeight(true)))
                {
                    AssetDatabase.SaveAssets();
                }
            }

            LDtkEditorGUIUtility.DrawDivider();
            
            serializedObject.DrawField(LDtkIntGridTile.PROP_TAG, _tagLabel);
            serializedObject.DrawField(LDtkIntGridTile.PROP_LAYERMASK, _layerMaskLabel);
            serializedObject.DrawField(LDtkIntGridTile.PROP_PHYSICS_MATERIAL, _physicsMaterialLabel);
            
            LDtkEditorGUIUtility.DrawDivider();
            
            SerializedProperty colliderTypeProp = serializedObject.DrawField(LDtkIntGridTile.PROP_COLLIDER_TYPE, _colliderLabel);
            if (colliderTypeProp.enumValueIndex == (int)Tile.ColliderType.Sprite || serializedObject.isEditingMultipleObjects)
            {
                SerializedProperty physicsSpriteProp = serializedObject.DrawField(LDtkIntGridTile.PROP_CUSTOM_PHYSICS_SPRITE, _spriteLabel);
                if (physicsSpriteProp.objectReferenceValue != null && !serializedObject.isEditingMultipleObjects)
                {
                    DrawCollisionShape((Sprite)physicsSpriteProp.objectReferenceValue);
                }
            }
            
            LDtkEditorGUIUtility.DrawDivider();

            SerializedProperty gameObjectProp = serializedObject.DrawField(LDtkIntGridTile.PROP_GAME_OBJECT, _gameObjectLabel);
            
            LDtkSectionDrawer.DenyPotentialResursiveGameObjects(gameObjectProp);
            
            if (gameObjectProp.objectReferenceValue != null && !serializedObject.isEditingMultipleObjects)
            {
                DrawGameObjectPreview((GameObject)gameObjectProp.objectReferenceValue);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGameObjectPreview(GameObject go)
        {
            Rect frame = GetFrame(128);
            
            Texture2D tex = AssetPreview.GetAssetPreview(go);
            //if gameobject has no render of any kind, don't try drawing
            if (tex == null)
            {
                return;
            }
            //Debug.Log(tex.width);
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
    }
}