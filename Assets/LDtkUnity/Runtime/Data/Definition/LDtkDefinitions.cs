// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Tools;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#2-definitions
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

        
        public LDtkDefinitionLayer GetLayerDefinitionByUID(int uid) => LDtkToolUid.GetDefinitionByUid(uid, layers, item => item.uid);
        public LDtkDefinitionEntity GetEntityDefinitionByUID(int uid) => LDtkToolUid.GetDefinitionByUid(uid, entities, item => item.uid);
        public LDtkDefinitionTileset GetTilesetDefinitionByUID(int uid) => LDtkToolUid.GetDefinitionByUid(uid, tilesets, item => item.uid);
        public LDtkDefinitionEnum GetEnumDefinitionByUID(int uid) => LDtkToolUid.GetDefinitionByUid(uid, enums, item => item.uid);
        public LDtkDefinitionEnum GetExternalEnumDefinitionByUID(int uid) => LDtkToolUid.GetDefinitionByUid(uid, externalEnums, item => item.uid);
        
    }
}