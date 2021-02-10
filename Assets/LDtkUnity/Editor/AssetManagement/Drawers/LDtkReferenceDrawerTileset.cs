using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerTileset : LDtkAssetReferenceDrawer<TilesetDefinition>
    {
        private readonly LDtkProjectFile _projectFile;
        public Rect[] instances;
        
        LDtkTilesetAsset Asset => (LDtkTilesetAsset)Property.objectReferenceValue;

        public LDtkReferenceDrawerTileset(SerializedProperty asset, LDtkProjectFile projectProjectPath) : base(asset)
        {
            _projectFile = projectProjectPath;
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
                    bool success = LDtkSpriteUtil.GenerateMetaSpritesFromTexture(Asset.ReferencedAsset, instances);

                    if (!success)
                    {
                        ThrowError(controlRect, "Had trouble generating meta files for texture");
                    }
                }
            }
        }
        
        
    }
}