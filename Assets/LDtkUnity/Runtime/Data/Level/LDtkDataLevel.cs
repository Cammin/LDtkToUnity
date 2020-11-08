// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Level
{
    public struct LDtkDataLevel
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string identifier;
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int uid;
        
        /// <summary>
        /// Width of the level in pixels
        /// </summary>
        public int pxWid;
        
        /// <summary>
        /// Height of the level in pixels
        /// </summary>
        public int pxHei;
        
        /// <summary>
        /// Array of Layer instance
        /// </summary>
        public LDtkDataLayerInstance[] layerInstances;
    }
}
