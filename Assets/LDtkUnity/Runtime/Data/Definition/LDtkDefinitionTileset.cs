// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#23-tileset-definition
    public struct LDtkDefinitionTileset
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string identifier;

        /// <summary>
        /// An array of all tiles that are fully opaque (ie. no transparent pixel). Used internally for optimizations.
        /// </summary>
        public int[] opaqueTiles;
        
        /// <summary>
        /// Distance in pixels from image borders
        /// </summary>
        public int padding;
        
        /// <summary>
        /// Image width in pixels
        /// </summary>
        public int pxHei;
        
        /// <summary>
        /// Image width in pixels
        /// </summary>
        public int pxWid;
        
        /// <summary>
        /// Path to the source file, relative to the current project JSON file
        /// </summary>
        public string relPath;
        
        /// <summary>
        /// Space in pixels between all tiles
        /// </summary>
        public int spacing;
        
        /// <summary>
        /// 
        /// </summary>
        public int tileGridSize;
        
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int uid;
    }
}