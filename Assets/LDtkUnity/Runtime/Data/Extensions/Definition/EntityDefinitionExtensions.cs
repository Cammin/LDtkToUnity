// ReSharper disable InconsistentNaming

using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class EntityDefinitionExtensions
    {
        public static Color UnityColor(this EntityDefinition definition) => definition.Color.ToColor();
        
        public static Vector2Int UnitySize(this EntityDefinition definition) => new Vector2Int((int)definition.Width, (int)definition.Height);
        
        public static Vector2 UnityPivot(this EntityDefinition definition) => new Vector2((float)definition.PivotX, (float)definition.PivotY);
    }
}