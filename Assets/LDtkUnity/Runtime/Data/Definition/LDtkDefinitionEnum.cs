// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#24-enum-definition
    public struct LDtkDefinitionEnum
    {
        /// <summary>
        /// Relative path to the external file providing this Enum
        /// </summary>
        public string externalRelPath;
        
        /// <summary>
        /// Tileset UID if provided
        /// </summary>
        public int? iconTilesetUid;
        
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string identifier;
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int uid;
        
        /// <summary>
        /// All possible enum values, with their optional Tile infos.
        /// </summary>
        public LDtkDefinitionEnumValue[] values;
    }
}