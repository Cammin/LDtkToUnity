using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Instance Data
    /// </summary>
    public partial class EntityInstanceTile
    {
        /// <value>
        /// Reference to the tileset that this entity instance tile uses. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public TilesetDefinition TilesetDefinition => LDtkUidBank.GetUidData<TilesetDefinition>(TilesetUid);
        
        /// <value>
        /// A rect that refers to the tile in the tileset image
        /// </value>
        [JsonIgnore] public Rect UnitySourceRect => SrcRect.ToRect();
    }
}