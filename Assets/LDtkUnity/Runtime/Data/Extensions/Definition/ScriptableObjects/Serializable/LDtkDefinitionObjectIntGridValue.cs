using System;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LAYER_DEF_JSON)]
    [Serializable]
    public sealed class LDtkDefinitionObjectIntGridValue
    {
        [field: Tooltip("User defined unique identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: SerializeField] public Color Color { get; private set; }
        
        [field: Tooltip("Parent group identifier (can be null)")]
        [field: SerializeField] public int GroupUid { get; private set; }
        
        [field: SerializeField] public Sprite Tile { get; private set; }
        
        [field: Tooltip("The IntGrid value itself")]
        [field: SerializeField] public int Value { get; private set; }
        
        internal void Populate(LDtkDefinitionObjectsCache cache, IntGridValueDefinition def)
        {
            Color = def.UnityColor;
            GroupUid = def.GroupUid; //todo add support for the actual group object reference
            Identifier = def.Identifier;
            Tile = cache.GetSpriteForTilesetRectangle(def.Tile);
            Value = def.Value;
        }
    }
}