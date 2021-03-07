using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkTextureMetaSprites : LDtkTextureImportModifier
    {
        private readonly int _pixelsPerUnit;
        
        public LDtkTextureMetaSprites(int pixelsPerUnit)
        {
            _pixelsPerUnit = pixelsPerUnit;
        }
        
        protected override void AlterTexture(TextureImporter importer)
        {
            //set as None, then Multiple in order to destroy the old metadatas
            importer.spriteImportMode = SpriteImportMode.None;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            
            List<SpriteMetaData> metaDatas = GenerateSpriteMetaDatas();
            importer.spritesheet = metaDatas.ToArray();
            
            Debug.Log($"Generated {metaDatas.Count} Sprites from \"{Texture.name}\"", Texture);
            EditorGUIUtility.PingObject(Texture);
        }
        
        private List<SpriteMetaData> GenerateSpriteMetaDatas()
        {
            List<Vector2Int> srcRects = new List<Vector2Int>();
            for (int x = 0; x < Texture.width / _pixelsPerUnit; x++)
            {
                for (int y = 0; y < Texture.height / _pixelsPerUnit; y++)
                {
                    srcRects.Add(new Vector2Int(x, y) * _pixelsPerUnit);
                }
            }
            
            List<SpriteMetaData> metaDatas = new List<SpriteMetaData>();
            foreach (Vector2Int srcPos in srcRects)
            {
                Rect srcRect = new Rect(srcPos, Vector2.one * _pixelsPerUnit);

                SpriteMetaData metaData = new SpriteMetaData
                {
                    rect = srcRect,
                    name = LDtkTilesetSpriteKeyFormat.GetKeyFormat(Texture.name, srcRect.position)
                };

                metaDatas.Add(metaData);
            }

            return metaDatas;
        }
    }
}