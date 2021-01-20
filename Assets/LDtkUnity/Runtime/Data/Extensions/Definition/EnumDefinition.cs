// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;

namespace LDtkUnity.Data
{
    public partial class EnumDefinition : ILDtkUid, ILDtkIdentifier
    {
        public TilesetDefinition IconTileset => IconTilesetUid == null ? null : LDtkProviderUid.GetUidData<TilesetDefinition>(IconTilesetUid.Value);
    }
}