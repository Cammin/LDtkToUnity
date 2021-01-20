// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;

namespace LDtkUnity
{
    public partial class LdtkNeighbourLevel
    {
        public Level LevelReference => LDtkProviderUid.GetUidData<Level>(LevelUid);
    }
}