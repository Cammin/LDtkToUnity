using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public class LDtkReferenceDrawerTileset : LDtkAssetReferenceDrawer<LDtkDefinitionTileset, LDtkTilesetAsset>
    {
        private bool _failedSpriteGet;
        private string _failedSpritePath;
        private readonly string _pathToSprite;
        


        public LDtkReferenceDrawerTileset(LDtkDefinitionTileset data, LDtkTilesetAsset asset, string initialPath) : base(data, asset)
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