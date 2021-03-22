using System;
using System.Collections;
using UnityEngine;

namespace LDtkUnity
{
    public partial class TileInstance
    {
        /// <summary>
        /// X mirror transformation of the tile
        /// </summary>
        public bool FlipX => new BitArray(BitConverter.GetBytes(F)).Get(0);
        
        /// <summary>
        /// Y mirror transformation of the tile
        /// </summary>
        public bool FlipY => new BitArray(BitConverter.GetBytes(F)).Get(1);
        
        /// <summary>
        /// Pixel coordinates of the tile in the layer. Don't forget optional layer offsets, if they exist!
        /// </summary>
        public Vector2Int LayerPixelPosition => Px.ToVector2Int();
        
        /// <summary>
        /// Pixel coordinates of the tile in the tileset
        /// </summary>
        public Vector2Int SourcePixelPosition => Src.ToVector2Int();
        
        //internal use data only in LDtk, not needed (maybe)
        //public int AutoLayerRuleID => (int)D[0];
        //public int AutoLayerCoordID => (int)D[1];
        //public int TileLayerCoordId => (int)D[0];
    }
}