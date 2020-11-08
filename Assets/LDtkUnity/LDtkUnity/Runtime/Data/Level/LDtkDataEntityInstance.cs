// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Level
{
    public struct LDtkDataEntityInstance
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string __identifier;
        
        /// <summary>
        /// Grid-based coordinates ([x,y] format)
        /// </summary>
        public int[] __grid;

        /// <summary>
        /// Optional Tile used to display this entity (it could either be the default Entity tile, or some tile provided by a field value, like an Enum).
        /// </summary>
        //public LEdEntityInstanceTile __tile; //TODO, not json convertable yet
        
        /// <summary>
        /// Reference of the Entity definition UID
        /// </summary>
        public int defUid;
        
        /// <summary>
        /// Pixel coordinates ([x,y] format)
        /// </summary>
        public int[] px;

        /// <summary>
        /// Array of Field instance
        /// </summary>
        public LDtkDataEntityInstanceField[] fieldInstances;
    }
}