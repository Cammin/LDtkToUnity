// ReSharper disable InconsistentNaming

using System;

namespace LDtkUnity.Runtime.Data.Definition
{
    [Serializable]
    public struct LDtkDefinitionTileset
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string identifier;

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
        /// Unique Int identifier
        /// </summary>
        public int uid;
    }
}