using System;
using System.Collections;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity
{
    public partial class TileInstance
    {
        public bool FlipX => new BitArray(BitConverter.GetBytes(F)).Get(0);
        public bool FlipY => new BitArray(BitConverter.GetBytes(F)).Get(1);
        
        public Vector2Int LayerPixelPosition => Px.ToVector2Int();
        public Vector2Int SourcePixelPosition => Src.ToVector2Int();
        
        public int AutoLayerRuleID => (int)D[0];
        public int AutoLayerCoordID => (int)D[1];
        
        public  int TileLayerCoordId => (int)D[0];

    }
}