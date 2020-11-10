// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#22-entity-definition
    public struct LDtkDefinitionEntity
    {
        /// <summary>
        /// Base entity color
        /// </summary>
        public string color;
        
        /// <summary>
        /// Array of field definitions
        /// </summary>
        public LDtkDefinitionField[] fieldDefs;
        
        /// <summary>
        /// Pixel height
        /// </summary>
        public int height;
        
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string identifier;
        
        /// <summary>
        /// Max instances per level
        /// </summary>
        public int maxPerLevel;
        
        /// <summary>
        /// Pivot X coordinate (from 0 to 1.0)
        /// </summary>
        public float pivotX;
        
        /// <summary>
        /// Pivot Y coordinate (from 0 to 1.0)
        /// </summary>
        public float pivotY;
        
        /// <summary>
        /// Tile ID used for optional tile display
        /// </summary>
        public int tileId;
        
        /// <summary>
        /// Tileset ID used for optional tile display
        /// </summary>
        public int tilesetId;
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int uid;
        
        /// <summary>
        /// Pixel width
        /// </summary>
        public int width;
    }
}