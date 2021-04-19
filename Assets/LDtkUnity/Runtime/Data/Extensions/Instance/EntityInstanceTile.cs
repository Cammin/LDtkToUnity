using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class EntityInstanceTile
    {
        /// <summary>
        /// Reference to the tileset that this entity instance tile uses.
        /// </summary>
        [JsonIgnore] public TilesetDefinition TilesetDefinition => LDtkUidBank.GetUidData<TilesetDefinition>(TilesetUid);
        
        /// <summary>
        /// A rect that refers to the tile in the tileset image
        /// </summary>
        [JsonIgnore] public Rect UnitySourceRect => SrcRect.ToRect();
    }
}