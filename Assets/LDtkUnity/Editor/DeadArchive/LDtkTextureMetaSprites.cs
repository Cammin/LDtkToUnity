using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /*public class LDtkTextureMetaSprites : LDtkTextureImportModifier
    {
        private readonly int _gridSize;
        private readonly int _spacing;
        private readonly int _padding;

        public LDtkTextureMetaSprites(int gridSize, int spacing, int padding)
        {
            _gridSize = gridSize;
            _spacing = spacing;
            _padding = padding;
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
            List<Vector2Int> srcPositions = new List<Vector2Int>();
            int gap = _gridSize + _spacing;
            
            for (int x = _padding; x <= Texture.width - gap; x += gap)
            {
                for (int y = _padding; y <= Texture.height - gap; y += gap)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    pos = LDtkToolOriginCoordConverter.ImageSliceCoord(pos, Texture.height, _gridSize);
                    srcPositions.Add(pos);
                }
            }
            
            List<SpriteMetaData> metaDatas = new List<SpriteMetaData>();
            foreach (Vector2Int srcPos in srcPositions)
            {
                Rect srcRect = new Rect(srcPos, Vector2.one * _gridSize);

                SpriteMetaData metaData = new SpriteMetaData
                {
                    rect = srcRect,
                    name = LDtkKeyFormatUtil.TilesetKeyFormat(Texture, srcRect.position)
                };

                metaDatas.Add(metaData);
            }

            return metaDatas;
        }
    }*/
}