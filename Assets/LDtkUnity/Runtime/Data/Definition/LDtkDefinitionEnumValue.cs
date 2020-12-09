// ReSharper disable InconsistentNaming

using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDefinitionEnumValueExtensions
    {
        public static Rect SourceRect(this LDtkDefinitionEnumValue definition) => definition.__tileSrcRect.ToRect();
    }
}