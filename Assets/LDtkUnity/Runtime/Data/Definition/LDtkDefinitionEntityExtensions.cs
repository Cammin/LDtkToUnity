// ReSharper disable InconsistentNaming

using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDefinitionEntityExtensions
    {
        public static Color Color(this LDtkDefinitionEntity definition) => definition.color.ToColor();
        
        public static Vector2Int Size(this LDtkDefinitionEntity definition) => new Vector2Int(definition.width, definition.height);
        
        public static Vector2 Pivot(this LDtkDefinitionEntity definition) => new Vector2(definition.pivotX, definition.pivotY);
    }
}