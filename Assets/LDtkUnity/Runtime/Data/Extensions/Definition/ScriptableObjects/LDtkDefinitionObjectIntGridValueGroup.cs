using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LayerDefJson)]
    public sealed class LDtkDefinitionObjectIntGridValueGroup : ScriptableObject
    {
        [field: Tooltip("User defined color")]
        public Color Color { get; private set; }
        
        [field: Tooltip("User defined string identifier")]
        public string Identifier { get; private set; }
        
        [field: Tooltip("Group unique ID")]
        public int Uid { get; private set; }
        
        internal void Populate(LDtkDefinitionObjectsCache cache, IntGridValueGroupDefinition def)
        {
            name = $"IntGridValueGroup_{def.Identifier}";
            
            Color = def.UnityColor;
            Identifier = def.Identifier;
            Uid = def.Uid;
        }
    }
}