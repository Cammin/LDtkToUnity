using System;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LAYER_DEF_JSON)]
    [Serializable]
    public sealed class LDtkDefinitionObjectIntGridValueGroup
    {
        [field: Tooltip("User defined string identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: Tooltip("User defined color")]
        [field: SerializeField] public Color Color { get; private set; }
        
        [field: Tooltip("Group unique ID")]
        [field: SerializeField] public int Uid { get; private set; }
        
        internal void Populate(LDtkDefinitionObjectsCache cache, IntGridValueGroupDefinition def)
        {
            Color = def.UnityColor;
            Identifier = def.Identifier;
            Uid = def.Uid;
        }
    }
}