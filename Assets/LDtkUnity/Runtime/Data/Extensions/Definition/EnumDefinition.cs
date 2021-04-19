using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class EnumDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// Reference to the tileset that uses this icon
        /// </summary>
        [JsonIgnore] public TilesetDefinition IconTileset => IconTilesetUid == null ? null : LDtkUidBank.GetUidData<TilesetDefinition>(IconTilesetUid.Value);
    }
}