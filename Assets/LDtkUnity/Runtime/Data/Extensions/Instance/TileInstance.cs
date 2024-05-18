using System;
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class TileInstance
    {
        /// <value>
        /// Reference of this tile's rule definition, only for AutoLayers!<br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public AutoLayerRuleDefinition AutoLayerRuleDefinition => LDtkUidBank.GetUidData<AutoLayerRuleDefinition>(AutoLayerRuleId);
        
        /// <value>
        /// Rule definition uid.<br/>
        /// Ensure to get this value from an auto layer context!
        /// </value>
        [IgnoreDataMember] public int AutoLayerRuleId => D[0];
        
        /// <value>
        /// The cell ID of this tile, much like the tile ID in a tileset.
        /// Ensure to get this value from an auto layer context!
        /// </value>
        [IgnoreDataMember] public int AutoLayerCoordId => D[1];
        
        /// <value>
        /// The cell ID of this tile, much like the tile ID in a tileset.
        /// Ensure to get this value from a tile layer context!
        /// </value>
        [IgnoreDataMember] public int TileLayerCoordId => D[0];
        
        /// <value>
        /// X mirror transformation of the tile
        /// </value>
        [IgnoreDataMember] public bool FlipX => new BitArray(BitConverter.GetBytes(F)).Get(0);
        
        /// <value>
        /// Y mirror transformation of the tile
        /// </value>
        [IgnoreDataMember] public bool FlipY => new BitArray(BitConverter.GetBytes(F)).Get(1);
        
        /// <value>
        /// Layer Pixel Position; Pixel coordinates of the tile in the layer. Don't forget optional layer offsets, if they exist!
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityPx => Px.ToVector2Int();
        
        /// <value>
        /// Source Pixel Position; Pixel coordinates of the tile in the tileset
        /// </value>
        [IgnoreDataMember] public Vector2Int UnitySrc => Src.ToVector2Int();
        
        //internal use data only in LDtk, not needed (maybe)
        //public int AutoLayerRuleID => (int)D[0];
        //public int AutoLayerCoordID => (int)D[1];
        //public int TileLayerCoordId => (int)D[0];
    }
}