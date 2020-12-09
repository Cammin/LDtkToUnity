// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;

namespace LDtkUnity.Data
{
    public static class LDtkDefinitionEnumExtensions
    {
        public static LDtkDefinitionTileset GetIconTileset(this LDtkDefinitionEnum iconTileset) => LDtkProviderUid.GetUidData<LDtkDefinitionTileset>(iconTileset.iconTilesetUid);
    }
}