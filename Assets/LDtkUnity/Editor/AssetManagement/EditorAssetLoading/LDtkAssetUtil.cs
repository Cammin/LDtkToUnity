using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkAssetUtil
    {
        public static void WriteText(string path, string content)
        {
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.Write(content);
            }
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