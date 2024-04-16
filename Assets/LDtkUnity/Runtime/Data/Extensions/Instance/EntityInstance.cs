using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Instance Data
    /// </summary>
    public partial class EntityInstance : ILDtkIdentifier, ILDtkIid
    {
        /// <value>
        /// Reference of this instance's definition. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public EntityDefinition Definition => LDtkUidBank.GetUidData<EntityDefinition>(DefUid);
        
        /// <value>
        /// Grid-based coordinates
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityGrid => Grid.ToVector2Int();
        
        /// <value>
        /// Pixel coordinates in current level coordinate space. Don't forget optional layer offsets, if they exist!
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityPx => Px.ToVector2Int();
        
        /// <value>
        /// Pivot coordinates of the Entity. (values are from 0 to 1)
        /// </value>
        [IgnoreDataMember] public Vector2 UnityPivot => Pivot.ToVector2();
        
        /// <value>
        /// The entity "smart" color, guessed from either Entity definition, or one its field
        /// instances.
        /// </value>
        [IgnoreDataMember] public Color UnitySmartColor => string.IsNullOrEmpty(SmartColor) ? Definition.UnityColor : SmartColor.ToColor();
        
        /// <value>
        /// World coordinate in pixels. Only available in GridVania or Free world layouts.
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityWorld => (WorldX != null && WorldY != null) ? new Vector2Int(WorldX.Value, WorldY.Value) : Vector2Int.zero;
        
        /// <value>
        /// Entity size in pixels, adjusted for this instance's resizing.
        /// </value>
        [IgnoreDataMember] public Vector2Int UnitySize => new Vector2Int(Width, Height);
        
        /// <value>
        /// Entity scale multiplier, suitable for a transform's scale.
        /// </value>
        [IgnoreDataMember] public Vector3 UnityScale => new Vector3(Width / (float) Definition.Width, Height / (float) Definition.Height, 1);
    }
}