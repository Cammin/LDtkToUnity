using System;

namespace LDtkUnity
{
    public static class KeyFormatUtil
    {
        public static string GetSubstringAfterMagicKey(string[] sources, string magicKey)
        {
            foreach (string token in sources)
            {
                if (!token.Contains(magicKey))
                {
                    continue;
                }
                
                return GetSubstringAfterMagicKey(token, magicKey);
            }

            return string.Empty;
        }
        public static string GetSubstringAfterMagicKey(string source, string magicKey)
        {
            int index = source.IndexOf(magicKey, StringComparison.Ordinal) + magicKey.Length - 1;
            return source.Substring(index);
        }
    }
}