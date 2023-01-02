using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class NeighbourLevel
    {
        /// <value>
        /// An implicit operator for simpler direct access to the neighbour's level data.
        /// </value>
        /// <param name="neighbour">
        /// The neighbour.
        /// </param>
        /// <returns>
        /// The level.
        /// </returns>
        public static implicit operator Level(NeighbourLevel neighbour) => neighbour.Level;
        
        /// <value>
        /// Reference to the level of this neighbour. 
        /// </value>
        /// <returns>
        /// The level of this neighbour. <br/>
        /// Make sure to call <see cref="LDtkIidBank"/>.<see cref="LDtkIidBank.CacheIidData"/> first!
        /// </returns>
        [IgnoreDataMember] public Level Level => LevelIid == null ? null : LDtkIidBank.GetIidData<Level>(LevelIid);

        /// <value>
        /// Returns true if this neighbour is above the relative level.
        /// </value>
        [IgnoreDataMember] public bool IsNorth => Dir[0].Equals('n');
        
        /// <value>
        /// Returns true if this neighbour is below the relative level.
        /// </value>
        [IgnoreDataMember] public bool IsSouth => Dir[0].Equals('s');
        
        /// <value>
        /// Returns true if this neighbour is to the right of the relative level.
        /// </value>
        [IgnoreDataMember] public bool IsEast => Dir[0].Equals('e');
        
        /// <value>
        /// Returns true if this neighbour is to the left of the relative level.
        /// </value>
        [IgnoreDataMember] public bool IsWest => Dir[0].Equals('w');
    }
}