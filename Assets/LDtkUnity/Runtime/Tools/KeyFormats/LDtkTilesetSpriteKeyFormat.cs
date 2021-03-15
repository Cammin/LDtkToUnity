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