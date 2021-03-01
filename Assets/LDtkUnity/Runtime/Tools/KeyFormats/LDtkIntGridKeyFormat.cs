using System;
using UnityEngine;

namespace LDtkUnity.Tools
{
    public static class LDtkIntGridKeyFormat
    {
        private const string MAGIC_KEY = "_IGV:";
        
        public static string GetKeyFormat(LayerDefinition intGridLayerDef, IntGridValueDefinition def)
        {
            return $"{intGridLayerDef.Identifier}{MAGIC_KEY}{def.Value}";
        }

        public static int GetValueFromKey(string key)
        {
            string stringValue = KeyFormatUtil.GetSubstringAfterMagicKey(key, MAGIC_KEY);

            Debug.Log("Getting IntGridValue " + stringValue);

            if (int.TryParse(stringValue, out int value))
            {
                return value;
            }

            Debug.LogError("Incorrect int parse when getting Int Grid Value");
            return 0;
        }
    }
}