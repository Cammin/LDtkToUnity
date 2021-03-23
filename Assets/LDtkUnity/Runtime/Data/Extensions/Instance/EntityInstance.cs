using UnityEngine;

namespace LDtkUnity
{
    public partial class EntityInstance : ILDtkIdentifier
    {
        /// <summary>
        /// Reference of this instance's definition.
        /// </summary>
        public EntityDefinition Definition => LDtkUidBank.GetUidData<EntityDefinition>(DefUid);
        
        /// <summary>
        /// Pixel coordinates in current level coordinate space. Don't forget optional layer offsets, if they exist!
        /// </summary>
        public Vector2Int UnityPx => Px.ToVector2Int();
        
        /// <summary>
        /// Pivot coordinates of the Entity. (values are from 0 to 1)
        /// </summary>
        public Vector2 UnityPivot => Pivot.ToVector2();
        
        /// <summary>
        /// Grid-based coordinates
        /// </summary>
        public Vector2Int UnityGrid => Grid.ToVector2Int();
        
        /// <summary>
        /// Entity size in pixels, adjusted for this instance's resizing.
        /// </summary>
        public Vector2Int UnitySize => new Vector2Int((int)Width, (int)Height);
    }
}