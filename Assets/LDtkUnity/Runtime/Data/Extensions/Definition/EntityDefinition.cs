using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class EntityDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <value>
        /// Reference to the tileset that uses this icon. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public TilesetDefinition Tileset => TilesetId == null ? null : LDtkUidBank.GetUidData<TilesetDefinition>(TilesetId.Value); //todo test that this really works by storing inside the uid bank

        /// <value>
        /// The "color" field converted for use with Unity
        /// </value>
        [IgnoreDataMember] public Color UnityColor => Color.ToColor();
        
        /// <value>
        /// Original pixel size
        /// </value>
        [IgnoreDataMember] public Vector2Int UnitySize => new Vector2Int(Width, Height);
        
        /// <value>
        /// Pivot coords (from 0 to 1 for both axes)
        /// </value>
        [IgnoreDataMember] public Vector2 UnityPivot => new Vector2(PivotX, PivotY);
        
        /// <value>
        /// An array of 4 dimensions for the up/right/down/left borders (in this order) when using
        /// 9-slice mode for `tileRenderMode`.<br/>  If the tileRenderMode is not NineSlice, then
        /// this array is empty.<br/>  See: https://en.wikipedia.org/wiki/9-slice_scaling
        /// </value>
        [IgnoreDataMember] public Rect UnityNineSliceBorders => NineSliceBorders.IsNullOrEmpty() ? Rect.zero : NineSliceBorders.ToRect(); //todo implement some importer functionality for this
    }
}