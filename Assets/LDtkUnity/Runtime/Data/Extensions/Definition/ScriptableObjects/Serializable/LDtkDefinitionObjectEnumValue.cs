using System;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_EnumDefValues)]
    [Serializable]
    public sealed class LDtkDefinitionObjectEnumValue
    {
        /// <summary>
        /// Enum value
        /// </summary>
        [field: Tooltip("")]
        [field: SerializeField] public string Id { get; private set; }
        
        /// <summary>
        /// Optional color
        /// </summary>
        [field: Tooltip("")]
        [field: SerializeField] public Color Color { get; private set; }

        /// <summary>
        /// Optional tileset rectangle to represents this value
        /// </summary>
        [field: Tooltip("")]
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