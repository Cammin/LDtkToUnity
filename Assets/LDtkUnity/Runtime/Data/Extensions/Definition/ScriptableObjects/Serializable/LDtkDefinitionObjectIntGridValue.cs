using System;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LayerDefJson)]
    [Serializable]
    public sealed class LDtkDefinitionObjectIntGridValue
    {
        public Color Color { get; private set; }

        /// <summary>
        /// Parent group identifier (can be null)
        /// </summary>
        [field: Tooltip("")]
        public LDtkDefinitionObjectIntGridValueGroup Group { get; private set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [field: Tooltip("")]
        public string Identifier { get; private set; }

        public LDtkDefinitionObjectTilesetRectangle Tile { get; private set; }

        /// <summary>
        /// The IntGrid value itself
        /// </summary>
        [field: Tooltip("")]
        public int Value { get; private set; }
        
        internal LDtkDefinitionObjectIntGridValue(LDtkDefinitionObjectsCache cache, IntGridValueDefinition def)
        {
            Color = def.UnityColor;
            Group = cache.GetObject(cache.IntGridValueGroups, def.GroupUid == 0 ? (int?)null : def.GroupUid);
            if (def.Tile != null)
            {
                Tile = ScriptableObject.CreateInstance<LDtkDefinitionObjectTilesetRectangle>();
                Tile.Populate(cache, def.Tile);
            }
            Value = def.Value;
        }
    }
}