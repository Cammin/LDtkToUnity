using System;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LayerDefJson)]
    [Serializable]
    public sealed class LDtkDefinitionObjectIntGridValue
    {
        public Color Color { get; private set; }
        
        [field: Tooltip("Parent group identifier (can be null)")]
        public LDtkDefinitionObjectIntGridValueGroup Group { get; private set; }
        
        [field: Tooltip("User defined unique identifier")]
        public string Identifier { get; private set; }

        public LDtkDefinitionObjectTilesetRectangle Tile { get; private set; }
        
        [field: Tooltip("The IntGrid value itself")]
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