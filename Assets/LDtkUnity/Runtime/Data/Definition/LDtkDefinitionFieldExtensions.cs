// ReSharper disable InconsistentNaming

using System;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDefinitionFieldExtensions
    {
        public static LDtkDefinitionFieldDisplayPosition DisplayPosition(this LDtkDefinitionField definition)
        {
            return GetEnum<LDtkDefinitionFieldDisplayPosition>(definition);
        }

        public static LDtkDefinitionFieldDisplayMode DisplayMode(this LDtkDefinitionField definition)
        {
            return GetEnum<LDtkDefinitionFieldDisplayMode>(definition);
        }
        
        private static T GetEnum<T>(LDtkDefinitionField definition) where T : struct
        {
            string tried = definition.editorDisplayMode;

            //Debug.Log($"{tried}");
            if (Enum.TryParse(tried, out T val))
            {
                return val;
            }

            Debug.LogWarning("LDtk: Field Extension Parse Error");
            return default;
        }
    }
}