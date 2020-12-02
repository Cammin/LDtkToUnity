using System.Runtime.InteropServices;
using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public class LDtkReferenceDrawerTileset : LDtkAssetReferenceDrawer<LDtkDefinitionTileset>
    {
        private bool _failedSpriteGet;
        private string _failedSpritePath;
        private readonly string _pathToSprite;
        
        LDtkTilesetAsset Asset => (LDtkTilesetAsset)Property.objectReferenceValue;

        public LDtkReferenceDrawerTileset(SerializedProperty asset, string initialPath) : base(asset)
        {
            _pathToSprite = initialPath;
        }

        
        protected override void DrawInternal(Rect controlRect, LDtkDefinitionTileset data)
        {
            DrawLeftIcon(controlRect, LDtkIconLoader.LoadTilesetIcon());
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadTilesetIcon(), data);
            
            if (DrawRightFieldIconButton(controlRect, "Refresh"))
            {
                RefreshSpritePathAssignment(data);
            }

            if (Asset != null && _failedSpriteGet)
            {
                GUIStyle miniLabel = EditorStyles.miniLabel;
                miniLabel.normal.textColor = Color.red;

                
                
                //SerializedObject serializedAssetObject = new SerializedObject(Property.objectReferenceValue);

                
                //SerializedProperty spriteProp = serializedAssetObject.FindProperty("_asset");

                if (!Asset.AssetExists)
                {
                    EditorGUILayout.LabelField($"Tileset could not be found in path {_pathToSprite}", miniLabel);
                }
                else
                {
                    if (!Asset.ReferencedAsset.texture.isReadable)
                    {
                        EditorGUILayout.LabelField($"Texture is not set to Read/Write enabled", miniLabel);
                    }
                }

                
            }
        }

        public void RefreshSpritePathAssignment(LDtkDefinitionTileset data)
        {
            Sprite tileset = AssetDatabase.LoadAssetAtPath<Sprite>(_pathToSprite);

            _failedSpriteGet = tileset == null;

            if (Asset)
            {
                Asset.ReferencedAsset = tileset;
            }
        }


    }
}