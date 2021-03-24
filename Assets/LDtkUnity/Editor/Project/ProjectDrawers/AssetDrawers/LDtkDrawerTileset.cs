using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerTileset : LDtkAssetDrawer<TilesetDefinition, Texture2D>
    {
        public LDtkDrawerTileset(TilesetDefinition def, SerializedProperty obj, string key) : base(def, obj, key)
        {
        }

        private bool HasReadableError => !Asset.isReadable;
        protected override string AssetUnassignedText => "Texture not assigned";

        public override void Draw()
        {
        
            Rect controlRect = EditorGUILayout.GetControlRect();
            DrawField(controlRect);

            if (Value == null || Asset == null)
            {
                return;
            }

            int buttonLvl = 0;
            
            if (HasReadableError)
            {
                //Rect errorRect = new Rect(controlRect)
                GUIContent errorContent = EditorGUIUtility.IconContent("console.erroricon.sml");
                if (DrawButtonToLeftOfField(controlRect, errorContent, buttonLvl))
                {
                    new LDtkTextureIsReadable(true).Modify(Asset);
                }
                buttonLvl++;
                
            }
            
            if (HasProblem())
            {
                return;
            }

            DrawAutoSliceButton(controlRect, buttonLvl);
            buttonLvl++;
            GenerateTilesButton(controlRect, buttonLvl);
        }
        
        private void DrawAutoSliceButton(Rect controlRect, int buttonLvl)
        {
            GUIContent autoSliceContent = new GUIContent()
            {
                tooltip = "Auto Slice Sprites",
                image = LDtkIconLoader.GetUnityIcon("Sprite")
            };
            if (DrawButtonToLeftOfField(controlRect, autoSliceContent, buttonLvl))
            {
                new LDtkTextureMetaSprites((int) _data.TileGridSize).Modify(Asset);
            }
        }
        
        private void GenerateTilesButton(Rect controlRect, int buttonLvl)
        {
            
            
            GUIContent buttonContent = new GUIContent()
            {
                tooltip = "Generate Tile Collection",
                image = LDtkIconLoader.GetUnityIcon("Tilemap") 
                //tooltip = "For this texture, generate a tile collection and save it as a collection asset.",
                //image = EditorGUIUtility.IconContent("Tile Icon").image
            };
            
            if (!DrawButtonToLeftOfField(controlRect, buttonContent, buttonLvl))
            {
                return;
            }

            Sprite[] sprites = LDtkAssetUtil.GetMetaSpritesOfTexture(Asset);
            string assetName = Asset.name + "_Tiles";
            LDtkTileCollection tileCollection = LDtkTileCollectionFactory.CreateAndSaveTileCollection(sprites, assetName, ContructTile);

            if (tileCollection != null)
            {
                LDtkEditorUtil.Dirty(tileCollection);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorGUIUtility.PingObject(tileCollection);
            }
        }
        
        private Tile ContructTile(Sprite sprite)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            
            tile.colliderType = Tile.ColliderType.None;
            tile.sprite = sprite;
            tile.name = LDtkTilesetSpriteKeyFormat.GetKeyFormat(sprite.texture, sprite.rect.position);
            return tile;
        }


        public override bool HasProblem()
        {
            if (base.HasProblem())
            {
                return true;
            }

            if (HasReadableError)
            {
                CacheError("Tileset texture does not have Read/Write enabled which is required to generate tiles. Click to enable Read/Write.");
                return true;
            }
            
            //todo test pixels per unit if it is right

            return false;
        }
    }
}