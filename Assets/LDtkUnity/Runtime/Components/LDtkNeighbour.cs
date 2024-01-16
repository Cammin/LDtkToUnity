using System;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable]
    public sealed class LDtkNeighbour
    {
        internal const string PROPERTY_IDENTIFIER = nameof(_identifier);
        internal const string PROPERTY_DIR = nameof(_dir);
        internal const string PROPERTY_LEVEL_IID = nameof(_levelIid);
        
        [SerializeField] private string _identifier;
        [SerializeField] private string _dir;
        [SerializeField] private string _levelIid;

        internal LDtkNeighbour(NeighbourLevel lvl)
        {
            //this is possible with separate level files because the levels still have their surface level root info in the project
            _identifier = lvl.Level.Identifier;
            _dir = lvl.Dir;
            _levelIid = lvl.LevelIid;
        }
        
        /// <value>
        /// User defined unique identifier
        /// </value>
        public string Identifier => _identifier;
        
        /// <value>
        /// A lowercase string tipping on the level location (`n`orth, `s`outh, `w`est,
        /// `e`ast).<br/>  Since 1.4.0, this value can also be `lessThan` (neighbour depth is lower), `greaterThan`
        /// (neighbour depth is greater) or `o` (levels overlap and share the same world
        /// depth).<br/>  Since 1.5.3, this value can also be `nw`,`ne`,`sw` or `se` for levels only
        /// touching corners.
        /// </value>
        public string Dir => _dir;
        
        /// <value>
        /// Neighbour Instance Identifier
        /// </value>
        public string LevelIid => _levelIid;

        /// <value>
        /// Returns true if this neighbour is above the relative level.
        /// </value>
        public bool IsNorth => _dir.Contains("n");
        
        /// <value>
        /// Returns true if this neighbour is below the relative level.
        /// </value>
        public bool IsSouth => _dir.Contains("s");
        
        /// <value>
        /// Returns true if this neighbour is to the right of the relative level.
        /// </value>
        public bool IsEast => _dir.Contains("e");
        
        /// <value>
        /// Returns true if this neighbour is to the left of the relative level.
        /// </value>
        public bool IsWest => _dir.Contains("w");
        
        /// <value>
        /// Returns true if this neighbour has a lesser depth
        /// </value>
        public bool IsAbove => _dir.Equals("<");
        
        /// <value>
        /// Returns true if this neighbour has a greater depth
        /// </value>
        public bool IsBelow => _dir.Equals(">");
        
        /// <value>
        /// Returns true if this neighbour overlaps the other
        /// </value>
        public bool IsOverlap => _dir.Equals("o");
        
        /// <returns>
        /// Finds the iid Level GameObject.
        /// </returns>
        /// <remarks>
        /// This function uses Object.FindObjectsOfType if a cached component is not found, so it is slow and not recommended to use every frame. However if the object is found, it is cached. <br/>
        /// In most cases you can use <see cref="GetLevel"/> instead as long as the object you are looking for is active. If the object you are looking for is inactive, you can try this.
        /// </remarks>
        public LDtkIid FindLevel() => LDtkIidComponentBank.FindObjectOfIid(_levelIid);

        /// <returns>
        /// Gets the neighbour gameobject that matches the iid.
        /// </returns>
        /// <remarks>
        /// The objects are only available after their OnEnable. Otherwise, try using <see cref="FindLevel"/>. <br/>
        /// If the iid component exists but this returned null, then make sure the referenced component is active and accessed after it's OnEnable.
        /// </remarks>
        public LDtkIid GetLevel() => LDtkIidComponentBank.GetByIid(_levelIid);
    }
}