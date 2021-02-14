using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerTileset : LDtkAssetReferenceDrawer<TilesetDefinition>
    {
        private readonly int TargetPixelsPerUnit;
        
        LDtkTilesetAsset Asset => (LDtkTilesetAsset)(object)Value.serializedObject.targetObject;

        public LDtkReferenceDrawerTileset(SerializedObject obj, string key, int targetPixelsPerUnit) : base(obj, key)
        {
            TargetPixelsPerUnit = targetPixelsPerUnit;
        }

        
        protected override void DrawInternal(Rect controlRect, TilesetDefinition data)
        {
            DrawLeftIcon(controlRect, LDtkIconLoader.LoadTilesetIcon());
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadTilesetIcon(), data);

            if (!HasProblem)
            {
                if (!Asset.ReferencedAsset.isReadable)
                {
                    ThrowError(controlRect, "Tileset texture does not have Read/Write Enabled");
                }
                
                if (Asset.AssetExists && GUILayout.Button("Generate Sprites"))
                {
                    bool success = LDtkSpriteUtil.GenerateMetaSpritesFromTexture(Asset.ReferencedAsset, TargetPixelsPerUnit);

                    if (!success)
                    {
                        ThrowError(controlRect, "Had trouble generating meta files for texture");
                    }
                }
            }
        }
        
        
    }
}