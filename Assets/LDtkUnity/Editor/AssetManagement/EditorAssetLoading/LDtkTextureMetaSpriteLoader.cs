using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkTextureMetaSpriteLoader
    {
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