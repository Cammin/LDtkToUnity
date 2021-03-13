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
                GUIContent errorContent = new GUIContent(EditorGUIUtility.IconContent("console.erroricon.sml").image);
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
                image = EditorGUIUtility.IconContent("Sprite Icon").image
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
                //tooltip = "For this texture, generate a tile collection and save it as a collection asset.",
                image = EditorGUIUtility.IconContent("Tile Icon").image
            };
            
            if (!DrawButtonToLeftOfField(controlRect, buttonContent, buttonLvl))
            {
                return;
            }

            LDtkTileCollectionFactory.CreateAndSaveTileCollection(Asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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