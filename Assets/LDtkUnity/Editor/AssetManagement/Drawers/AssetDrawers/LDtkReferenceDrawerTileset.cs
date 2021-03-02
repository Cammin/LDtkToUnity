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
            //DrawLeftIcon(controlRect, LDtkIconLoader.LoadTilesetIcon());
            DrawSelfSimple<Texture2D>(controlRect, LDtkIconLoader.LoadTilesetIcon(), data);
            //DrawIconAndLabel(controlRect, LDtkIconLoader.LoadTilesetIcon(), data);

            if (!HasProblem)
            {
                Texture2D texture = Value.objectReferenceValue as Texture2D;
                if (texture == null)
                {
                    Debug.LogError("Null texture");                    
                }
                
                if (!texture.isReadable)
                {
                    ThrowError(controlRect, "Tileset texture does not have Read/Write Enabled");
                }

                
                
                //if (GUILayout.Button("Generate Sprites"))
                if (DrawRightFieldIconButton(controlRect, "Refresh", "Generate Sprites"))
                {
                    bool success = LDtkMetaSpriteFactory.GenerateMetaSpritesFromTexture(texture, (int)data.TileGridSize);

                    if (!success)
                    {
                        ThrowError(controlRect, "Had trouble generating meta files for texture");
                    }
                }
            }
        }
        
        
    }
}