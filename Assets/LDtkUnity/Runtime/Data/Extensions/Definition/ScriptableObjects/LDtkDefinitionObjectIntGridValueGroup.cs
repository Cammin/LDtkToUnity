using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LayerDefJson)]
    public sealed class LDtkDefinitionObjectIntGridValueGroup : ScriptableObject
    {
        [field: Tooltip("User defined color")]
        public Color Color { get; set; }
        
        [field: Tooltip("User defined string identifier")]
        public string Identifier { get; set; }
        
        [field: Tooltip("Group unique ID")]
        public int Uid { get; set; }
        
        internal void Populate(LDtkDefinitionObjectsCache cache, IntGridValueGroupDefinition def)
        {
            name = $"IntGridValueGroup_{def.Identifier}";
            
            Color = def.UnityColor;
            Identifier = def.Identifier;
            Uid = def.Uid;
        }
    }
}