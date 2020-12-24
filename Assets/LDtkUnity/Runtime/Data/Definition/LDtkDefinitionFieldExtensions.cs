// ReSharper disable InconsistentNaming

using System;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDefinitionFieldExtensions
    {
        public static LDtkDefinitionFieldDisplayPosition DisplayPosition(this LDtkDefinitionField definition)
        {
            string tried = nameof(LDtkDefinitionFieldDisplayPosition) + "." + definition.editorDisplayPos;
            
            if (Enum.TryParse(tried, out LDtkDefinitionFieldDisplayPosition val))
            {
                return val;
            }

            Debug.LogWarning("LDtk: Field Extension Parse Error");
            return LDtkDefinitionFieldDisplayPosition.Above;

        }

        public static LDtkDefinitionFieldDisplayMode DisplayMode(this LDtkDefinitionField definition)
        {
            string tried = nameof(LDtkDefinitionFieldDisplayMode) + "." + definition.editorDisplayMode;
            
            if (Enum.TryParse(tried, out LDtkDefinitionFieldDisplayMode val))
            {
                return val;
            }

            Debug.LogWarning("LDtk: Field Extension Parse Error");
            return LDtkDefinitionFieldDisplayMode.Hidden;
        }
    }
}