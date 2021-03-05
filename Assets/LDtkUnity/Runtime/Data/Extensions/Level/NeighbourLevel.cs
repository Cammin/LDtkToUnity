using UnityEngine.Assertions;

namespace LDtkUnity
{
    public partial class NeighbourLevel
    {
        public Level Level => LDtkProviderUid.GetUidData<Level>(LevelUid);

        public bool IsNorth => IsOrientation('n');
        public bool IsSouth => IsOrientation('s');
        public bool IsEast => IsOrientation('e');
        public bool IsWest => IsOrientation('w');


        private bool IsOrientation(char input) => Dir[0].Equals(input);
    }
}