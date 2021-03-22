using UnityEngine;

namespace LDtkUnity
{
    public partial class EntityInstanceTile
    {
        /// <summary>
        /// Reference to the tileset that this entity instance tile uses.
        /// </summary>
        public TilesetDefinition TilesetDefinition => LDtkProviderUid.GetUidData<TilesetDefinition>(TilesetUid);
        
        /// <summary>
        /// A rect that refers to the tile in the tileset image
        /// </summary>
        public Rect UnitySourceRect => SrcRect.ToRect();
    }
}