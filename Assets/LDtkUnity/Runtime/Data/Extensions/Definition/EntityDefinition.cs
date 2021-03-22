using UnityEngine;

namespace LDtkUnity
{
    public partial class EntityDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// The "color" field converted for use with Unity
        /// </summary>
        public Color UnityColor => Color.ToColor();
        
        /// <summary>
        /// Original pixel size
        /// </summary>
        public Vector2Int UnitySize => new Vector2Int((int)Width, (int)Height);
        
        /// <summary>
        /// Pivot coords (from 0 to 1 for both axes)
        /// </summary>
        public Vector2 UnityPivot => new Vector2((float)PivotX, (float)PivotY);
    }
}