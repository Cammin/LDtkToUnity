using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkSpriteUtil
    {
        public static bool GenerateMetaSpritesFromTexture(Texture2D spriteSheet, Rect[] srcRects)
        {
            string path = AssetDatabase.GetAssetPath(spriteSheet);
            
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer == null)
            {
                Debug.LogError("importer null");
                return false;
            }
            
            importer.isReadable = true;
            
            List<SpriteMetaData> metaDatas = new List<SpriteMetaData>();

            for (int i = 0; i < srcRects.Length; i++)
            {
                Rect srcRect = srcRects[i];
                SpriteMetaData metaData = importer.spritesheet[i];

                metaData.rect = srcRect;
                metaData.name = $"{srcRect.ToString("F0")}";

                metaDatas.Add(metaData);
            }

            importer.spritesheet = metaDatas.ToArray();
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