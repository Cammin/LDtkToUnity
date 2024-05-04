using System;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_ENUM_DEF_VALUES)]
    [Serializable]
    public sealed class LDtkDefinitionObjectEnumValue
    {
        [field: Tooltip("Enum value")]
        [field: SerializeField] public string Id { get; private set; }
        
        [field: Tooltip("Optional color")]
        [field: SerializeField] public Color Color { get; private set; }
        
        [field: Tooltip("Optional tileset rectangle to represents this value")]
        [field: SerializeField] public Sprite TileRect { get; private set; }
        
        internal void Populate(LDtkDefinitionObjectsCache cache, EnumValueDefinition def)
        {
            Color = def.UnityColor;
            Id = def.Id;
            TileRect = cache.GetSpriteForTilesetRectangle(def.TileRect);
        }
    }
}