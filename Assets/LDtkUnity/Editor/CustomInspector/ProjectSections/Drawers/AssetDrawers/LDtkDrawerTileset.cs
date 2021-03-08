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
                
                if (DrawButtonToLeftOfField(controlRect, "console.erroricon.sml", null, buttonLvl))
                {
                    new LDtkTextureIsReadable(true).Modify(Asset);
                }
                buttonLvl++;
                
            }
            
            if (HasProblem())
            {
                return;
            }
            
            if (DrawButtonToLeftOfField(controlRect, "Sprite Icon", "Auto Slice Sprites", buttonLvl))
            {
                new LDtkTextureMetaSprites((int)_data.TileGridSize).Modify(Asset);
            }
            buttonLvl++;
            
            if (DrawButtonToLeftOfField(controlRect, "Tile Icon", "Generate Tiles", buttonLvl))
            {
                Debug.Log("Generate Tiles");

                Tile[] tiles = LDtkTileFactory.GenerateTilesForTextureMetas(Asset);

                Debug.Log(Value.serializedObject.targetObject.name);

                SerializedProperty tilesArrayProp = Root.serializedObject.FindProperty(LDtkProject.TILES);

                LDtkAsset[] metaTiles = (LDtkAsset[])(object)tilesArrayProp.objectReferenceValue;

                SaveTheTiles(tiles);


                foreach (LDtkAsset metaTile in metaTiles)
                {
                    //if our array property does not have one with their name, 
                    //Set the 
                    
                    
                    //delete that array index, and replace it with a new one at the same array position
                }
                
                foreach (Tile tile in tiles)
                {
                    //do more progress here
                }
            }
            buttonLvl++;
        }

        private static void SaveTheTiles(Tile[] tiles)
        {
            //save them in assets
            foreach (Tile tile in tiles)
            {
                LDtkAssetUtil.SaveAsset(tile);
            }

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
            
            //todo test pixels per unit

            return false;
        }
    }
}