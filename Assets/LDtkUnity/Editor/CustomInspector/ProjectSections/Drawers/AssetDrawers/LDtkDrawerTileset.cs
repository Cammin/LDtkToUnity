using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerTileset : LDtkAssetDrawer<TilesetDefinition, Texture2D>
    {
        public LDtkDrawerTileset(TilesetDefinition def, SerializedProperty obj, string key) : base(def, obj, key)
        {
            
        }
        
        public override void Draw()
        {
        
            Rect controlRect = EditorGUILayout.GetControlRect();
            DrawField(controlRect);

            if (Value == null || Asset == null)
            {
                return;
            }

            int buttonLvl = 0;
            
            if (!Asset.isReadable)
            {
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
                //new LDtkTextureMetaSprites((int)data.TileGridSize).Modify(Asset);
            }
            buttonLvl++;
        }

        public override bool HasProblem()
        {
            if (base.HasProblem())
            {
                return true;
            }

            if (!Asset.isReadable)
            {
                CacheError("Tileset texture does not have Read/Write enabled which is required to generate tiles. Click to enable Read/Write.");
                return true;
            }
            
            //todo test pixels per unit

            return false;
        }
    }
}