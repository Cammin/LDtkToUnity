// ReSharper disable InconsistentNaming

using System;
using System.Collections;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDataTileExtensions
    {
        public static bool FlipX(this LDtkDataTile data) => new BitArray(BitConverter.GetBytes(data.f)).Get(0);
        public static bool FlipY(this LDtkDataTile data) => new BitArray(BitConverter.GetBytes(data.f)).Get(1);
        
        public static Vector2Int LayerPixelPosition(this LDtkDataTile data) => data.px.ToVector2Int();
        public static Vector2Int SourcePixelPosition(this LDtkDataTile data) => data.src.ToVector2Int();
        
        public static int AutoLayerRuleID(this LDtkDataTile data) => data.d[0];
        public static int AutoLayerCoordID(this LDtkDataTile data) => data.d[1];
        
        public static int TileLayerCoordId(this LDtkDataTile data) => data.d[0];

    }
}