using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkMetaSpriteFactory
    {
        public static bool GenerateMetaSpritesFromTexture(Texture2D spriteSheet, int pixelsPerUnit)
        {
            string path = AssetDatabase.GetAssetPath(spriteSheet);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                Debug.LogError("importer null");
                return false;
            }
            
            //set as None, then Multiple in order to destroy the old metadatas
            importer.spriteImportMode = SpriteImportMode.None;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            
            List<SpriteMetaData> metaDatas = GenerateSpriteMetaDatas(spriteSheet, pixelsPerUnit);
            importer.spritesheet = metaDatas.ToArray();
            importer.isReadable = true;
            importer.spritePixelsPerUnit = pixelsPerUnit;
            
            importer.SaveAndReimport();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            
            Debug.Log($"Generated {metaDatas.Count} Sprites from \"{spriteSheet.name}\"", spriteSheet);
            EditorGUIUtility.PingObject(spriteSheet);
            
            return true;
        }

        private static List<SpriteMetaData> GenerateSpriteMetaDatas(Texture2D spriteSheet, int pixelsPerUnit)
        {
            List<Vector2Int> srcRects = new List<Vector2Int>();
            for (int x = 0; x < spriteSheet.width / pixelsPerUnit; x++)
            {
                for (int y = 0; y < spriteSheet.height / pixelsPerUnit; y++)
                {
                    srcRects.Add(new Vector2Int(x, y) * pixelsPerUnit);
                }
            }
            
            List<SpriteMetaData> metaDatas = new List<SpriteMetaData>();
            foreach (Vector2Int srcPos in srcRects)
            {
                Rect srcRect = new Rect(srcPos, Vector2.one * pixelsPerUnit);

                SpriteMetaData metaData = new SpriteMetaData
                {
                    rect = srcRect,
                    name = LDtkTilesetSpriteKeyFormat.GetKeyFormat(spriteSheet.name, srcRect.position)
                };

                metaDatas.Add(metaData);
            }

            return metaDatas;
        }

        public static Sprite[] GetMetaSpritesOfTexture(Texture2D spriteSheet)
        {
            if (spriteSheet == null)
            {
                Debug.LogError("Texture2D null");
                return null;
            }
            
            string spriteSheetPath = AssetDatabase.GetAssetPath(spriteSheet);
            return AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath).OfType<Sprite>().ToArray();
        }
    }
}