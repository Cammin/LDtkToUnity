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
            int index = key.IndexOf(MAGIC_KEY, StringComparison.Ordinal) + MAGIC_KEY.Length-1;
            string stringValue = key.Substring(index);

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