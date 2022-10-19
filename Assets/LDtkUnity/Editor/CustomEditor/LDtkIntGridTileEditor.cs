using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LDtkIntGridTile))]
    internal sealed class LDtkIntGridTileEditor : LDtkEditor
    {
        private static readonly GUIContent ColliderLabel = new GUIContent
        {
            text = "Collider Type",
            tooltip = "None > No collision. Renders a square if rendering IntGridValues is enabled\n\n" +
                      "Sprite > Use a sprite's physics shape(s) for collision. Renders the sprite if rendering IntGridValues is enabled\n\n" +
                      "Grid > Square collision. Renders a square if rendering IntGridValues is enabled"
        };
        private static readonly GUIContent SpriteLabel = new GUIContent
        {
            text = "Custom Physics Sprite",
            tooltip = "The collision shape is based on the physics shape(s) of the sprite which is previewed here for convenience. Commonly useful for slopes, etc"
        };
        private static readonly GUIContent GameObjectLabel = new GUIContent
        {
            text = "Game Object Prefab",
            tooltip = "Optional.\n" +
                      "The GameObject spawned at this TileBase."
        };
        private static readonly GUIContent TagLabel = new GUIContent
        {
            text = "Tilemap Tag",
            tooltip = "Sets the tag of this tile's tilemap.\n" +
                      "If tiles have the same tag, layer and physics material, then they will be grouped in the same tilemap and can merge colliders if using a composite collider."
        };
        private static readonly GUIContent LayerMaskLabel = new GUIContent
        {
            text = "Tilemap Layer",
            tooltip = "Sets the layer mask of this tile's tilemap.\n" +
                      "If tiles have the same tag, layer and physics material, then they will be grouped in the same tilemap and can merge colliders if using a composite collider."
        };
        private static readonly GUIContent PhysicsMaterialLabel = new GUIContent
        {
            text = "Physics Material",
            tooltip = "Sets the physics material of this tile's tilemap.\n" +
                      "If tiles have the same tag, layer and physics material, then they will be grouped in the same tilemap and can merge colliders if using a composite collider."
        };
        
        protected override Texture2D StaticPreview => LDtkIconUtility.LoadIntGridIcon();
        protected override bool ShouldHideOpenButton() => true;
        
        private SpritePhysicsPointsDrawer _shapeDrawer;
        private SerializedProperty _propTag;
        private SerializedProperty _propLayermask;
        private SerializedProperty _propPhysicsMaterial;
        private SerializedProperty _propColliderType;
        private SerializedProperty _propCustomPhysicsSprite;
        private SerializedProperty _propGameObject;

        private void OnEnable()
        {
            _shapeDrawer = new SpritePhysicsPointsDrawer();

            _propTag = serializedObject.FindProperty(LDtkIntGridTile.PROPERTY_TAG);
            _propLayermask = serializedObject.FindProperty(LDtkIntGridTile.PROPERTY_LAYERMASK);
            _propPhysicsMaterial = serializedObject.FindProperty(LDtkIntGridTile.PROPERTY_PHYSICS_MATERIAL);
            _propColliderType = serializedObject.FindProperty(LDtkIntGridTile.PROPERTY_COLLIDER_TYPE);
            _propCustomPhysicsSprite = serializedObject.FindProperty(LDtkIntGridTile.PROPERTY_CUSTOM_PHYSICS_SPRITE);
            _propGameObject = serializedObject.FindProperty(LDtkIntGridTile.PROPERTY_GAME_OBJECT);
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
            
            EditorGUILayout.PropertyField(_propTag, TagLabel);
            EditorGUILayout.PropertyField(_propLayermask, LayerMaskLabel);
            EditorGUILayout.PropertyField(_propPhysicsMaterial, PhysicsMaterialLabel);
            
            LDtkEditorGUIUtility.DrawDivider();
            
            EditorGUILayout.PropertyField(_propColliderType, ColliderLabel);
            if (_propColliderType.enumValueIndex == (int)Tile.ColliderType.Sprite || serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.PropertyField(_propCustomPhysicsSprite, SpriteLabel);
                if (_propCustomPhysicsSprite.objectReferenceValue != null && !serializedObject.isEditingMultipleObjects)
                {
                    DrawCollisionShape((Sprite)_propCustomPhysicsSprite.objectReferenceValue);
                }
            }
            
            LDtkEditorGUIUtility.DrawDivider();

            EditorGUILayout.PropertyField(_propGameObject, GameObjectLabel);
            
            LDtkEditorGUIUtility.DenyPotentialResursiveGameObjects(_propGameObject);
            
            if (_propGameObject.objectReferenceValue != null && !serializedObject.isEditingMultipleObjects)
            {
                DrawGameObjectPreview((GameObject)_propGameObject.objectReferenceValue);
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