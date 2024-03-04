using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LayerDefJson)]
    public sealed class LDtkDefinitionObjectIntGridValueGroup : ScriptableObject
    {
        /// <summary>
        /// User defined color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// User defined string identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Group unique ID
        /// </summary>
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