// ReSharper disable InconsistentNaming

using System;
using System.Collections;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDataTileExtensions
    {
        public static bool FlipX(this TileInstance data) => new BitArray(BitConverter.GetBytes(data.F)).Get(0);
        public static bool FlipY(this TileInstance data) => new BitArray(BitConverter.GetBytes(data.F)).Get(1);
        
        public static Vector2Int LayerPixelPosition(this TileInstance data) => data.Px.ToVector2Int();
        public static Vector2Int SourcePixelPosition(this TileInstance data) => data.Src.ToVector2Int();
        
        public static int AutoLayerRuleID(this TileInstance data) => (int)data.D[0];
        public static int AutoLayerCoordID(this TileInstance data) => (int)data.D[1];
        
        public static int TileLayerCoordId(this TileInstance data) => (int)data.D[0];

    }
}