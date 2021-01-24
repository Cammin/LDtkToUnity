using LDtkUnity.Providers;

namespace LDtkUnity
{
    public partial class EnumDefinition : ILDtkUid, ILDtkIdentifier
    {
        public TilesetDefinition IconTileset => IconTilesetUid == null ? null : LDtkProviderUid.GetUidData<TilesetDefinition>(IconTilesetUid.Value);
    }
}