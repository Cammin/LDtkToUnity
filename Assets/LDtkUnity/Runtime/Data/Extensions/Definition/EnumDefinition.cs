using System.Runtime.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class EnumDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <value>
        /// Reference to the tileset that uses this icon. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public TilesetDefinition IconTileset => IconTilesetUid == null ? null : LDtkUidBank.GetUidData<TilesetDefinition>(IconTilesetUid.Value);
    }
}