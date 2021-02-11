using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkSpriteUtil
    {
        public static bool GenerateMetaSpritesFromTexture(Texture2D spriteSheet, int pixelsPerUnit)
        {
            List<Vector2Int> srcRects = new List<Vector2Int>();
            for (int x = 0; x < spriteSheet.width / pixelsPerUnit; x++)
            {
                for (int y = 0; y < spriteSheet.height / pixelsPerUnit; y++)
                {
                    srcRects.Add(new Vector2Int(x, y) * pixelsPerUnit);
                }
            }
            
            string path = AssetDatabase.GetAssetPath(spriteSheet);
            
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer == null)
            {
                Debug.LogError("importer null");
                return false;
            }
            
            importer.isReadable = true;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            
            List<SpriteMetaData> metaDatas = new List<SpriteMetaData>();

            importer.spritesheet = null;
            foreach (Vector2Int srcPos in srcRects)
            {
                Rect srcRect = new Rect(srcPos, Vector2.one * pixelsPerUnit);
                SpriteMetaData metaData = new SpriteMetaData();
                

                metaData.rect = srcRect;
                metaData.name = $"{srcRect.ToString("F0")}";

                metaDatas.Add(metaData);
            }

            importer.spritesheet = metaDatas.ToArray();

            Debug.Log(importer.spritesheet);
            
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

            return true;
        }

        public static Sprite[] GetSpritesOfTexture(Texture2D spriteSheet)
        {
            string spriteSheetPath = AssetDatabase.GetAssetPath(spriteSheet);
            return AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath).OfType<Sprite>().ToArray();
        }
    }
}