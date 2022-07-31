using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class NeighbourLevel
    {
        //Using an implicit operator for simpler direct access to the neighbour's level data. (May change if problematic)
        public static implicit operator Level(NeighbourLevel neighbour) => neighbour.Level;
        
        /// <summary>
        /// Reference to the level of this neighbour. 
        /// </summary>
        /// <returns>
        /// The level of this neighbour. <br/>
        /// Make sure to call <see cref="LDtkIidBank"/>.<see cref="LDtkIidBank.CacheIidData"/> first!
        /// </returns>
        public Level Level => LevelIid == null ? null : LDtkIidBank.GetIidData<Level>(LevelIid);

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