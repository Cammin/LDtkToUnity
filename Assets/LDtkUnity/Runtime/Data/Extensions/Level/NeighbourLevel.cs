using LDtkUnity.Providers;

namespace LDtkUnity
{
    public partial class NeighbourLevel
    {
        public Level LevelReference => LDtkProviderUid.GetUidData<Level>(LevelUid);
    }
}