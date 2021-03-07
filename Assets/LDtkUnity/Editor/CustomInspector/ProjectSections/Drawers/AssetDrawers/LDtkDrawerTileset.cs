using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerTileset : LDtkAssetDrawer<TilesetDefinition, Texture2D>
    {
        public LDtkDrawerTileset(SerializedProperty obj, string key) : base(obj, key)
        {
            
        }

        
        protected override void DrawInternal(Rect controlRect, TilesetDefinition data)
        {
            DrawField(controlRect, data);

            if (Value == null || Asset == null)
            {
                return;
            }
            
            if (!Asset.isReadable && DrawButtonToLeftOfField(controlRect, "console.erroricon.sml", null))
            {
                new LDtkTextureIsReadable(true).Modify(Asset);
            }
            
            if (HasError(data))
            {
                return;
            }
            
            if (DrawButtonToLeftOfField(controlRect, "Refresh", "Auto-slice sprites"))
            {
                new LDtkTextureMetaSprites((int)data.TileGridSize).Modify(Asset);
            }
        }

        public override bool HasError(TilesetDefinition data)
        {
            if (base.HasError(data))
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