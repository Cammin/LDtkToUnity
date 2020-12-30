// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;

namespace LDtkUnity.Data
{
    public static class LDtkDataLevelNeighbourExtensions
    {
        public static LDtkDataLevel LevelReference(this LDtkDataLevelNeighbour data) => LDtkProviderUid.GetUidData<LDtkDataLevel>(data.levelUid);
    }
}