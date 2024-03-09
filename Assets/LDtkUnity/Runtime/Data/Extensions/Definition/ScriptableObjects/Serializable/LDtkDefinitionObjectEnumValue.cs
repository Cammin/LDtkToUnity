using System;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_EnumDefValues)]
    [Serializable]
    public sealed class LDtkDefinitionObjectEnumValue
    {
        [field: Tooltip("Enum value")]
        [field: SerializeField] public string Id { get; private set; }
        
        [field: Tooltip("Optional color")]
        [field: SerializeField] public Color Color { get; private set; }
        
        [field: Tooltip("Optional tileset rectangle to represents this value")]
        [field: SerializeField] public LDtkDefinitionObjectTilesetRectangle TileRect { get; private set; }
        
        internal LDtkDefinitionObjectEnumValue(LDtkDefinitionObjectsCache cache, EnumValueDefinition def)
        {
            Color = def.UnityColor;
            Id = def.Id;
            if (def.TileRect != null)
            {
                //todo create this from the scope of the cache object instead.
                TileRect = ScriptableObject.CreateInstance<LDtkDefinitionObjectTilesetRectangle>();
                TileRect.Populate(cache, def.TileRect);
            }
        }
    }
}