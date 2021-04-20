using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class TileInstance
    {
        /// <summary>
        /// X mirror transformation of the tile
        /// </summary>
        [JsonIgnore] public bool FlipX => new BitArray(BitConverter.GetBytes(F)).Get(0);
        
        /// <summary>
        /// Y mirror transformation of the tile
        /// </summary>
        [JsonIgnore] public bool FlipY => new BitArray(BitConverter.GetBytes(F)).Get(1);
        
        /// <summary>
        /// Layer Pixel Position; Pixel coordinates of the tile in the layer. Don't forget optional layer offsets, if they exist!
        /// </summary>
        [JsonIgnore] public Vector2Int UnityPx => Px.ToVector2Int();
        
        /// <summary>
        /// Source Pixel Position; Pixel coordinates of the tile in the tileset
        /// </summary>
        [JsonIgnore] public Vector2Int UnitySrc => Src.ToVector2Int();
        
        //internal use data only in LDtk, not needed (maybe)
        //public int AutoLayerRuleID => (int)D[0];
        //public int AutoLayerCoordID => (int)D[1];
        //public int TileLayerCoordId => (int)D[0];
    }
}