// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;

namespace LDtkUnity.Data
{
    public static class EnumDefinitionExtensions
    {
        public static TilesetDefinition IconTileset(this EnumDefinition iconTileset)
        {
            return iconTileset.IconTilesetUid == null ? null : LDtkProviderUid.GetUidData<TilesetDefinition>(iconTileset.IconTilesetUid.Value);
        }
    }
}