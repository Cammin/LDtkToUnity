using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerTileset : LDtkAssetReferenceDrawer<TilesetDefinition>
    {
        public LDtkReferenceDrawerTileset(SerializedProperty obj, string key) : base(obj, key)
        {
            
        }

        
        protected override void DrawInternal(Rect controlRect, TilesetDefinition data)
        {
            DrawSelfSimple<Texture2D>(controlRect, data);

            if (HasError(data))
            {
                return;
            }
            
            
            //if (GUILayout.Button("Generate Sprites"))
            if (DrawRightFieldIconButton(controlRect, "Refresh", "Generate Sprites"))
            {
                Texture2D texture = Value.objectReferenceValue as Texture2D;
                bool success = LDtkMetaSpriteFactory.GenerateMetaSpritesFromTexture(texture, (int)data.TileGridSize);

                if (!success)
                {
                    ThrowError("Had trouble generating meta files for texture");
                }
            }
        }

        public override bool HasError(TilesetDefinition data)
        {
            if (base.HasError(data))
            {
                return true;
            }
            
            Texture2D texture = Value.objectReferenceValue as Texture2D;
            if (texture == null)
            {
                ThrowError("Null texture");
                return true;
            }
                
            if (!texture.isReadable)
            {
                ThrowError("Tileset texture does not have Read/Write Enabled");
                return true;
            }

            return false;
        }
    }
}