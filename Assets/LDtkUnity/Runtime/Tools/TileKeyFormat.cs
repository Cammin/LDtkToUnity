#pragma warning disable CS0659

using System;
using UnityEngine;
#pragma warning disable CS0659

namespace LDtkUnity
{
    internal struct TileKeyFormat : IEquatable<TileKeyFormat>
    {
        public string AssetName;
        public Rect SrcRect;

        public bool Equals(TileKeyFormat other)
        {
            return AssetName == other.AssetName && SrcRect.Equals(other.SrcRect);
        }

        public override bool Equals(object obj)
        {
            return obj is TileKeyFormat other && Equals(other);
        }

        public override string ToString()
        {
            return $"{AssetName}_{SrcRect.x}_{SrcRect.y}_{SrcRect.width}_{SrcRect.height}";
        }
    }
}