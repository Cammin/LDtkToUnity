using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// Json Level Data
    /// </summary>
    public partial class NeighbourLevel
    {
        //Using an implicit operator for simpler direct access to the neighbour's level data. (May change if problematic)
        /// <summary>
        /// Reference to the level of this neighbour. 
        /// </summary>
        /// <param name="neighbour">
        /// The neighbour.
        /// </param>
        /// <returns>
        /// The level of this neighbour. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </returns>
        public static implicit operator Level(NeighbourLevel neighbour) => LDtkUidBank.GetUidData<Level>(neighbour.LevelUid);

        /// <value>
        /// Returns true if this neighbour is above the relative level.
        /// </value>
        [JsonIgnore] public bool IsNorth => Dir[0].Equals('n');
        
        /// <value>
        /// Returns true if this neighbour is below the relative level.
        /// </value>
        [JsonIgnore] public bool IsSouth => Dir[0].Equals('s');
        
        /// <value>
        /// Returns true if this neighbour is to the right of the relative level.
        /// </value>
        [JsonIgnore] public bool IsEast => Dir[0].Equals('e');
        
        /// <value>
        /// Returns true if this neighbour is to the left of the relative level.
        /// </value>
        [JsonIgnore] public bool IsWest => Dir[0].Equals('w');
    }
}