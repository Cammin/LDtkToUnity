using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Instance Data
    /// </summary>
    public partial class TileInstance
    {
        /// <value>
        /// X mirror transformation of the tile
        /// </value>
        [JsonIgnore] public bool FlipX => new BitArray(BitConverter.GetBytes(F)).Get(0);
        
        /// <value>
        /// Y mirror transformation of the tile
        /// </value>
        [JsonIgnore] public bool FlipY => new BitArray(BitConverter.GetBytes(F)).Get(1);
        
        /// <value>
        /// Layer Pixel Position; Pixel coordinates of the tile in the layer. Don't forget optional layer offsets, if they exist!
        /// </value>
        [JsonIgnore] public Vector2Int UnityPx => Px.ToVector2Int();
        
        /// <value>
        /// Source Pixel Position; Pixel coordinates of the tile in the tileset
        /// </value>
        [JsonIgnore] public Vector2Int UnitySrc => Src.ToVector2Int();
        
        //internal use data only in LDtk, not needed (maybe)
        //public int AutoLayerRuleID => (int)D[0];
        //public int AutoLayerCoordID => (int)D[1];
        //public int TileLayerCoordId => (int)D[0];
    }
}