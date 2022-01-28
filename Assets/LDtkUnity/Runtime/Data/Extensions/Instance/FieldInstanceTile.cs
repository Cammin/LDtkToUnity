using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class FieldInstanceTile
    {
        /// <value>
        /// Reference to the tileset that uses this tile. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public TilesetDefinition Tileset => LDtkUidBank.GetUidData<TilesetDefinition>(TilesetUid);
        
        /// <value>
        /// Rect that refers to the tile in the tileset image of this enum value's definition
        /// </value>
        [JsonIgnore] public Rect UnityTileSrcRect => SrcRect.ToRect();
    }
}