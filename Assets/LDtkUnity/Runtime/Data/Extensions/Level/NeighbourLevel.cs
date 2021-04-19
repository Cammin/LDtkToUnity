using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class NeighbourLevel
    {
        //Using an implicit operator for simpler direct access to the neighbour's level data. (May change if problematic)
        /// <summary>
        /// Reference to the level of this neighbour. 
        /// </summary>
        public static implicit operator Level(NeighbourLevel neighbour) => LDtkUidBank.GetUidData<Level>(neighbour.LevelUid);

        /// <summary>
        /// Returns true if this neighbour is above the relative level.
        /// </summary>
        [JsonIgnore] public bool IsNorth => Dir[0].Equals('n');
        
        /// <summary>
        /// Returns true if this neighbour is below the relative level.
        /// </summary>
        [JsonIgnore] public bool IsSouth => Dir[0].Equals('s');
        
        /// <summary>
        /// Returns true if this neighbour is to the right of the relative level.
        /// </summary>
        [JsonIgnore] public bool IsEast => Dir[0].Equals('e');
        
        /// <summary>
        /// Returns true if this neighbour is to the left of the relative level.
        /// </summary>
        [JsonIgnore] public bool IsWest => Dir[0].Equals('w');
    }
}