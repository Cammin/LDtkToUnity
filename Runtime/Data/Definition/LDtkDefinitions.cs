// ReSharper disable InconsistentNaming

using System;

namespace LDtkUnity.Runtime.Data.Definition
{
    [Serializable]
    public struct LDtkDefinitions
    {
        /// <summary>
        /// Array of Layer definition
        /// </summary>
        public LDtkDefinitionLayer[] layers;
        
        /// <summary>
        /// Array of Entity definition
        /// </summary>
        public LDtkDefinitionEntity[] entities;
        
        /// <summary>
        /// Array of Tileset definition
        /// </summary>
        public LDtkDefinitionTileset[] tilesets;
        
        /// <summary>
        /// Array of Enum definition
        /// </summary>
        public LDtkDefinitionEnum[] enums;
        
        /// <summary>
        /// Array of Enum definition
        /// Note: external enums are exactly the same as enums, except they have a relPath to point to an external source file.
        /// </summary>
        public LDtkDefinitionEnum[] externalEnums;
    }
}