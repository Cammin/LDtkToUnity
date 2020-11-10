// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#1-level
    public struct LDtkDataLevel
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string identifier;
        
        /// <summary>
        /// Array of Layer instance
        /// </summary>
        public LDtkDataLayerInstance[] layerInstances;
        
        /// <summary>
        /// Height of the level in pixels
        /// </summary>
        public int pxHei;
        
        /// <summary>
        /// Width of the level in pixels
        /// </summary>
        public int pxWid;
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int uid;
    }
}
