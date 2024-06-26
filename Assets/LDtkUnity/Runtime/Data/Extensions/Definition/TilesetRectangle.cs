﻿using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class TilesetRectangle
    {
        /// <value>
        /// Tileset used for this rectangle data. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        //todo while awaiting this fix, just safely not log an error if a definition is not found https://github.com/deepnight/ldtk/issues/1107
        [IgnoreDataMember] public TilesetDefinition Tileset => LDtkUidBank.GetUidData<TilesetDefinition>(TilesetUid, true);
        
        /// <value>
        /// Rectangle of the tile in the Tileset atlas
        /// </value>
        [IgnoreDataMember] public Rect UnityRect => new Rect(X, Y, W, H);
        
        /// <value>
        /// Rectangle of the tile in the Tileset atlas
        /// </value>
        [IgnoreDataMember] public RectInt UnityRectInt => new RectInt(X, Y, W, H);

        protected bool Equals(TilesetRectangle other)
        {
            return H == other.H && 
                   TilesetUid == other.TilesetUid && 
                   W == other.W && 
                   X == other.X && 
                   Y == other.Y;
        }

        public override string ToString() => $"({X}_{Y}, {W}x{H}, uid:{TilesetUid})";
    }
}