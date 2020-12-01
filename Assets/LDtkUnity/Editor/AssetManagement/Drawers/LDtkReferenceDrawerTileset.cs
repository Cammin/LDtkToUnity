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
            
            return;

            GUI.enabled = false;
            Asset = (LDtkTilesetAsset) EditorGUI.ObjectField(controlRect, Asset, typeof(LDtkTilesetAsset), false);
            GUI.enabled = true;

            if (Asset != null && _failedSpriteGet)
            {
                GUIStyle miniLabel = EditorStyles.miniLabel;
                miniLabel.normal.textColor = Color.red;
                EditorGUILayout.LabelField($"Tileset could not be found in path {_pathToSprite}", miniLabel);
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