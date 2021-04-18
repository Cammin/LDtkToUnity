using UnityEngine;

namespace LDtkUnity
{
    public static class LDtkKeyFormatUtil
    {
        public static string IntGridValueFormat(LayerDefinition intGridLayerDef, IntGridValueDefinition def)
        {
            return $"{intGridLayerDef.Identifier}_{def.Value}";
        }
        
        public static string TilesetKeyFormat(Texture2D tex, Vector2 srcRect)
        {
            return $"{tex.name}_x{srcRect.x}_y{srcRect.y}";
        }
    }
}