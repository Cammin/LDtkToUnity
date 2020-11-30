using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow.Drawers
{
    public class LDtkReferenceDrawerTileset : LDtkAssetReferenceDrawer<LDtkDefinitionTileset, LDtkTilesetAsset>
    {

        
        protected override Texture2D FieldIcon => LDtkIconLoader.LoadTilesetIcon();

        private readonly string _pathToSprite;

        public LDtkReferenceDrawerTileset(LDtkDefinitionTileset data, LDtkTilesetAsset asset, string initialPath) : base(data, asset)
        {
            _pathToSprite = initialPath;
        }
        
        protected override void DrawField(Rect fieldRect, LDtkDefinitionTileset data)
        {
            const float buttonWidth = 55;
        
            Rect buttonRect = new Rect(fieldRect)
            {
                width = buttonWidth
            };
            fieldRect.xMin += buttonWidth;

            if (GUI.Button(buttonRect, "Refresh"))
            {
                Refresh(data);
            }
            

            //GUI.enabled = false;
            Asset = (LDtkTilesetAsset) EditorGUI.ObjectField(fieldRect, Asset, typeof(LDtkTilesetAsset), false);
            //GUI.enabled = true;

            if (Asset != null && _failedSpriteGet)
            {
                GUIStyle miniLabel = EditorStyles.miniLabel;
                miniLabel.normal.textColor = Color.red;
                EditorGUILayout.LabelField($"Tileset could not be found in path {_pathToSprite}", miniLabel);
            }
        }

        private bool _failedSpriteGet;
        private string _failedSpritePath;

        public void Refresh(LDtkDefinitionTileset data)
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