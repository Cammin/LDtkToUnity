// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#112-entity-instance
    public struct LDtkDataEntity
    {
        /// <summary>
        /// Grid-based coordinates ([x,y] format)
        /// </summary>
        public int[] __grid;

        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string __identifier;

        /// <summary>
        /// Optional Tile used to display this entity (it could either be the default Entity tile, or some tile provided by a field value, like an Enum).
        /// </summary>
        public LDtkDataEntityTile __tile;
        
        /// <summary>
        /// Reference of the Entity definition UID
        /// </summary>
        public int defUid;

        /// <summary>
        /// Array of Field instance
        /// </summary>
        public LDtkDataField[] fieldInstances;

        /// <summary>
        /// Pixel coordinates ([x,y] format). Don't forget optional layer offsets, if they exist!
        /// </summary>
        public int[] px;
    }
}