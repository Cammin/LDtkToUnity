// ReSharper disable InconsistentNaming

using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDefinitionIntGridValueExtensions
    {
        public static Color Color(this LDtkDefinitionIntGridValue definition) => definition.color.ToColor();
    }
}