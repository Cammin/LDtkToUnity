using Newtonsoft.Json;
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
        [JsonIgnore] public TilesetDefinition Tileset => TilesetId == null ? null : LDtkUidBank.GetUidData<TilesetDefinition>(TilesetId.Value); //todo test that this works some time
        
        /// <value>
        /// Reference to the tile that this icon uses. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public EntityInstanceTile Tile => null; //todo need to figure out the right type first
        
        /// <value>
        /// The "color" field converted for use with Unity
        /// </value>
        [JsonIgnore] public Color UnityColor => Color.ToColor();
        
        /// <value>
        /// Original pixel size
        /// </value>
        [JsonIgnore] public Vector2Int UnitySize => new Vector2Int((int)Width, (int)Height);
        
        /// <value>
        /// Pivot coords (from 0 to 1 for both axes)
        /// </value>
        [JsonIgnore] public Vector2 UnityPivot => new Vector2((float)PivotX, (float)PivotY);
    }
}