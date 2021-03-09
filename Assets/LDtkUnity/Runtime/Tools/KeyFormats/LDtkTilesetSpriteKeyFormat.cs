using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    public static class LDtkTilesetSpriteKeyFormat
    {
        private const string MAGIC_KEY_ID = "_id:";
        private const string MAGIC_KEY_X = "_x:";
        private const string MAGIC_KEY_Y = "_y:";
        
        public static string GetKeyFormat(Texture2D tex, Vector2 srcRect)
        {
            return $"{tex.name}_x{srcRect.x}_y{srcRect.y}";
        }

        public static Sprite GetSpriteByMatchingKeyString(Sprite[] sprites, string key, int pixelsPerUnit)
        {
            return null;
            /*if (sprites.NullOrEmpty())
            {
                Debug.LogError("Trying to get sprites when there are none");
                return null;
            }
            
            
            
            string[] keys = key.Split('|');
            
            string identifier = KeyFormatUtil.GetSubstringAfterMagicKey(keys, MAGIC_KEY_ID);
            sprites = sprites.Where(p => p.texture.name == identifier).ToArray();
            
            
            int x = ValueFromAxis(keys, MAGIC_KEY_X);
            if (x == -1)
            {
                return null;
            }
            
            int y = ValueFromAxis(keys, MAGIC_KEY_Y);
            if (y == -1)
            {
                return null;
            }

            Vector2Int coord = new Vector2Int(x, y);
            Vector2Int imageSliceCoord = LDtkToolOriginCoordConverter.ImageSliceCoord(coord, sourceTex.height, pixelsPerUnit);


            return sprites.FirstOrDefault(p => 
                p.texture == sourceTex && 
                p.rect.position == imageSliceCoord);*/
        }

        private static int ValueFromAxis(string[] keys, string magicKey)
        {
            string stringNumber = KeyFormatUtil.GetSubstringAfterMagicKey(keys, magicKey);
            
            if (int.TryParse(stringNumber, out int intValue))
            {
                return intValue;
            }

            Debug.LogError("Failed parsing int");
            return -1;
        }
    }
}